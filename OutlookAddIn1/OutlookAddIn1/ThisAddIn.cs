using System;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace OutlookAddIn1
{
    /*
     * Class AddIn to handle outlook event and synchronize them into Proseeda store
     */
    public partial class ThisAddIn
    {

        static public Hashtable ht = new Hashtable();//temp persistance for meeting content
        private const string PROSEEDA = "proseeda";
        private const string Key = "combo1";
        private const string cboxKey1 = "cbox1";
        private const string cboxKey2 = "cbox2";
        private const string cboxKey3 = "cbox3";
        Outlook.Inspectors inspectors;
        Outlook.NameSpace outlookNameSpace;
        Outlook.MAPIFolder calender;
        Outlook.MAPIFolder sent;
        Outlook.Items items;
        Outlook.Items itemsSent;
        private TcpClient _tcpclient;

        private System.IO.StreamReader _sReader;
        private System.IO.StreamWriter _sWriter;
        public static List<string> lst_storeddata = new List<string>();

        private Boolean _isConnected;
        string name;
        string phone;
        string address;
        string passport;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            inspectors = this.Application.Inspectors;

            /*inspectors.NewInspector +=
            new Microsoft.Office.Interop.Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);
            */

            outlookNameSpace = this.Application.GetNamespace("MAPI");
            //getting the calender folder
            calender = outlookNameSpace.GetDefaultFolder(
                    Microsoft.Office.Interop.Outlook.
                    OlDefaultFolders.olFolderCalendar);

            items = calender.Items;

            items.ItemAdd +=
                new Outlook.ItemsEvents_ItemAddEventHandler(items_ItemAdd);//register the event handler for calender meeting

            items.ItemRemove +=
                new Outlook.ItemsEvents_ItemRemoveEventHandler(items_ItemRemove);//register the event handler for calender meeting
            sent = outlookNameSpace.GetDefaultFolder(
                    Microsoft.Office.Interop.Outlook.
                    OlDefaultFolders.olFolderOutbox);

            itemsSent = sent.Items;

            itemsSent.ItemAdd +=
                new Outlook.ItemsEvents_ItemAddEventHandler(items_ItemAdd);//register the event handler for calender meeting

            itemsSent.ItemRemove +=
                new Outlook.ItemsEvents_ItemRemoveEventHandler(items_ItemRemove);//register the event handler for calender meeting
        }



        /*
         * Method is used when a new appointment is saved in the calender
         * 
         */
        void items_ItemAdd(object Item)
        {
            
            if (Item is Outlook.AppointmentItem)
            {

                if (Item != null)
                {
                    Outlook.AppointmentItem appointment = (Outlook.AppointmentItem)Item;

                    StartClient(appointment);

                }

            }
            if (Item is Outlook.MailItem)
            {

                if (Item != null)
                {
                    Outlook.MailItem appointment = (Outlook.MailItem)Item;

                    StartClient(appointment);

                }


            }
        }

        void items_ItemRemove()
        {
            
            System.Windows.Forms.MessageBox.Show("gow the appointment delete");

        }


        //this is used when a new event is opened

        /*void Inspectors_NewInspector(Microsoft.Office.Interop.Outlook.Inspector inspector)
        {
            if (inspector != null)
            {
                object item = inspector.CurrentItem;
                if (item != null)
                {
                    if (item is Outlook.MailItem)
                    {
                        Outlook.MailItem mailItem = item as Outlook.MailItem;
                        if (mailItem != null)
                        {
                            if (mailItem.EntryID == null)
                            {
                                //mailItem.Subject = "This text was added by using code";
                                //mailItem.Body = "This text was added by using code";
                            }

                        }
                        // use mail 
                    }
                    else if (item is Outlook.AppointmentItem)
                    {
                        Outlook.AppointmentItem appointment = item as Outlook.AppointmentItem;
                        if (appointment != null)
                        {
                            if (appointment.EntryID == null)
                            {
                                
                                //appointment.Subject = "This text was added by using code";
                                //appointment.Body = "This text was added by using code";
                                
                            }

                        }
                        // use appointment 
                    }
                }
            }
                    
        }*/

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see https://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
        private void StartClient(Outlook.AppointmentItem appointment)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];
            //get client name
            Hashtable ht = (Hashtable)OutlookAddIn1.ThisAddIn.ht[appointment.GlobalAppointmentID];
            ComboBox cb = (ComboBox)ht[Key];
            // Connect to a remote device.  
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
                    DateTime dtStart = appointment.Start;
                    DateTime dtEnd = appointment.End;
                    int hour = ((int)(dtEnd - dtStart).TotalMinutes) / 60;
                    int minute = ((int)(dtEnd - dtStart).TotalMinutes) % 60;
                    NetworkStream serverStream = _tcpclient.GetStream();
                    //capturing the meeting time
                    String Name = cb.SelectedItem.ToString().Substring(0,cb.SelectedItem.ToString().IndexOf(")")+1);
                    String Case = cb.SelectedItem.ToString().Substring(cb.SelectedItem.ToString().IndexOf(")")+2);
                    String clientData = "{\"Name\": \"" + Name + "\",\"Case\": \"" + Case + "\",\"Hour\": \"" +
                        Convert.ToString(hour) + "." + Convert.ToString(minute) +
                        "\", \"Description\": \"" + appointment.Subject +
                        "\",\"user\": \""+appointment.Organizer +
                        "\",\"Source\": \"Calender Meeting Actual Time\",\"msgRequestInsert\":\"insert\"" +
                        "}";
                    
                    byte[] outStream = Encoding.ASCII.GetBytes(clientData);
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                    //capturing the time it took to setup a meeting
                    dtStart = (DateTime)ht["time"];
                    dtEnd = DateTime.UtcNow.ToLocalTime();
                    hour = ((int)(dtEnd - dtStart).TotalMinutes) / 60;
                    minute = ((int)(dtEnd - dtStart).TotalMinutes) % 60;
                    clientData = "{\"Name\": \"" + Name + "\",\"Case\": \"" + Case + "\",\"Hour\": \"" +
                        Convert.ToString(hour) + "." + Convert.ToString(minute) +
                        "\", \"Description\": \"" + appointment.Subject +
                        "\",\"user\": \"" + appointment.Organizer  +
                        "\",\"Source\": \"Calender Meeting Setup Time\",\"msgRequestInsert\":\"insert\"" +
                        "}";
                    

                    outStream = Encoding.ASCII.GetBytes(clientData);
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();


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

        

        
        private void StartClient(Outlook.MailItem appointment)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];
            //get client name
            Hashtable ht = (Hashtable)OutlookAddIn1.ThisAddIn.ht[PROSEEDA];
            ComboBox cb = (ComboBox)ht[Key];
            // Connect to a remote device.  
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
                    DateTime dtStart = (DateTime)ht["time"];
                    DateTime dtEnd = DateTime.UtcNow.ToLocalTime();
                    int hour = ((int)(dtEnd - dtStart).TotalMinutes) / 60;
                    int minute = ((int)(dtEnd - dtStart).TotalMinutes) % 60;
                    NetworkStream serverStream = _tcpclient.GetStream();
                    String Name = cb.SelectedItem.ToString().Substring(0, cb.SelectedItem.ToString().IndexOf(")")+1);
                    String Case = cb.SelectedItem.ToString().Substring(cb.SelectedItem.ToString().IndexOf(")") + 2);
                    String cn = appointment.SenderEmailAddress.Substring(
                        appointment.SenderEmailAddress.IndexOf("CN") + 3);
                    String user = cn.Substring(
                        cn.IndexOf("CN") + 3);
                    string clientData = "{\"Name\": \"" + Name + "\",\"Case\": \"" + Case + "\",\"Hour\": \"" +
                        Convert.ToString(hour) + "." + Convert.ToString(minute) + 
                        "\", \"Description\": \"" + appointment.Subject +
                        "\",\"user\": \"" + user  +
                        "\",\"Source\": \"Email\",\"msgRequestInsert\":\"insert\"" + 
                        "}";


                    byte[] outStream = Encoding.ASCII.GetBytes(clientData);
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();


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
    }

}
