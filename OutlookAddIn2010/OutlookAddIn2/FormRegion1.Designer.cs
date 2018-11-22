using System;
using System.Collections;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OutlookAddIn2
{
    [System.ComponentModel.ToolboxItemAttribute(false)]
    partial class FormRegion1 : Microsoft.Office.Tools.Outlook.FormRegionBase
    {
        private TcpClient _tcpclient;
        private System.IO.StreamReader _sReader;
        private System.IO.StreamWriter _sWriter;
        public FormRegion1(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            : base(Globals.Factory, formRegion)
        {
            this.InitializeComponent();
        }
        private const string PROSEEDA = "proseeda";
        private const string Key = "combo1";
        private const string cboxKey1 = "cbox1";
        private const string cboxKey2 = "cbox2";
        private const string cboxKey3 = "cbox3";

        private bool first = false;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Billable";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Please Select a Client Case:"});
            this.comboBox1.Location = new System.Drawing.Point(90, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(358, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // FormRegion1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox1);
            this.Name = "FormRegion1";
            this.Size = new System.Drawing.Size(686, 35);
            this.FormRegionShowing += new System.EventHandler(this.FormRegion1_FormRegionShowing);
            this.FormRegionClosed += new System.EventHandler(this.FormRegion1_FormRegionClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //handle state changes
        private void checkBox1_SelectedCheckedChanged(object sender, System.EventArgs e)
        {
            CheckBox comboBox = (CheckBox)sender;
            this.checkBox1.Checked = comboBox.Checked;
            Microsoft.Office.Interop.Outlook.MailItem appointment = (Microsoft.Office.Interop.Outlook.MailItem)this.OutlookItem;

            Microsoft.Office.Interop.Outlook.ItemProperty prop01 = appointment.ItemProperties.Add("checkBox", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olNumber);
            if (this.checkBox1.Checked)
            {
                prop01.Value = 1;
            }
            else
            {
                prop01.Value = 0;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void getCustomerDetails()
        {
            try
            {
                //server ip
                //String ipAddress = "127.0.0.1";
                String ipAddress = "18.224.148.94";
                //port number
                int portNum = 8099;
                //@todo error handling

                try
                {
                    _tcpclient = new TcpClient();
                    _tcpclient.Connect(ipAddress, portNum);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                try
                {

                    NetworkStream serverStream = _tcpclient.GetStream();
                    //capturing the meeting time

                    String clientData = "{\"msgRequestInsert\": \"query\"}";

                    byte[] outStream = Encoding.ASCII.GetBytes(clientData);
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                    // String to store the response ASCII representation.
                    String responseData = String.Empty;
                    Byte[] data = new Byte[1024];
                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = serverStream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    dynamic json = JsonConvert.DeserializeObject(responseData);
                    for (int i = 0; i < json.Count; i++)
                    {
                        dynamic record = json[i];
                        if (!ThisAddIn.customerData.ContainsKey(record.name))
                        {
                            ArrayList casesList = new ArrayList();
                            dynamic cases = record.cases;
                            for (int j = 0; j < cases.Count; j++)
                            {
                                dynamic caseRec = cases[j];
                                casesList.Add(caseRec.name);
                            }
                            ThisAddIn.customerData.Add(record.name, casesList);


                        }
                    }
                    Console.WriteLine("Received: {0}", responseData);

                    _sWriter.Close();
                    _tcpclient.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        

        private void comboBox1_SelectedIndexChanged(object sender,
        System.EventArgs e)
        {

            ComboBox comboBox = (ComboBox)sender;


            //let's 



            // Save the selected employee's name, because we will remove
            // the employee's name from the list.
            string selectedEmployee = (string)comboBox.SelectedItem;


            int resultIndex = -1;

            // Call the FindStringExact method to find the first 
            // occurrence in the list.
            resultIndex = comboBox.FindStringExact(selectedEmployee);

            this.comboBox1.SelectedIndex = resultIndex;
            Microsoft.Office.Interop.Outlook.MailItem appointment = (Microsoft.Office.Interop.Outlook.MailItem)this.OutlookItem;
            Microsoft.Office.Interop.Outlook.ItemProperty propEmp = appointment.ItemProperties.Add("SelectedItem", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
            propEmp.Value = (string)comboBox.SelectedItem;

            Microsoft.Office.Interop.Outlook.ItemProperty prop01 = appointment.ItemProperties.Add("selectedIndex", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olNumber);
            prop01.Value = this.comboBox1.SelectedIndex;
        }

        #endregion

        #region Form Region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private static void InitializeManifest(Microsoft.Office.Tools.Outlook.FormRegionManifest manifest, Microsoft.Office.Tools.Outlook.Factory factory)
        {
            manifest.FormRegionName = "Proseeda Billing Data";
            manifest.FormRegionType = Microsoft.Office.Tools.Outlook.FormRegionType.Adjoining;
            manifest.ShowInspectorRead = false;
            manifest.ShowReadingPane = false;

        }

        #endregion

        private CheckBox checkBox1;
        private ComboBox comboBox1;

        public partial class FormRegion1Factory : Microsoft.Office.Tools.Outlook.IFormRegionFactory
        {
            public event Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler FormRegionInitializing;

            private Microsoft.Office.Tools.Outlook.FormRegionManifest _Manifest;

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public FormRegion1Factory()
            {
                this._Manifest = Globals.Factory.CreateFormRegionManifest();
                FormRegion1.InitializeManifest(this._Manifest, Globals.Factory);
                this.FormRegionInitializing += new Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler(this.FormRegion1Factory_FormRegionInitializing);
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public Microsoft.Office.Tools.Outlook.FormRegionManifest Manifest
            {
                get
                {
                    return this._Manifest;
                }
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            Microsoft.Office.Tools.Outlook.IFormRegion Microsoft.Office.Tools.Outlook.IFormRegionFactory.CreateFormRegion(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            {
                FormRegion1 form = new FormRegion1(formRegion);
                form.Factory = this;
                if (ThisAddIn.customerData.Count == 0)
                    form.getCustomerDetails();
                foreach (DictionaryEntry pair in ThisAddIn.customerData)
                {
                    ArrayList listCases = (ArrayList)pair.Value;
                    for (int i = 0; i < listCases.Count; i++)
                    {
                        form.comboBox1.Items.Add(pair.Key + "," + listCases[i].ToString());
                    }


                }

                if (form.OutlookItem is Microsoft.Office.Interop.Outlook.MailItem)
                {
                    Microsoft.Office.Interop.Outlook.MailItem appointment = (Microsoft.Office.Interop.Outlook.MailItem)form.OutlookItem;


                    Microsoft.Office.Interop.Outlook.ItemProperty prop01 = appointment.ItemProperties["selectedIndex"];
                    if (prop01 != null)
                    {
                        try
                        {
                            form.comboBox1.SelectedIndex = (int)prop01.Value;
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                    }
                    else
                    {
                        prop01 = appointment.ItemProperties.Add("selectedIndex", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olNumber);
                        prop01.Value = 0;
                        form.comboBox1.SelectedIndex = 0;
                    }


                    Microsoft.Office.Interop.Outlook.ItemProperty prop02 = appointment.ItemProperties["checkBox"];
                    if (prop02 != null)
                    {
                        try
                        {
                            if ((int)prop02.Value == 1)
                            {
                                form.checkBox1.Checked = true;

                            }
                            else
                            {
                                form.checkBox1.Checked = false;
                            }
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                    }


                    else
                    {


                        prop02 = appointment.ItemProperties.Add("checkBox", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olNumber);
                        prop02.Value = 0;

                         

                    }
                    Microsoft.Office.Interop.Outlook.ItemProperty propTime = appointment.ItemProperties.Add("time", Microsoft.Office.Interop.Outlook.OlUserPropertyType.olText);
                    propTime.Value = DateTime.UtcNow.ToLocalTime();


                }
                form.comboBox1.SelectedIndexChanged += new System.EventHandler(form.comboBox1_SelectedIndexChanged);
                form.checkBox1.CheckedChanged += new System.EventHandler(form.checkBox1_SelectedCheckedChanged);

                
                
                return form;
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            byte[] Microsoft.Office.Tools.Outlook.IFormRegionFactory.GetFormRegionStorage(object outlookItem, Microsoft.Office.Interop.Outlook.OlFormRegionMode formRegionMode, Microsoft.Office.Interop.Outlook.OlFormRegionSize formRegionSize)
            {
                throw new System.NotSupportedException();
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            bool Microsoft.Office.Tools.Outlook.IFormRegionFactory.IsDisplayedForItem(object outlookItem, Microsoft.Office.Interop.Outlook.OlFormRegionMode formRegionMode, Microsoft.Office.Interop.Outlook.OlFormRegionSize formRegionSize)
            {
                if (this.FormRegionInitializing != null)
                {
                    Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs cancelArgs = Globals.Factory.CreateFormRegionInitializingEventArgs(outlookItem, formRegionMode, formRegionSize, false);
                    this.FormRegionInitializing(this, cancelArgs);
                    return !cancelArgs.Cancel;
                }
                else
                {
                    return true;
                }
            }

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            Microsoft.Office.Tools.Outlook.FormRegionKindConstants Microsoft.Office.Tools.Outlook.IFormRegionFactory.Kind
            {
                get
                {
                    return Microsoft.Office.Tools.Outlook.FormRegionKindConstants.WindowsForms;
                }
            }
        }
    }

    partial class WindowFormRegionCollection
    {
        internal FormRegion1 FormRegion1
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.GetType() == typeof(FormRegion1))
                        return (FormRegion1)item;
                }
                return null;
            }
        }

    }
}
