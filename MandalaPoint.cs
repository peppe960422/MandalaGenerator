using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandalaGenerator
{
    class MandalaPoint
    {
        public static float GlobalBass;
        public static float KickFlash;
        public List<MandalaPoint> Childs = new List<MandalaPoint>();

        public TypeOfMandalaMovement angularVelocityType { get; set; }
        public TypeOfMandalaMovement speedType { get; set; }

        public TypeOfMandalaGeneretionalProgress GeneretionalProgress { get; set; }
        Func<double> SpeedAngularFunc { get { return MandalaFuncs.AngularAccelerationStrategies[(int)angularVelocityType]; } }
        Func<double> SpeedFunc { get { return MandalaFuncs.AccelerationStrategies[(int)speedType]; } }

        Action<Graphics, MandalaPoint, float, Func<double, double, double, Color>>
            DrawAction
        { get; set; } = MandalaFuncs.DrawingStrategies[(int)TypeOfMusicAnimation.Explode];

        public double angle;
        public double distance;
        public double speed;
        double rotationSpeed;

        public int depth;

        public PointF position;
        PointF center;

        static Random rnd = new Random();

        public MandalaPoint(
            PointF c,
            double angle,
            double distance,
            int generation,
            int childs, TypeOfMandalaMovement movementPattern,
            TypeOfMandalaGeneretionalProgress generationalProgress,
            Action<Graphics, MandalaPoint, float, Func<double, double, double, Color>> DrawAction
           )
        {
            center = c;
            speedType = movementPattern;
            angularVelocityType = movementPattern;
            this.angle = angle;
            this.distance = distance;
            this.GeneretionalProgress = generationalProgress;
            speed = SpeedFunc();
            this.DrawAction = DrawAction;
            rotationSpeed = SpeedAngularFunc.Invoke();

            depth = generation;

            UpdatePosition();

            if (generation <= 0)
                return;

            int tchilds = MandalaFuncs.ChildrenFunc[(int)GeneretionalProgress](childs, generation);

            for (int i = 0; i < tchilds; i++)
            {
                double a =
                    angle +
                    ((Math.PI * 2) / tchilds) * i;

                Childs.Add(
                    new MandalaPoint(
                        position,
                        a,
                        distance * 0.7,
                        generation - 1, tchilds, movementPattern, generationalProgress, DrawAction));
            }
        }

        void UpdatePosition()
        {
            position = new PointF(
                center.X + (float)(Math.Cos(angle) * distance),
                center.Y + (float)(Math.Sin(angle) * distance));
        }

        public void Update(float time)
        {
            angle += rotationSpeed;

            distance += 0.5 + Math.Sin(time + angle) * 0.1;

            ;
            rotationSpeed = 0.01;
            ;

            UpdatePosition();

            foreach (var c in Childs)
            {
                c.center = position;
                c.Update(time + 0.1f);
            }
        }

        Color HSV(double h, double s, double v)
        {
            int hi = Convert.ToInt32(Math.Floor(h / 60)) % 6;
            double f = h / 60 - Math.Floor(h / 60);

            v *= 255;
            int vi = Convert.ToInt32(v);
            int p = Convert.ToInt32(v * (1 - s));
            int q = Convert.ToInt32(v * (1 - f * s));
            int t = Convert.ToInt32(v * (1 - (1 - f) * s));

            return hi switch
            {
                0 => Color.FromArgb(vi, t, p),
                1 => Color.FromArgb(q, vi, p),
                2 => Color.FromArgb(p, vi, t),
                3 => Color.FromArgb(p, q, vi),
                4 => Color.FromArgb(t, p, vi),
                _ => Color.FromArgb(vi, p, q),
            };
        }

        public void Draw(Graphics g, float time)
        {
            DrawAction(g, this, time, HSV);
        }
    }
}
