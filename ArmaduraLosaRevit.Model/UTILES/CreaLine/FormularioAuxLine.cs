
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.UTILES.CreaLine
{
    public partial class FormularioAuxLine : Form
    {
        public bool Isok { get; set; }
        public ptosLinea p1 { get; set; }
        public ptosLinea p2 { get; set; }
        public FormularioAuxLine()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string[] wordsP1=  P1_.Text.Split(',');
            string[] wordsP2 = P2_.Text.Split(',');

            if (wordsP1.Length != 3)
            {
                MessageBox.Show("Error en el pto1", "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return;
            }
            if (wordsP2.Length != 3)
            {
                MessageBox.Show("Error en el pto2", "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return;
            }

            p1 = new ptosLinea(Util.ConvertirStringInDouble(wordsP1[0]), Util.ConvertirStringInDouble(wordsP1[1]), Util.ConvertirStringInDouble(wordsP1[2]));
            p2 = new ptosLinea(Util.ConvertirStringInDouble(wordsP2[0]), Util.ConvertirStringInDouble(wordsP2[1]), Util.ConvertirStringInDouble(wordsP2[2]));
            Isok = true;
            Close();
        }

        private void Button_p1_normal_Click(object sender, EventArgs e)
        {
            string[] wordsP1 = p1_normal.Text.Split(',');
            string[] words_normal = textBox_normal.Text.Split(',');
            
            if (wordsP1.Length != 3)
            {
                MessageBox.Show("Error en el pto1", "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return;
            }
            if (words_normal.Length != 3)
            {
                MessageBox.Show("Error en la normal", "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return;
            }


            if (_largo.Text.Trim() =="")
            {
                MessageBox.Show("Error, largo ai valor", "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return;
            }
            if (!Util.IsNumeric(_largo.Text))
            {
                MessageBox.Show("Error, largo no es numerico", "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            double largo = Util.ConvertirStringInDouble(_largo.Text);


            p1 = new ptosLinea(Util.ConvertirStringInDouble(wordsP1[0]), Util.ConvertirStringInDouble(wordsP1[1]), Util.ConvertirStringInDouble(wordsP1[2]));
            p2 = new ptosLinea(Util.ConvertirStringInDouble(wordsP1[0])+ Util.ConvertirStringInDouble(words_normal[0])* largo,
                               Util.ConvertirStringInDouble(wordsP1[1]) + Util.ConvertirStringInDouble(words_normal[1]) * largo,
                               Util.ConvertirStringInDouble(wordsP1[2]) + Util.ConvertirStringInDouble(words_normal[2]) * largo);
            Isok = true;
            Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Isok = false;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Isok = false;
            Close();
        }
    }
}
