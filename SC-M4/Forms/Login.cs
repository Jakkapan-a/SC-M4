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
    public partial class Login : Form
    {
        private NameList nameList;
        public event EventHandler<EventArgs> OnLogin;
        public bool _login = false;
        public Login()
        {
            InitializeComponent();
        }

        public Login(NameList name)
        {
            InitializeComponent();
            nameList = name;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtPassword;
            txtPassword.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == "0987" && nameList != null)
            {
                nameList._login = true;
            }else if(txtPassword.Text == "0987")
            {
                _login = true;
                OnLogin?.Invoke(_login, EventArgs.Empty);
            }
            this.Close();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
            }
        }
    }
}
