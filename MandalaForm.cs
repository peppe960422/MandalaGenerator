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
        ComboBox GenerationCombobox = new ComboBox { Location = new Point(10, 10), Size = new Size(80, 100) };
        ComboBox IterationStartBox = new ComboBox { Size = new Size(80, 100), Items = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 } };
        ComboBox NumberOfChildBox = new ComboBox { Size = new Size(80, 100), Items = { 1, 2, 3, 4, 5, 6, 7, 8 } };
        ComboBox MovementOptionBox = new ComboBox { Size = new Size(80, 100) };
        ComboBox SelectAnimationEffect = new ComboBox { Size = new Size(80, 100) };
        ComboBox SelectNumberGenerationLengthBox = new ComboBox { Size = new Size(80, 100) };
        Action<Graphics, MandalaPoint, float, Func<double, double, double, Color>> DrawAction
                ;
        TypeOfMusicAnimation TypeOfMusicAnimation = TypeOfMusicAnimation.Explode;
        TypeOfMandalaGeneretionalProgress generetionalProgress = TypeOfMandalaGeneretionalProgress.Const;
        TypeOfMandalaMovement movementPattern = TypeOfMandalaMovement.Const;

        int PatternCounter = 8;
        int ChildsCounter = 3;
        int Generations = 5;

        const int NODE_LIMIT = 10000;
        List<MandalaPoint> roots =
            new List<MandalaPoint>();

        float time = 0;
        AudioAnalyzer audio = new AudioAnalyzer();
        public MandalaForm()
        {
            InitializeComponent();

            DoubleBuffered = true;
            DrawAction = MandalaFuncs.DrawingStrategies[(int)TypeOfMusicAnimation];

            audio.Start();

            audio.OnKick += () =>
            {
                MandalaPoint.GlobalBass = audio.Bass;
                MandalaPoint.KickFlash = 1f;
            };

            KeyDown += FormMandala_KeyDown
                ;

            timer.Interval = 16;

            timer.Tick += (s, e) =>
            {
                time += 0.03f;
                MandalaPoint.GlobalBass = audio.Bass;
                MandalaPoint.KickFlash *= 0.88f;

                if (Math.Ceiling(time) % 6 == 0 && !IsOverTheLimit()) {; CreateMandala(); }

                foreach (var r in roots)
                    r.Update(time);

                Invalidate();
            };
            GenerationCombobox.SelectedValueChanged += GenerationCombobox_SelectedValueChanged;
            IterationStartBox.SelectedValueChanged += IterationStartBox_SelectedValueChanged;
            NumberOfChildBox.SelectedValueChanged += NumberOfChildBox_SelectedValueChanged;
            MovementOptionBox.SelectedIndexChanged += OptionMovCombobox_SelectedValueChanged;
            SelectAnimationEffect.SelectedIndexChanged += SelectAnimationEffect_SelectedIndexChanged;
            SelectNumberGenerationLengthBox.SelectedIndexChanged += SelectNumberGenerationLengthBox_SelectedIndexChanged;
            ;

            InitComboBoxes();

            timer.Start();
            Paint += FormMandala_Paint;
        }

        private void SelectNumberGenerationLengthBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            Generations = (int)SelectNumberGenerationLengthBox.SelectedItem;
        }

        private void SelectAnimationEffect_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int S = (int)SelectAnimationEffect.SelectedItem;
            this.TypeOfMusicAnimation = (TypeOfMusicAnimation)(int)SelectAnimationEffect.SelectedItem;
        }

        private void NumberOfChildBox_SelectedValueChanged(object? sender, EventArgs e)
        {
            ChildsCounter = (int)NumberOfChildBox.SelectedItem;

        }

        private void IterationStartBox_SelectedValueChanged(object? sender, EventArgs e)
        {
            PatternCounter = (int)IterationStartBox.SelectedItem;
        }

        private void GenerationCombobox_SelectedValueChanged(object? sender, EventArgs e)
        {
            this.generetionalProgress = (TypeOfMandalaGeneretionalProgress)GenerationCombobox.SelectedItem;

        }
        private void OptionMovCombobox_SelectedValueChanged(object? sender, EventArgs e)
        {
            this.movementPattern = (TypeOfMandalaMovement)MovementOptionBox.SelectedItem;

        }

        private void EventCreateForBoxes(object? sender, EventArgs e)
        {
            if (IsOverTheLimit()) { MessageBox.Show("!!!!DU WILLST ZU VIEL!!!!"); return; }
            CreateMandala();

        }
        private void InitComboBoxes()
        {
            foreach (var value in Enum.GetValues<TypeOfMandalaGeneretionalProgress>())
            {
                GenerationCombobox.Items.Add(value);
            }
            foreach (var value in Enum.GetValues<TypeOfMandalaMovement>())
            {
                MovementOptionBox.Items.Add(value);
            }
            foreach (var value in Enum.GetValues<TypeOfMusicAnimation>())
            {
                SelectAnimationEffect.Items.Add(value);
            }
            for (int i = 0; i < 20; i++) { SelectNumberGenerationLengthBox.Items.Add(i); }

            GenerationCombobox.SelectedValueChanged += EventCreateForBoxes;
            NumberOfChildBox.SelectedValueChanged += EventCreateForBoxes;
            IterationStartBox.SelectedValueChanged += EventCreateForBoxes;
            MovementOptionBox.SelectedIndexChanged += EventCreateForBoxes;
            SelectAnimationEffect.SelectedIndexChanged += EventCreateForBoxes;
            NumberOfChildBox.Location = new Point(120, 10);
            IterationStartBox.Location = new Point(240, 10);
            MovementOptionBox.Location = new Point(360, 10);
            SelectAnimationEffect.Location = new Point(480, 10);
            SelectNumberGenerationLengthBox.Location = new Point(600, 10);
            this.Controls.Add(NumberOfChildBox);
            this.Controls.Add(IterationStartBox);
            this.Controls.Add(GenerationCombobox);
            this.Controls.Add(MovementOptionBox);
            this.Controls.Add(SelectAnimationEffect);
            this.Controls.Add(SelectNumberGenerationLengthBox);
        }


        int CountNodes(int generation, int childs, ref int total)
        {
            total++;
            if (total > NODE_LIMIT) return total;
            if (generation <= 0) return total;

            int tchilds = MandalaFuncs.ChildrenFunc[(int)generetionalProgress](childs, generation);

            for (int i = 0; i < tchilds; i++)
            {
                CountNodes(generation - 1, tchilds, ref total);
                if (total > NODE_LIMIT) return total;
            }
            return total;
        }

        bool IsOverTheLimit()
        {
            int total = 0;
            int rootCount = Math.Max(1, PatternCounter);

            for (int i = 0; i < rootCount; i++)
            {
                CountNodes(Generations, ChildsCounter, ref total);
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
                    PatternCounter = (PatternCounter % MAX_PATTERN) + 1;
                    break;

                case Keys.Down:
                    PatternCounter = ((PatternCounter - 2 + MAX_PATTERN) % MAX_PATTERN) + 1;
                    break;
                case Keys.Left:
                    ChildsCounter = ((ChildsCounter - 2 + MAX_CHILDS) % MAX_CHILDS) + 1;
                    break;
                case Keys.Right:
                    ChildsCounter = (ChildsCounter % MAX_CHILDS) + 1;
                    break;
            }

            if (IsOverTheLimit()) return;
            CreateMandala();
        }

        void CreateMandala()
        {
            roots.Clear();

            PointF center =
                new PointF(
                    ClientSize.Width / 2
                    ,
                    ClientSize.Height / 2);



            int count = PatternCounter;

            for (int i = 0; i < count; i++)
            {
                double angle =
                    (Math.PI * 2 / count) * i;

                roots.Add(
                    new MandalaPoint(
                        center,
                        angle,
                        100,
                        Generations,
                        ChildsCounter,
                        movementPattern,
                        generetionalProgress,
                        MandalaFuncs.DrawingStrategies[(int)this.TypeOfMusicAnimation]
                        )
                    )
                    ;
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
