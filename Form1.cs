using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MandalaGenerator
{
    public partial class FormMandalaControls: Form
    {        ComboBox GenerationCombobox = new ComboBox { Size = new Size(80, 100) };
            ComboBox IterationStartBox = new ComboBox { Size = new Size(80, 100), Items = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 } };
            ComboBox NumberOfChildBox = new ComboBox { Size = new Size(80, 100), Items = { 1, 2, 3, 4, 5, 6, 7, 8 } };
            ComboBox MovementOptionBox = new ComboBox { Size = new Size(80, 100) };
            ComboBox SelectAnimationEffect = new ComboBox { Size = new Size(80, 100) };
            ComboBox SelectNumberGenerationLengthBox = new ComboBox { Size = new Size(80, 100) };
            ComboBox DrawSchemaComboBox = new ComboBox { Size = new Size(80, 100) };
             List<ComboBox> comboBoxes = new List<ComboBox>();
        Button ExecuteCLIBtn = new Button();
        Button LoadMandalaSkriptBtn = new Button();    
        Button SaveMandalaSkriptBtn = new Button();    
        TextBox CLIinterface;
        MandalaForm mandalaForm { get ; set; }   

        public FormMandalaControls()
        {
            InitializeComponent();
         
            GenerationCombobox.SelectedValueChanged += GenerationCombobox_SelectedValueChanged;
            IterationStartBox.SelectedValueChanged += IterationStartBox_SelectedValueChanged;
            NumberOfChildBox.SelectedValueChanged += NumberOfChildBox_SelectedValueChanged;
            MovementOptionBox.SelectedIndexChanged += OptionMovCombobox_SelectedValueChanged;
            SelectAnimationEffect.SelectedIndexChanged += SelectAnimationEffect_SelectedIndexChanged;
            SelectNumberGenerationLengthBox.SelectedIndexChanged += SelectNumberGenerationLengthBox_SelectedIndexChanged;
            DrawSchemaComboBox.SelectedIndexChanged += DrawSchemaComboBox_SelectedIndexChanged;
            InitComboBoxes();
       
            this.HandleCreated += FormMandalaControls_HandleCreated;
            
      
        }


        private void FormMandalaControls_HandleCreated(object? sender, EventArgs e)
        {      DarkTheme.Apply(this);
            CLIinterface  = new TextBox { Multiline = true, Size = new Size(400, 400), Location = new Point(400, 40), BackColor = Color.Black, ForeColor = Color.YellowGreen, Font = new Font("Classic Console", 18) };
            this.Controls.Add(CLIinterface);
            ExecuteCLIBtn.Click += ExecuteCLIBtn_Click;
            ExecuteCLIBtn.Location = new Point(CLIinterface.Left, CLIinterface.Bottom + 20);
            ExecuteCLIBtn.Size = new Size(80, 40);
            ExecuteCLIBtn.Text = "Execute ⇒";
            this.Controls.Add(ExecuteCLIBtn);
        }

        private void ExecuteCLIBtn_Click(object? sender, EventArgs e)
        {
            mandalaForm.ReciveMsgFromCLI(CLIinterface.Text);
        }

        public void SetMandalaForm(MandalaForm mandalaForm) 
        {
          this.mandalaForm = mandalaForm;
        }
        private void DrawSchemaComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            mandalaForm.MandalaOptions.schemaAnimation = (TypeOfSchemaAnimation)DrawSchemaComboBox.SelectedItem;
        }

        private void SelectNumberGenerationLengthBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            mandalaForm.MandalaOptions.Generations = (int)SelectNumberGenerationLengthBox.SelectedItem;
        }

        private void SelectAnimationEffect_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int S = (int)SelectAnimationEffect.SelectedItem;
            this.mandalaForm.MandalaOptions.TypeOfMusicAnimation = (TypeOfMusicAnimation)(int)SelectAnimationEffect.SelectedItem;
        }

        private void NumberOfChildBox_SelectedValueChanged(object? sender, EventArgs e)
        {
            mandalaForm.MandalaOptions.ChildsCounter = (int)NumberOfChildBox.SelectedItem;

        }

        private void IterationStartBox_SelectedValueChanged(object? sender, EventArgs e)
        {
            mandalaForm.MandalaOptions.PatternCounter = (int)IterationStartBox.SelectedItem;
        }

        private void GenerationCombobox_SelectedValueChanged(object? sender, EventArgs e)
        {
            this.mandalaForm.MandalaOptions.generetionalProgress = (TypeOfMandalaGeneretionalProgress)GenerationCombobox.SelectedItem;

        }
        private void OptionMovCombobox_SelectedValueChanged(object? sender, EventArgs e)
        {
            this.mandalaForm.MandalaOptions.movementPattern = (TypeOfMandalaMovement)MovementOptionBox.SelectedItem;

        }

        private void EventCreateForBoxes(object? sender, EventArgs e)
        {
            if (mandalaForm.IsOverTheLimit()) { MessageBox.Show("!!!!DU WILLST ZU VIEL!!!!"); return; }
            mandalaForm.CreateMandala();

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
            foreach (var value in Enum.GetValues<TypeOfSchemaAnimation>())
            {
                DrawSchemaComboBox.Items.Add(value);
            }
            for (int i = 0; i < 20; i++) { SelectNumberGenerationLengthBox.Items.Add(i); }

            comboBoxes.Add(GenerationCombobox);
            comboBoxes.Add(NumberOfChildBox);
            comboBoxes.Add(IterationStartBox);
            comboBoxes.Add(MovementOptionBox);
            comboBoxes.Add(SelectAnimationEffect);
            comboBoxes.Add(SelectNumberGenerationLengthBox);
            comboBoxes.Add(DrawSchemaComboBox);
            string[] names = { "Grow Func", "Num of childs", "Num 1st gen", "Movement", "Animation", "Num of gen", "Draw schema" };
            for (int i = 0;i < comboBoxes.Count;i++) 
            {
                Label beschriftung = new Label { Text = names[i] , Location = new Point(10 , (i+1)*40) };
                comboBoxes[i].Location = new Point(160, (i + 1) * 40); 
                comboBoxes[i].SelectedValueChanged += EventCreateForBoxes;
                this.Controls.Add(comboBoxes[i]);
                this.Controls.Add( beschriftung);    
            }
            
          
        }

    }
}
