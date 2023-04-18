using SC_M4.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M4.Forms
{
    public partial class Key : Form
    {
        private KeyForms key;
        public Key(KeyForms key)
        {
            InitializeComponent();
            this.key = key;
        }

        private void Key_Load(object sender, EventArgs e)
        {
            if(key == KeyForms.CAM1)
            {
                lbTitle.Text = "KEY WORD CAM 1";
                txtKey.Text = Properties.Settings.Default.keyCAM1;
            }
            else if(key == KeyForms.CAM2)
            {
                lbTitle.Text = "KEY WORD CAM 2";
                txtKey.Text = Properties.Settings.Default.keyCAM2;
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            // Check key word
            if (txtKey.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter key word", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Save key word
            if (key == KeyForms.CAM1)
            {
                Properties.Settings.Default.keyCAM1 = txtKey.Text.Trim();
            }
            else if (key == KeyForms.CAM2)
            {
                Properties.Settings.Default.keyCAM2 = txtKey.Text.Trim();
            }

            Properties.Settings.Default.Save();

            // Close form
            this.Close();
        }
    }
}
