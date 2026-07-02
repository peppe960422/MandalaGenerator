using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MandalaGenerator
{
    public partial class MandalaForm: Form
    {
       
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
    
        Action<Graphics, MandalaPoint, float, Func<double, double, double, Color>> DrawAction
                ;
      
       public  MandalaOptions MandalaOptions = new MandalaOptions();
       public  MandalaOptions[] OptionsFromCode = null;  
        int indexOfTheOptions = -1; 

     

        const int NODE_LIMIT = 10000;
        List<MandalaPoint> roots =
            new List<MandalaPoint>();

        float time = 0;
        AudioAnalyzer audio = new AudioAnalyzer();
        public MandalaForm()
        {
            InitializeComponent();
           // Test();
            DoubleBuffered = true;
            DrawAction = MandalaFuncs.DrawingStrategies[(int)MandalaOptions.TypeOfMusicAnimation];

            audio.Start();

            audio.OnKick += () =>
            {
                MandalaPoint.GlobalBass = audio.Bass;
                MandalaPoint.KickFlash = 1f;
            };

            KeyDown += FormMandala_KeyDown
                ;

            timer.Interval = 16;

            timer.Tick += MandalaTick;


         

            timer.Start();
            Paint += FormMandala_Paint;
            FormMandalaControls ct = new FormMandalaControls();
            ct.SetMandalaForm(this);    
            ct.Show();
        }

        private void MandalaTick (object sender, EventArgs e)
        {
            time += 0.03f;
            MandalaPoint.GlobalBass = audio.Bass;
            MandalaPoint.KickFlash *= 0.88f;

            if (time >= 3.00 && !IsOverTheLimit())
            {
                if (OptionsFromCode == null) { CreateMandala(); }
                else { CreateMandala(OptionsFromCode[indexOfTheOptions]); indexOfTheOptions = (indexOfTheOptions + 1) % OptionsFromCode.Length; }
                time = 0;
            }

            foreach (var r in roots)
                r.Update(time);

            Invalidate();
        }

       
     
        int CountNodes(int generation, int childs, ref int total)
        {
            total++;
            if (total > NODE_LIMIT) return total;
            if (generation <= 0) return total;

            int tchilds = MandalaFuncs.ChildrenFunc[(int)MandalaOptions.generetionalProgress](childs, generation);

            for (int i = 0; i < tchilds; i++)
            {
                CountNodes(generation - 1, tchilds, ref total);
                if (total > NODE_LIMIT) return total;
            }
            return total;
        }

        public bool IsOverTheLimit()
        {
            int total = 0;
            int rootCount = Math.Max(1, MandalaOptions.PatternCounter);

            for (int i = 0; i < rootCount; i++)
            {
                CountNodes(MandalaOptions.Generations, MandalaOptions.ChildsCounter, ref total);
                if (total > NODE_LIMIT) return true;
            }
            return total > NODE_LIMIT;
        }
        private void FormMandala_KeyDown(object? sender, KeyEventArgs e)
        {
            Keys k = e.KeyData;
            int MAX_PATTERN = 12;
            int MAX_CHILDS = 8;

            switch (k)
            {
                case Keys.Up:
                    MandalaOptions.PatternCounter = (MandalaOptions.PatternCounter % MAX_PATTERN) + 1;
                    break;

                case Keys.Down:
                    MandalaOptions.PatternCounter = ((MandalaOptions.PatternCounter - 2 + MAX_PATTERN) % MAX_PATTERN) + 1;
                    break;
                case Keys.Left:
                    MandalaOptions.ChildsCounter = ((MandalaOptions.ChildsCounter - 2 + MAX_CHILDS) % MAX_CHILDS) + 1;
                    break;
                case Keys.Right:
                    MandalaOptions.ChildsCounter = (MandalaOptions.ChildsCounter % MAX_CHILDS) + 1;
                    break;
            }

            if (IsOverTheLimit()) return;
            CreateMandala();
        }

        public void ReciveMsgFromCLI(string skript) {
           //"|anima:1,mov:1,evo:0,schema:0|for(5)|childs:3,gens:2,start:10,anima:1|";
            MandalaStringParser parser = new MandalaStringParser();
            OptionsFromCode =parser.GenerateMandalaFromCode(skript);
            indexOfTheOptions = 0;
           
                
           
        }

    
        public void CreateMandala(MandalaOptions options = null)
        {
            roots.Clear();
            if (options != null) { MandalaOptions = options; }

            Point[] points = MandalaFuncs.SchemaStrategies[(int)MandalaOptions.schemaAnimation ](
              
                this.Height,
                this.Width);



            int count = MandalaOptions.PatternCounter;
            for (int i = 0; i < points.Length; i++)
            {
                int MandalaDimensionLength;
                if(i == points.Length-1 ) {
                    MandalaDimensionLength = 100;
                }
                else
                {
                    MandalaDimensionLength = 50;
                }
                PointF center = points[i];
                for (int j = 0; j < count; j++)
                {
                    double angle =
                        (Math.PI * 2 / count) * j;

                    roots.Add(
                        new MandalaPoint(
                            center,
                            angle,
                            MandalaDimensionLength,
                            MandalaOptions.Generations,
                            MandalaOptions.ChildsCounter,
                                MandalaOptions.movementPattern,
                            MandalaOptions.generetionalProgress,
                            MandalaFuncs.DrawingStrategies[(int)this.MandalaOptions.TypeOfMusicAnimation]
                            )
                        )
                        ;
                }
            }



        }

        private void FormMandala_Paint(
            object? sender,
            PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode =
                SmoothingMode.AntiAlias;

            g.Clear(Color.Black);

            foreach (var r in roots)
            {
                r.Draw(g, time);
            }
        }
    }
}
