using SC_M4.Modules;
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
    public partial class NameListForms : Form
    {
        private NameList _NameList;

        private int _id = 0;
        public NameListForms(NameList name,int id = -1)
        {
            InitializeComponent();
            _NameList = name;
            _id = id;
        }
        private void NameListForms_Load(object sender, EventArgs e)
        {
            if(_id != -1)
            {
                lbTitle.Text = "Edit Name";

                var item = Modules.ReplaceName.Get(_id);
                txtOldName.Text = item.oldName;
                txtNewName.Text = item.newName;

                btnSave.Text = "Update";
                
            }
            else
            {
                lbTitle.Text = "Add Name";
            }

            this.ActiveControl = txtOldName;
            txtOldName.Focus();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_id != -1)
                {

                    var name = new ReplaceName();
                    name.id = _id;
                    name.oldName = txtOldName.Text;
                    name.newName = txtNewName.Text;
                    name._type = _NameList._type;
                    name.Validate();
                    if (ReplaceName.isExist(name.oldName, name._type,name.id) > 0)
                    {
                        throw new Exception("Old name is exist");
                    }
                    name.Update();
                    _NameList.randersTable();
                    this.Close();

                }
                else
                {
                    ReplaceName replace = new ReplaceName();
                    replace.newName = txtNewName.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\\", "").Replace("|", "").Replace(@"\", "");
                    replace.oldName = txtOldName.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\\", "").Replace("|", "").Replace(@"\", "");
                    replace._type = _NameList._type;
                    replace.Validate();
                    if (ReplaceName.isExist(replace.oldName, replace._type) > 0)
                    {
                        throw new Exception("Old name is exist");
                    }
                    replace.Save();
                    _NameList.randersTable();
                    this.Close(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

       
    }
}
