using System;
using System.Collections;
using System.Windows.Forms;

namespace OutlookAddIn1
{
    [System.ComponentModel.ToolboxItemAttribute(false)]
    partial class FormRegion2 : Microsoft.Office.Tools.Outlook.FormRegionBase
    {
        public FormRegion2(Microsoft.Office.Interop.Outlook.FormRegion formRegion)
            : base(Globals.Factory, formRegion)
        {
            this.InitializeComponent();
        }

        private const string Key = "combo1";
        private const string cboxKey1 = "cbox1";
        private const string cboxKey2 = "cbox2";
        private const string cboxKey3 = "cbox3";


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
            //initalize the default value into the new form we added
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // checkBox3
            // 
            this.checkBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(207, 3);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(59, 17);
            this.checkBox3.TabIndex = 8;
            this.checkBox3.Text = "Private";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(97, 3);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(82, 17);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "Non Billable";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(3, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Billable";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            //@todo get this from database
            this.comboBox1.Items.AddRange(new object[] {
            "Please Select a Client Case:",
            "PWC Law Division (4331) Foxcon Due Diligence (4331/021)",
            "PWC Law Division (4331) Smith vs IRS (4331/82)",
            "PWC Real Estate Division (4337) Wembley Building (4337/991)"});
            this.comboBox1.Location = new System.Drawing.Point(301, 1);
            this.comboBox1.MaxDropDownItems = 60;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(348, 21);
            this.comboBox1.Sorted = true;
            this.comboBox1.TabIndex = 9;
            //if this is an edit of a meeting and not a new meeting get the old setting from a store
            if (this.OutlookItem is Microsoft.Office.Interop.Outlook.AppointmentItem)
            {
                Microsoft.Office.Interop.Outlook.AppointmentItem appointment = (Microsoft.Office.Interop.Outlook.AppointmentItem)this.OutlookItem;
                if (OutlookAddIn1.ThisAddIn.ht.ContainsKey(appointment.GlobalAppointmentID))
                {
                    Hashtable ht = (Hashtable)OutlookAddIn1.ThisAddIn.ht[appointment.GlobalAppointmentID];
                    ComboBox cb = (ComboBox)ht[Key];
                    CheckBox cbox1 = (CheckBox)ht[cboxKey1];
                    CheckBox cbox2 = (CheckBox)ht[cboxKey2];
                    CheckBox cbox3 = (CheckBox)ht[cboxKey3];
                    this.comboBox1.SelectedIndex = cb.SelectedIndex;
                    this.checkBox1.Checked = cbox1.Checked;
                    this.checkBox2.Checked = cbox2.Checked;
                    this.checkBox3.Checked = cbox3.Checked;

                }
                else
                {
                    Hashtable ht = new Hashtable();
                    ht.Add(Key, this.comboBox1);
                    ht.Add(cboxKey1, this.checkBox1);
                    ht.Add(cboxKey2, this.checkBox2);
                    ht.Add(cboxKey3, this.checkBox3);
                    ht.Add("time", DateTime.UtcNow.ToLocalTime());
                    OutlookAddIn1.ThisAddIn.ht.Add(appointment.GlobalAppointmentID, ht);
                    this.comboBox1.SelectedIndex = 0;
                }
            }
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_SelectedCheckedChanged);
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_SelectedCheckedChanged);
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_SelectedCheckedChanged);
            // 
            // FormRegion2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Name = "FormRegion2";
            this.Size = new System.Drawing.Size(884, 150);
            this.FormRegionShowing += new System.EventHandler(this.FormRegion2_FormRegionShowing);
            this.FormRegionClosed += new System.EventHandler(this.FormRegion2_FormRegionClosed);
            
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void checkBox1_SelectedCheckedChanged(object sender,System.EventArgs e)
        {
            CheckBox comboBox = (CheckBox)sender;
            this.checkBox1.Checked = comboBox.Checked;
            if (this.OutlookItem is Microsoft.Office.Interop.Outlook.AppointmentItem)
            {
                Microsoft.Office.Interop.Outlook.AppointmentItem appointment = (Microsoft.Office.Interop.Outlook.AppointmentItem)this.OutlookItem;
                if (OutlookAddIn1.ThisAddIn.ht.ContainsKey(appointment.GlobalAppointmentID))
                {
                    Hashtable ht = (Hashtable)OutlookAddIn1.ThisAddIn.ht[appointment.GlobalAppointmentID];
                    ht.Remove(Key);
                    ht.Add(Key, this.comboBox1);
                    ht.Remove(cboxKey1);
                    ht.Add(cboxKey1, this.checkBox1);
                    ht.Remove(cboxKey2);
                    ht.Add(cboxKey2, this.checkBox2);
                    ht.Remove(cboxKey3);
                    ht.Add(cboxKey3, this.checkBox3);
                    OutlookAddIn1.ThisAddIn.ht.Remove(appointment.GlobalAppointmentID);
                    OutlookAddIn1.ThisAddIn.ht.Add(appointment.GlobalAppointmentID, ht);

                }
                else
                {
                    Hashtable ht = new Hashtable();
                    ht.Add(Key, this.comboBox1);
                    ht.Add(cboxKey1, this.checkBox1);
                    ht.Add(cboxKey2, this.checkBox2);
                    ht.Add(cboxKey3, this.checkBox3);
                    OutlookAddIn1.ThisAddIn.ht.Add(appointment.GlobalAppointmentID, ht);
                }
            }
        }
        private void checkBox2_SelectedCheckedChanged(object sender, System.EventArgs e)
        {
            CheckBox comboBox = (CheckBox)sender;
            this.checkBox2.Checked = comboBox.Checked;
            if (this.OutlookItem is Microsoft.Office.Interop.Outlook.AppointmentItem)
            {
                Microsoft.Office.Interop.Outlook.AppointmentItem appointment = (Microsoft.Office.Interop.Outlook.AppointmentItem)this.OutlookItem;
                if (OutlookAddIn1.ThisAddIn.ht.ContainsKey(appointment.GlobalAppointmentID))
                {
                    Hashtable ht = (Hashtable)OutlookAddIn1.ThisAddIn.ht[appointment.GlobalAppointmentID];
                    ht.Remove(Key);
                    ht.Add(Key, this.comboBox1);
                    ht.Remove(cboxKey1);
                    ht.Add(cboxKey1, this.checkBox1);
                    ht.Remove(cboxKey2);
                    ht.Add(cboxKey2, this.checkBox2);
                    ht.Remove(cboxKey3);
                    ht.Add(cboxKey3, this.checkBox3);
                    OutlookAddIn1.ThisAddIn.ht.Remove(appointment.GlobalAppointmentID);
                    OutlookAddIn1.ThisAddIn.ht.Add(appointment.GlobalAppointmentID, ht);

                }
                else
                {
                    Hashtable ht = new Hashtable();
                    ht.Add(Key, this.comboBox1);
                    ht.Add(cboxKey1, this.checkBox1);
                    ht.Add(cboxKey2, this.checkBox2);
                    ht.Add(cboxKey3, this.checkBox3);
                    OutlookAddIn1.ThisAddIn.ht.Add(appointment.GlobalAppointmentID, ht);
                }
            }
        }
        private void checkBox3_SelectedCheckedChanged(object sender, System.EventArgs e)
        {
            CheckBox comboBox = (CheckBox)sender;
            this.checkBox3.Checked = comboBox.Checked;
            if (this.OutlookItem is Microsoft.Office.Interop.Outlook.AppointmentItem)
            {
                Microsoft.Office.Interop.Outlook.AppointmentItem appointment = (Microsoft.Office.Interop.Outlook.AppointmentItem)this.OutlookItem;
                if (OutlookAddIn1.ThisAddIn.ht.ContainsKey(appointment.GlobalAppointmentID))
                {
                    Hashtable ht = (Hashtable)OutlookAddIn1.ThisAddIn.ht[appointment.GlobalAppointmentID];
                    ht.Remove(Key);
                    ht.Add(Key, this.comboBox1);
                    ht.Remove(cboxKey1);
                    ht.Add(cboxKey1, this.checkBox1);
                    ht.Remove(cboxKey2);
                    ht.Add(cboxKey2, this.checkBox2);
                    ht.Remove(cboxKey3);
                    ht.Add(cboxKey3, this.checkBox3);
                    OutlookAddIn1.ThisAddIn.ht.Remove(appointment.GlobalAppointmentID);
                    OutlookAddIn1.ThisAddIn.ht.Add(appointment.GlobalAppointmentID, ht);

                }
                else
                {
                    Hashtable ht = new Hashtable();
                    ht.Add(Key, this.comboBox1);
                    ht.Add(cboxKey1, this.checkBox1);
                    ht.Add(cboxKey2, this.checkBox2);
                    ht.Add(cboxKey3, this.checkBox3);
                    OutlookAddIn1.ThisAddIn.ht.Add(appointment.GlobalAppointmentID, ht);
                }
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
            //convert this to an appointment and you can save on it
            if (this.OutlookItem is Microsoft.Office.Interop.Outlook.AppointmentItem)
            {
                Microsoft.Office.Interop.Outlook.AppointmentItem appointment = (Microsoft.Office.Interop.Outlook.AppointmentItem)this.OutlookItem;
                if (OutlookAddIn1.ThisAddIn.ht.ContainsKey(appointment.GlobalAppointmentID))
                {
                    Hashtable ht = (Hashtable)OutlookAddIn1.ThisAddIn.ht[appointment.GlobalAppointmentID];
                    ht.Remove(Key);
                    ht.Add(Key, this.comboBox1);
                    OutlookAddIn1.ThisAddIn.ht.Remove(appointment.GlobalAppointmentID);
                    OutlookAddIn1.ThisAddIn.ht.Add(appointment.GlobalAppointmentID, ht);

                }
                else
                {
                    Hashtable ht = new Hashtable();
                    ht.Add(Key, this.comboBox1);
                    OutlookAddIn1.ThisAddIn.ht.Add(appointment.GlobalAppointmentID, ht);
                }
            }
        }

        #endregion

        #region Form Region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private static void InitializeManifest(Microsoft.Office.Tools.Outlook.FormRegionManifest manifest, Microsoft.Office.Tools.Outlook.Factory factory)
        {
            manifest.FormRegionName = "Proseeda";
            manifest.FormRegionType = Microsoft.Office.Tools.Outlook.FormRegionType.Adjoining;

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox1;

        public partial class FormRegion2Factory : Microsoft.Office.Tools.Outlook.IFormRegionFactory
        {
            public event Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler FormRegionInitializing;

            private Microsoft.Office.Tools.Outlook.FormRegionManifest _Manifest;

            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public FormRegion2Factory()
            {
                this._Manifest = Globals.Factory.CreateFormRegionManifest();
                FormRegion2.InitializeManifest(this._Manifest, Globals.Factory);
                this.FormRegionInitializing += new Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler(this.FormRegion2Factory_FormRegionInitializing);
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
                FormRegion2 form = new FormRegion2(formRegion);
                form.Factory = this;
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
        internal FormRegion2 FormRegion2
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.GetType() == typeof(FormRegion2))
                        return (FormRegion2)item;
                }
                return null;
            }
        }
    }
}
