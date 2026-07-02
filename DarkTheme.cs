using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; using System.Drawing;
    using System.Windows.Forms;

namespace MandalaGenerator
{
   

    public static class DarkTheme
    {
        // Palette
        public static readonly Color Background = Color.FromArgb(30, 30, 30);      // #1E1E1E
        public static readonly Color Panel = Color.FromArgb(37, 37, 38);            // #252526
        public static readonly Color Input = Color.FromArgb(45, 45, 48);            // #2D2D30
        public static readonly Color Border = Color.FromArgb(63, 63, 70);           // #3F3F46
        public static readonly Color Text = Color.FromArgb(243, 243, 243);          // #F3F3F3
        public static readonly Color Secondary = Color.FromArgb(168, 168, 168);
        public static readonly Color Accent = Color.FromArgb(10, 132, 255);

        // Font
        public static readonly Font DefaultFont = new Font("Segoe UI", 10F);
        public static readonly Font TitleFont = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font ButtonFont = new Font("Segoe UI Semibold", 10F);

        public static void Apply(Form form)
        {
            form.BackColor = Background;
            form.ForeColor = Text;
            form.Font = DefaultFont;

            ApplyToControls(form.Controls);
        }

        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                switch (c)
                {
                    case Panel p:
                        p.BackColor = Panel;
                        p.ForeColor = Text;
                        break;

                    case GroupBox g:
                        g.BackColor = Background;
                        g.ForeColor = Text;
                        break;

                    case Label l:
                        l.ForeColor = Text;
                        l.BackColor = Color.Transparent;
                        break;

                    case TextBox t:
                        t.BackColor = Input;
                        t.ForeColor = Text;
                        t.BorderStyle = BorderStyle.FixedSingle;
                        t.Font = DefaultFont;
                        break;

                    case RichTextBox r:
                        r.BackColor = Input;
                        r.ForeColor = Text;
                        r.BorderStyle = BorderStyle.FixedSingle;
                        r.Font = new Font("Consolas", 10F);
                        break;

                    case ComboBox cb:
                        cb.BackColor = Input;
                        cb.ForeColor = Text;
                        cb.FlatStyle = FlatStyle.Flat;
                        break;

                    case Button b:
                        b.BackColor = Accent;
                        b.ForeColor = Color.White;
                        b.FlatStyle = FlatStyle.Flat;
                        b.FlatAppearance.BorderSize = 0;
                        b.Font = ButtonFont;
                        b.Height = 36;
                        break;

                    case CheckBox chk:
                        chk.ForeColor = Text;
                        chk.BackColor = Background;
                        break;

                    case ListBox lb:
                        lb.BackColor = Input;
                        lb.ForeColor = Text;
                        break;

                    case DataGridView dgv:
                        dgv.BackgroundColor = Panel;
                        dgv.GridColor = Border;

                        dgv.EnableHeadersVisualStyles = false;

                        dgv.ColumnHeadersDefaultCellStyle.BackColor = Panel;
                        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Text;

                        dgv.DefaultCellStyle.BackColor = Input;
                        dgv.DefaultCellStyle.ForeColor = Text;
                        dgv.DefaultCellStyle.SelectionBackColor = Accent;
                        dgv.DefaultCellStyle.SelectionForeColor = Color.White;

                        dgv.RowHeadersDefaultCellStyle.BackColor = Panel;
                        dgv.RowHeadersDefaultCellStyle.ForeColor = Text;

                        dgv.BorderStyle = BorderStyle.None;
                        break;
                }

                if (c.HasChildren)
                    ApplyToControls(c.Controls);
            }
        }
    }
}
