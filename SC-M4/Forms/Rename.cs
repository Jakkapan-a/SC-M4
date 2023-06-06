
using SC_M4.Modules;
using SC_M4.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SC_M4.Forms
{
    public partial class Rename : Form
    {
        private Edit_Items _editItems;
        private SettingModel _settingModel;
        private int _type = -1;
        private PageType _page = PageType.None;
        private int _idSoftware;
        private int _idModel;

        public delegate void OnReloadData();
        public event OnReloadData EventReloadData;

        public Rename(Edit_Items editItems, int operationType)
        {
            InitializeComponent();
            _editItems = editItems;
            _type = operationType;
        }

        public Rename(Edit_Items editItems, PageType pageType)
        {
            InitializeComponent();
            _editItems = editItems;
            _page = pageType;
        }

        public Rename(SettingModel settingModel, int operationType)
        {
            InitializeComponent();
            _settingModel = settingModel;
            _type = operationType;
        }


        public Rename(SettingModel settingModel, PageType pageType)
        {
            InitializeComponent();
            _settingModel = settingModel;
            _page = pageType;
        }

        int id_sw;
        int id_lb;

        private List<(string, string)> shades = new List<(string, string)>
        {
            /**
            White
            Blue
            Red
            Orange
            Yellow
            Green
            Violet
            Brown
            Black
            Grey
            */
            ("FFFFFF", "White"),
            ("0000FF", "Blue"),
            ("FF0000", "Red"),
            ("FFA500", "Orange"),
            ("FFFF00", "Yellow"),
            ("008000", "Green"),
            ("EE82EE", "Violet"),
            ("A52A2A", "Brown"),
            ("000000", "Black"),
            ("808080", "Grey"),
        };

        private void Edit_Item_Load(object sender, EventArgs e)
        {
            try
            {
                if (_editItems != null)
                {
                    _idModel = _editItems.id_lb;
                    _idSoftware = _editItems.id_sw;
                }
                else if (_settingModel != null)
                {
                    _idModel = _settingModel.id_lb;
                    _idSoftware = _settingModel.id_sw;
                }
                else
                {
                    Close();
                }

                if ((_type == -1 && _page == PageType.None) || (_type == 0 || _page == PageType.SW))
                {
                    RenameSoftware();
                }
                else if (_type == 1 || _page == PageType.LB)
                {
                    RenameModel();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        private void RenameSoftware()
        {
            lbTitle.Text = "Rename Software";
            var data = MasterSW.GetMasterSW(_idSoftware);
            txtInput.Text = data[0].name;
        }

        private void RenameModel()
        {
            lbTitle.Text = "Rename Model";
            var data = MasterLB.GetMasterLB(_idModel);
            txtInput.Text = data[0].name;
            cbColor.Visible = true;

            lbColor.Visible = true;

            // For loop cbColor
            if (data[0].color_name != null)
            {
                for (int i = 0; i < cbColor.Items.Count; i++)
                {
                    if (cbColor.Items[i].ToString().ToLower() == data[0].color_name.ToLower())
                    {
                        cbColor.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void setLbColor()
        {
            if (cbColor.SelectedIndex == -1)
            {
                lbColor.BackColor = Color.Black;
                return;
            }
            lbColor.BackColor = ColorTranslator.FromHtml("#" + shades[cbColor.SelectedIndex].Item1);
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if ((_type == -1 && _page == PageType.None) || (_type == 0 || _page == PageType.SW))
                {
                    var data = MasterSW.GetMasterSW(_idSoftware);
                    data[0].name = txtInput.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                    if (MasterSW.IsExist(data[0].name))
                    {
                        MessageBox.Show("SW name is exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    data[0].Update();

                }
                else if (_type == 1 || _page == PageType.LB)
                {
                    var data = MasterLB.GetMasterLB(_idModel);
                    data[0].name = txtInput.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
                    if (cbColor.SelectedItem != null)
                    {
                        data[0].color_name = cbColor.SelectedItem.ToString();
                    }
                    else
                    {
                       // data[0].color_name = null;
                        MessageBox.Show("Please select color!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (MasterLB.IsExist(data[0].id, data[0].name))
                    {
                        MessageBox.Show("Model name is exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    data[0].Update();
                }
                EventReloadData?.Invoke();

                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void cbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            setLbColor();
        }
    }
}
