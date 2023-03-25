using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.UI.Xaml.Documents;

namespace SC_M4.Forms
{
    public partial class CameraControls : Form
    {

        private Main main;
        public CameraControls(Main main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void _ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender is TrackBar track)
                {

                    switch (track.Name)
                    {
                        case "tFocus":
                            if (track.Value != nFocus.Value)
                            {
                                nFocus.Value = track.Value;
                                // Set focus
                                main.cameraControl.setFocus(track.Value);
                            }
                            break;
                        case "tZoom":
                            if (track.Value != nZoom.Value)
                            {
                                nZoom.Value = track.Value;
                                // Set zoom
                                main.cameraControl.setZoom(track.Value);
                            }
                            break;
                        case "tPan":
                            if (track.Value != nPan.Value)
                            {
                                nPan.Value = track.Value;
                                // Set pan
                                main.cameraControl.setPan(track.Value);
                            }
                            break;
                        case "tTilt":
                            if (track.Value != nTilt.Value)
                            {
                                nTilt.Value = track.Value;
                                // Set tilt
                                main.cameraControl.setTilt(track.Value);
                            }
                            break;
                        case "tExposure":
                            if (track.Value != nExposure.Value)
                            {
                                nExposure.Value = track.Value;
                                // Set exposure
                                main.cameraControl.setExposure(track.Value);
                            }
                            break;
                    }
                }
                else if (sender is NumericUpDown numericUp)
                {
                    switch (numericUp.Name)
                    {
                        case "nFocus":
                            if (numericUp.Value != tFocus.Value)
                            {
                                tFocus.Value = (int)numericUp.Value;
                                // Set focus
                                main.cameraControl.setFocus((int)numericUp.Value);
                            }
                            break;
                        case "nZoom":
                            if (numericUp.Value != tZoom.Value)
                            {
                                tZoom.Value = (int)numericUp.Value;
                                // Set zoom
                                main.cameraControl.setZoom((int)numericUp.Value);
                            }
                            break;
                        case "nPan":
                            if (numericUp.Value != tPan.Value)
                            {
                                tPan.Value = (int)numericUp.Value;
                                // Set pan
                                main.cameraControl.setPan((int)numericUp.Value);
                            }
                            break;
                        case "nTilt":
                            if (numericUp.Value != tTilt.Value)
                            {
                                tTilt.Value = (int)numericUp.Value;
                                // Set tilt
                                main.cameraControl.setTilt((int)numericUp.Value);
                            }
                            break;
                        case "nExposure":
                            if (numericUp.Value != tExposure.Value)
                            {
                                tExposure.Value = (int)numericUp.Value;
                                // Set exposure
                                main.cameraControl.setExposure((int)numericUp.Value);
                            }
                            break;
                    }
                }
                else
                {
                    Debug.WriteLine("Unknown control type");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CameraControls_Load(object sender, EventArgs e)
        {
            try
            {
                if (main != null && main.cameraControl.cameraControl != null)
            {
                tFocus.Maximum = main.cameraControl.fmax;
                tFocus.Minimum = main.cameraControl.fmin;
                tFocus.Value = main.cameraControl.fValue;
                nFocus.Maximum = main.cameraControl.fmax;
                nFocus.Minimum = main.cameraControl.fmin;
                nFocus.Value = main.cameraControl.fValue;


                tZoom.Maximum = main.cameraControl.zmax;
                tZoom.Minimum = main.cameraControl.zmin;
                tZoom.Value = main.cameraControl.zValue < main.cameraControl.zmin ? main.cameraControl.zmin : main.cameraControl.zValue > main.cameraControl.zmax ? main.cameraControl.zmax : main.cameraControl.zValue;
                nZoom.Maximum = main.cameraControl.zmax;
                nZoom.Minimum = main.cameraControl.zmin;
                nZoom.Value = main.cameraControl.zValue < main.cameraControl.zmin ? main.cameraControl.zmin : main.cameraControl.zValue > main.cameraControl.zmax ? main.cameraControl.zmax : main.cameraControl.zValue;

                tPan.Maximum = main.cameraControl.pmax;
                tPan.Minimum = main.cameraControl.pmin;
                tPan.Value = main.cameraControl.pValue;
                nPan.Maximum = main.cameraControl.pmax;
                nPan.Minimum = main.cameraControl.pmin;
                nPan.Value = main.cameraControl.pValue;

                tTilt.Maximum = main.cameraControl.tmax;
                tTilt.Minimum = main.cameraControl.tmin;
                tTilt.Value = main.cameraControl.tValue;
                nTilt.Maximum = main.cameraControl.tmax;
                nTilt.Minimum = main.cameraControl.tmin;
                nTilt.Value = main.cameraControl.tValue;

                tExposure.Minimum = main.cameraControl.emin;
                tExposure.Maximum = main.cameraControl.emax;
                tExposure.Value = main.cameraControl.eValue < main.cameraControl.emin ? main.cameraControl.emin : main.cameraControl.eValue > main.cameraControl.emax ? main.cameraControl.emax : main.cameraControl.eValue;
                nExposure.Maximum = main.cameraControl.emax;
                nExposure.Minimum = main.cameraControl.emin;
                nExposure.Value = main.cameraControl.eValue < main.cameraControl.emin ? main.cameraControl.emin : main.cameraControl.eValue > main.cameraControl.emax ? main.cameraControl.emax : main.cameraControl.eValue;

            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.dFocus = tFocus.Value;
                Properties.Settings.Default.dZoom = tZoom.Value;
                Properties.Settings.Default.dPan = tPan.Value;
                Properties.Settings.Default.dTilt = tTilt.Value;
                Properties.Settings.Default.dExposure = tExposure.Value;

                Properties.Settings.Default.Save();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error 018:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
