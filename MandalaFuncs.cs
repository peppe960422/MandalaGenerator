using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandalaGenerator
{
    public enum TypeOfMandalaMovement { Const, Random }

    public enum TypeOfMusicAnimation { Normal, Explode }

    public enum TypeOfSchemaAnimation { Single , Triple , Five }
    public enum TypeOfMandalaGeneretionalProgress { Const, Prograssive, Regressive, Random, Div2, GenerationDependent, SinWave, Explosion, Organic, SpiralGrowth, Fibonacci, HeartBeat }
    class MandalaFuncs
    {
        public static Func<double>[] AccelerationStrategies = { CostantVelocity, RandomVelocity };
        public static Func<double>[] AngularAccelerationStrategies = { CostantAngle, RandomAngle };
        public static Func<int, int, int>[] ChildrenFunc = { ConstantChildren, IncreasingLinear, DecreasingLinear, RandomChilds, Divide2Incareasing, GenerationDependent, SinWave, Explosion, Organic, SpiralGrowth, Fibonacci, HeartBeat };
        public static Action<Graphics, MandalaPoint, float, Func<double, double, double, Color>>[] DrawingStrategies =
            {DrawNormal,DrawExplode };
        public static Func< int , int ,Point[] >[] SchemaStrategies = { SingleSchema, TripleSchema, FiveSchema };

        private static Point[] FiveSchema( int height, int width)
        {
            return new Point[] { new Point(width / 4, height / 4), new Point((width / 4) * 3, height / 4),
                                 new Point (width / 4, (height / 4)*3), new Point((width / 4) * 3, (height / 4)*3), 
                                 new Point(width / 2, height / 2) };
        }

        private static Point[] TripleSchema(int height , int width )
        {
            return new Point[] { new Point(width / 4, height / 2), new Point((width / 4 )*3, height / 2), new Point(width / 2, height / 2) };
        }

        private static Point[] SingleSchema( int height, int width)
        {
            return new Point[] { new Point(width / 2, height / 2) };
        }

        static Random rnd = new Random();
        static public void DrawNormal(Graphics g, MandalaPoint point, float time,
            Func<double, double, double, Color> colorFunc)
        {
            double hue = (time * 40 + point.depth * 30) % 360;
            double v = MandalaPoint.GlobalBass * 2 + 0.3    
                ;
            if (v > 1) { v = 1; }
            Color c = colorFunc(hue, 1, v);

            using Pen p = new Pen(c, Math.Max(1, point.depth));

            foreach (var child in point.Childs)
            {
                g.DrawLine(
                    p,
                    point.position,
                    child.position);

                child.Draw(g, time);
            }

        }
        static public void DrawExplode(Graphics g, MandalaPoint point, float time,
            Func<double, double, double, Color> colorFunc)
        {
            double bassNorm = Math.Min(1.0, MandalaPoint.GlobalBass * 0.4);
            double kick = MandalaPoint.KickFlash;

            double hue = (time * 40 + point.depth * 30 + kick * 120 + bassNorm * 60) % 360;
            double sat = Math.Max(0.6, 1.0 - bassNorm * 0.3);
            double v = Math.Min(1.0, 0.55 + kick * 0.45 + bassNorm * 0.25);

            Color c = colorFunc(hue, sat, v);

            float penWidth = Math.Max(1, point.depth) + (float)(kick * 2.5);
            using Pen p = new Pen(c, penWidth);

            foreach (var child in point.Childs)
            {
                g.DrawLine(
                p,
                    point.position,
                    child.position);
                child.Draw(g, time);
            }

            float size =
                4 + (float)Math.Sin(time * 3 + point.angle) * 2
                  + (float)(kick * 5)
                  + (float)(bassNorm * 3);

            if (kick > 0.15)
            {
                int glowAlpha = (int)Math.Min(140, kick * 180);
                Color glow = Color.FromArgb(glowAlpha, c);
                using Brush gb = new SolidBrush(glow);
                float gsize = size * (2.2f + (float)kick);
                g.FillEllipse(
                    gb,
                    point.position.X - gsize / 2,
                    point.position.Y - gsize / 2,
                    gsize,
                    gsize);
            }

            using Brush b = new SolidBrush(c);

            g.FillEllipse(
                b,
                point.position.X - size / 2,
                point.position.Y - size / 2,
                size,
                size);
        }

        static double RandomVelocity() { return 0.05 + rnd.NextDouble(); }
        static double CostantVelocity() { return 0.01; }
        static double RandomAngle() { return 0.015 + rnd.NextDouble(); }
        static double CostantAngle() { return 0.01; }

        static int ConstantChildren(int childs, int generation) { return childs; }

        static int IncreasingLinear(int childs, int generation) { return childs + generation / 2; }

        static int DecreasingLinear(int childs, int generation) { return Math.Max(1, childs - generation / 2); ; }

        static int Divide2Incareasing(int childs, int generation) { return (int)Math.Ceiling((double)childs / 2); }



        static int RandomChilds(int childs, int generation) { return rnd.Next(1, 4); }

        static int GenerationDependent(int childs, int generation) { return generation; }

        static int SinWave(int childs, int generation)
        {
            return Math.Max(
                1,
                childs +
                (int)(Math.Sin(generation) * 3));
        }
        static int SpiralGrowth(int childs, int generation)
        {
            return 1 + (generation % 6);
        }
        static int Explosion(int childs, int generation)
        {
            return (int)Math.Pow(2, generation / 3.0);
        }
        static int Organic(int childs, int generation)
        {
            return Math.Max(
                1,
                childs +
                rnd.Next(-1, 3));
        }
        static int Fibonacci(int childs, int generation)
        {
            int a = 1;
            int b = 1;

            for (int i = 0; i < generation; i++)
            {
                int t = a + b;
                a = b;
                b = t;
            }

            return Math.Min(8, a);
        }
        static int HeartBeat(int childs, int generation)
        {
            return generation % 2 == 0
                ? childs * 2
                : childs;
        }
    }
}
