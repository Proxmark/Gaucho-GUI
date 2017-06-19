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
using System.Xml;
using System.Threading;

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
        public TreeNode tvSettingsNodes = new TreeNode("Settings");
        public XmlDocument pm3Commands = new XmlDocument();

        public List<COMport> _comPorts = new List<COMport>();
        public SerialPort _comPort = new SerialPort();
        public Thread _comThread;
        delegate void _readCommPort();

        delegate void SetrtbMainTextCallback(byte[] comData);
        private static Form Help = null;
        private static WebBrowser browser = new WebBrowser();
        
        public class CmdItem
        {
            public string Id;
            public string ParentId;
            public string Type;
            public string Text;
            public string Tooltip;
            public string Action;
            public Control Control;
        }
        public frmMain()
        {
            InitializeComponent();

            DropDownTreeNode ddtnCOMAutoDetect = new DropDownTreeNode("Autodetect Proxmark");
            ddtnCOMAutoDetect.ComboBox.Items.AddRange(new string[]
                    {
                        "Autodetect Proxmark",
                        "Manually Select COM Port"
                    }
                );
            ddtnCOMAutoDetect.ComboBox.SelectedIndex = Convert.ToInt16(ConfigurationManager.AppSettings["Autodetect Proxmark"]);
            ddtnCOMAutoDetect.ComboBox.Text = ddtnCOMAutoDetect.ComboBox.Items[ddtnCOMAutoDetect.ComboBox.SelectedIndex].ToString();

            ddtnCOMAutoDetect.ComboBox.SelectedIndexChanged += new EventHandler((sender, e) => cb_SelectedValueChanged("Autodetect Proxmark", ddtnCOMAutoDetect.ComboBox));

            _comPorts = getCOMports();

            _comPort.BaudRate = 115200;
            _comPort.DataBits = 8;
            _comPort.Encoding = Encoding.Default;
            _comPort.Handshake = Handshake.None;
            _comPort.Parity = Parity.None;
            _comPort.StopBits = StopBits.One;
            _comPort.RtsEnable = true;
            _comPort.DtrEnable = true;
            _comPort.DataReceived += new SerialDataReceivedEventHandler(comRx);

            DropDownTreeNode ddtnCOMport = new DropDownTreeNode("Select COM Port");
            ddtnCOMport.ComboBox.SelectedIndexChanged += new EventHandler((sender, e) => cb_SelectedValueChanged("Selected COM Port", ddtnCOMport.ComboBox));
            ddtnCOMport.ComboBox.Width = 200;
            ddtnCOMport.ComboBox.MinimumSize = new Size(200, 12);

            if (_comPorts.Count > 0)
            {
                // Not thought out very well. If manual COM port selection is enabled and a serial device it attached or removed, this probably will not work.
                int pm3Index = 0;
                for (int i = 0; i < _comPorts.Count; i++)
                {
                    ddtnCOMport.ComboBox.Items.Add("(" + _comPorts[i].DeviceID + ") " +
                        _comPorts[i].Description);
                    if (_comPorts.Exists(x => x.Description.Contains("Proxmark3")))
                        pm3Index = i;
                    if (_comPorts.Exists(x => x.PNPDeviceID.Contains("PROXMARK.ORG")))
                        pm3Index = i;
                }
                if (ddtnCOMAutoDetect.ComboBox.SelectedIndex == 0)
                {
                    ddtnCOMport.ComboBox.SelectedIndex = pm3Index;
                    ddtnCOMport.ComboBox.Text = ddtnCOMport.ComboBox.Items[pm3Index].ToString();
                    ddtnCOMport.ComboBox.SelectedValue = ddtnCOMport.ComboBox.Items[pm3Index].ToString();
                    ddtnCOMport.ComboBox.SelectedItem = ddtnCOMport.ComboBox.Items[pm3Index];
                }
                else
                {
                    ddtnCOMport.ComboBox.SelectedIndex = Convert.ToInt16(ConfigurationManager.AppSettings["Selected COM Port"]);
                    ddtnCOMport.ComboBox.Text = ddtnCOMport.ComboBox.Items[ddtnCOMport.ComboBox.SelectedIndex].ToString();
                }
                try
                {
                    _comPort.PortName = _comPorts[ddtnCOMport.ComboBox.SelectedIndex].DeviceID;
                    _comPort.Open();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Failed to open port. Freaking out and exiting.\r\n" + ex.Message);
                    Application.Exit();
                }

            }
            else
            {
                Trace.WriteLine("No available ports");
                Application.Exit();
            }
            TreeNode tnCOMport = new TreeNode("COM Port");
            tnCOMport.Nodes.Add(ddtnCOMAutoDetect);
            tnCOMport.Nodes.Add(ddtnCOMport);
            tvSettingsNodes.Nodes.Add(tnCOMport);
            //tvSettingsNodes.Nodes.Add("Some other setting.");
            tvSettings.Nodes.Add(tvSettingsNodes);
            

            tvSettings.ExpandAll();


            string curDir = Directory.GetCurrentDirectory();
            pm3Commands.Load(String.Format(@"{0}\pm3Commands.xml", curDir));
            TreeNode pm3root = new TreeNode("PM3");
            tvMain.Nodes.Add(pm3root);

            ParseXmlNodes(pm3Commands.SelectSingleNode("pm3").ChildNodes, pm3root);
            tvMain.ExpandAll();
        }

        private void comRx(object sender, SerialDataReceivedEventArgs e)
        {
            // This was done deliberately.
            // Just incase there is a requirement for packet joining, start of message, etc... 
                if (_comPort.IsOpen)
                {
                    int bytes = 0;
                    try
                    {
                        bytes = _comPort.BytesToRead;
                    }
                    catch
                    {
                        // Probably closed the port or pulled the plug.
                        return;
                    }
                    byte[] buffer = new byte[bytes];
                    try
                    {
                        _comPort.Read(buffer, 0, bytes);
                    }
                    catch
                    {
                    // If we can't read move on. Probably closed the port or the USB adapter was removed...
                    return;
                    }
                    try
                    {
                        this.BeginInvoke(new SetrtbMainTextCallback(setrtbText), new object[] { buffer });
                    }
                    catch
                    {
                    //Probably closing the app.
                    return;
                    }
                }
        }

        private void setrtbText(byte[] buffer)
        {
            string comData = Encoding.UTF8.GetString(buffer);
            rtbMain.AppendText(comData);
        }
        public void ParseXmlNodes(XmlNodeList pm3Commands, TreeNode parentTvNode)
        {
            foreach (XmlNode node in pm3Commands)
            {
                TreeNode newNode = new TreeNode();
                if (node.Name == "commands")
                {
                    newNode.Text = node.Attributes["title"].Value;
                    newNode.ToolTipText = node.Attributes["tooltip"].Value;
                    newNode.Tag = node.Attributes["id"].Value;
                    parentTvNode.Nodes.Add(newNode);
                    if (node.HasChildNodes)
                        ParseXmlNodes(node.ChildNodes, newNode);
                }
            }
        }

        void cb_SelectedValueChanged(string AppSetting, ComboBox cb)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[AppSetting].Value = cb.SelectedIndex.ToString();
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
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

        private void tvMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            scMain_2.Panel1.Controls.Clear();            
            XmlNodeList nodes = pm3Commands.SelectNodes("//commands[@id='"+ e.Node.Tag+"']");
            int y = 0;
            foreach (XmlNode node in nodes)
            {
                XmlNodeList _controls = node.ChildNodes;
                foreach (XmlNode _control in _controls)
                {
                    if (_control.Name == "item")
                    {
                        switch (_control.Attributes["type"].Value)
                        {
                            case "button":
                                Button _button = new Button();
                                _button.Text = _control.Attributes["text"].Value;
                                _button.Tag = _control.Attributes["action"].Value;
                                _button.Top = y;
                                _button.Click += new EventHandler((a, b) => b_Click(_control.Attributes["action"].Value, _button));
                                scMain_2.Panel1.Controls.Add(_button);
                                break;
                            case "label":
                                Label _label = new Label();
                                _label.Text = _control.Attributes["text"].Value;
                                _label.Top = y;
                                scMain_2.Panel1.Controls.Add(_label);
                                break;
                            case "textbox":
                                TextBox _textbox = new TextBox();
                                _textbox.Text = _control.Attributes["text"].Value;
                                _textbox.Top = y;
                                scMain_2.Panel1.Controls.Add(_textbox);
                                break;
                            default:
                                break;
                        }
                        y += 22;
                        if (_control.HasChildNodes)
                            y = tvMainChildControls(_control.ChildNodes, y);
                    }
                }
            }
        }

        private int tvMainChildControls(XmlNodeList nodes, int y)
        {
            foreach (XmlNode _control in nodes)
            {
                if (_control.Name == "item")
                {
                    switch (_control.Attributes["type"].Value)
                    {
                        case "button":
                            Button _button = new Button();
                            _button.Text = _control.Attributes["text"].Value;
                            _button.Tag = _control.Attributes["action"].Value;
                            _button.Top = y;
                            _button.Click += new EventHandler((a, b) => b_Click(_control.Attributes["action"].Value, _button));
                            scMain_2.Panel1.Controls.Add(_button);
                            break;
                        case "label":
                            Label _label = new Label();
                            _label.Text = _control.Attributes["text"].Value;
                            _label.Top = y;
                            scMain_2.Panel1.Controls.Add(_label);
                            break;
                        case "textbox":
                            TextBox _textbox = new TextBox();
                            _textbox.Text = _control.Attributes["text"].Value;
                            _textbox.Top = y;
                            scMain_2.Panel1.Controls.Add(_textbox);
                            break;
                        default:
                            break;
                    }
                    y += 22;
                    if (_control.HasChildNodes)
                        y = tvMainChildControls(_control.ChildNodes, y);
                }
            }
            return y;
        }
        public void b_Click(string _action, Button _button)
        {
            //Send stuff

            if (_comPort.IsOpen)
                _comPort.WriteLine(_action);
        }

    }

    public class COMport
    {
        public string Description; //Proxmark3
        public string DeviceID;
        public string PNPDeviceID; //PROXMARK.ORG
    }

}
