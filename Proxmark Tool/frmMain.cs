using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Management;
using System.Diagnostics;
using DropDownTreeView;
using System.IO;

#region
// Proxmark III Tool
//
// Original design stolen from Gaucho GUI
//
// Description: Provides a graphical user interface for the Proxmark III
//
// ============================================================================
// This software is free software; you can modify and/or redistribute it, provided
// that the authors are credited for the work.
//
// This library is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
// PARTICULAR PURPOSE.
// ============================================================================
#endregion

namespace Proxmark_Tool
{
    public partial class frmMain : Form
    {
        public List<COMport> COMPorts = new List<COMport>();
        public TreeNode tvSettingsNodes = new TreeNode("Settings");

        private static Form Help = null;
        private static WebBrowser browser = new WebBrowser();
        public frmMain()
        {
            InitializeComponent();

            COMPorts = getCOMports();
            DropDownTreeNode ddtnCOMport = new DropDownTreeNode("COM1");
            ddtnCOMport.ComboBox.Width = 200;
            ddtnCOMport.ComboBox.MinimumSize = new Size(200,12);
            TreeNode tnCOMport = new TreeNode("COM Port");
            tnCOMport.Nodes.Add(ddtnCOMport);
            tvSettingsNodes.Nodes.Add(tnCOMport);
            //tvSettingsNodes.Nodes.Add("Some other setting.");
            tvSettings.Nodes.Add(tvSettingsNodes);

            tvSettings.ExpandAll();

            if (COMPorts.Count > 0)
            {

                foreach (COMport port in COMPorts)
                {
                    ddtnCOMport.ComboBox.Items.Add("(" + port.DeviceID + ") " +
                        port.Description);

                }
                if (COMPorts.Exists(x => x.Description.Contains("Proxmark3")))
                    Trace.WriteLine("Description Match");
                if (COMPorts.Exists(x => x.PNPDeviceID.Contains("PROXMARK.ORG")))
                    Trace.WriteLine("PNPDeviceID Match");
                //COMPorts.Contains(ConfigurationManager.AppSettings["SerialPort"])

            }
            else
            {
                Trace.WriteLine("No available ports");
                Application.Exit();
            }
        }

        public List<COMport> getCOMports()
        {
            List<COMport> _result = new List<COMport>();
            try
            {
                using (var query = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_SerialPort"))
                    foreach (ManagementObject queryItem in query.Get())
                        _result.Add(new COMport() {
                            DeviceID    = queryItem.GetPropertyValue("DeviceID").ToString(),
                            Description = queryItem.GetPropertyValue("Description").ToString(),
                            PNPDeviceID = queryItem.GetPropertyValue("PNPDeviceID").ToString() });
            }
            catch (Exception _ex)
            {
                Trace.WriteLine(_ex.Message);
                Application.Exit();
            }

            return _result;
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            Trace.WriteLine(e.KeyCode.ToString());
            if (e.KeyCode == Keys.F1)
                ShowHelp();
        }

        private void ShowHelp()
        {
            if (Help != null)
            {
                Help.BringToFront();
            }
            else
            {
                Help = new Form();
                Help.Show();
            }

            string curDir = Directory.GetCurrentDirectory();

            browser.Dock                           = DockStyle.Fill;
            browser.AllowNavigation                = false;
            browser.AllowWebBrowserDrop            = false;
            browser.IsWebBrowserContextMenuEnabled = false;
            browser.Url                            = new Uri(String.Format("file:///{0}/Documentation/index.html", curDir));

            Help.Controls.Add(browser);
        }

    }



    public class COMport
    {
        public string Description; //Proxmark3
        public string DeviceID;
        public string PNPDeviceID; //PROXMARK.ORG
    }

}
