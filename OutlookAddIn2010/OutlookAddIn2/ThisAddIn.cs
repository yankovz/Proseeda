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

namespace OutlookAddIn2
{
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
        public static Hashtable customerData = new Hashtable();
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

            
            sent = outlookNameSpace.GetDefaultFolder(
                    Microsoft.Office.Interop.Outlook.
                    OlDefaultFolders.olFolderOutbox);

            itemsSent = sent.Items;

            itemsSent.ItemAdd +=
                new Outlook.ItemsEvents_ItemAddEventHandler(items_ItemAdd);//register the event handler for calender meeting

            
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see https://go.microsoft.com/fwlink/?LinkId=506785
        }

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

        private void StartClient(Outlook.MailItem appointment)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];
            
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
                    Microsoft.Office.Interop.Outlook.ItemProperty propTime = appointment.ItemProperties["time"];
                    DateTime dtStart;
                    if (propTime != null)
                    {
                        dtStart = Convert.ToDateTime(propTime.Value);
                    }
                    else
                    {
                        dtStart = DateTime.UtcNow.ToLocalTime();
                    }
                    DateTime dtEnd = DateTime.UtcNow.ToLocalTime();
                    int time = ((int)(dtEnd - dtStart).TotalMinutes);
                    
                    String date = dtEnd.Month + "/" + dtEnd.Day + "/" + dtEnd.Year;

                    String minute = dtEnd.Minute.ToString();
                    if (dtEnd.Minute == 0)
                    {
                        minute = dtEnd.Minute + "0";
                    };
                    if (dtEnd.Minute == 0)
                    {
                        minute = dtEnd.Minute + "0";
                    }
                    String second = dtEnd.Second.ToString();
                    if (dtEnd.Second == 0)
                    {
                        second = dtEnd.Second + "0";
                    }
                    String eventTime = dtEnd.Hour + ":" + minute + ":" + second;
                    NetworkStream serverStream = _tcpclient.GetStream();
                    Microsoft.Office.Interop.Outlook.ItemProperty prop02 = appointment.ItemProperties["SelectedItem"];
                    if (prop02 != null)
                    {
                        String Name = prop02.Value.ToString().Substring(0, prop02.Value.ToString().IndexOf(","));
                        String Case = prop02.Value.ToString().Substring(prop02.Value.ToString().IndexOf("(") + 1);
                        Case = Case.Substring(0, Case.Length - 1);
                        String cn = appointment.SenderEmailAddress.Substring(
                            appointment.SenderEmailAddress.IndexOf("CN") + 3);
                        String user = cn.Substring(
                            cn.IndexOf("CN") + 3);
                        string clientData = "{\"Name\": \"" + Name + "\",\"Case\": \"" + Case +
                            "\",\"date\": \"" + date + "\",\"time\":\"" + eventTime + "\",\"Duration\": \"" +
                            Convert.ToString(time) +
                            "\", \"Description\": \"" + appointment.Subject +
                            "\",\"user\": \"" + user +
                            "\",\"Source\": \"Email\",\"msgRequestInsert\":\"insert\"" +
                            "}";


                        byte[] outStream = Encoding.ASCII.GetBytes(clientData);
                        serverStream.Write(outStream, 0, outStream.Length);
                        serverStream.Flush();
                    }


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
    

    private void StartClient(Outlook.AppointmentItem appointment)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];
            
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
                    String date = dtEnd.Month + "/" + dtEnd.Day + "/" + dtEnd.Year;
                    String minute = dtEnd.Minute.ToString();
                    if(dtEnd.Minute==0)
                    {
                        minute = dtEnd.Minute + "0";
                    };
                    if(dtEnd.Minute==0)
                    {
                        minute = dtEnd.Minute + "0";
                    }
                    String second = dtEnd.Second.ToString();
                    if (dtEnd.Second == 0)
                    {
                        second = dtEnd.Second + "0";
                    }
                    String eventTime = dtEnd.Hour + ":" + minute + ":" + second;
                    int time = ((int)(dtEnd - dtStart).TotalMinutes);
                    
                    NetworkStream serverStream = _tcpclient.GetStream();
                    //capturing the meeting time
                    Microsoft.Office.Interop.Outlook.ItemProperty prop02 = appointment.ItemProperties["SelectedItem"];
                    if (prop02 != null)
                    {
                        String Name = prop02.Value.ToString().Substring(0, prop02.Value.ToString().IndexOf(","));
                        String Case = prop02.Value.ToString().Substring(prop02.Value.ToString().IndexOf("(") + 1);
                        Case = Case.Substring(0, Case.Length - 1);
                        String clientData = "{\"Name\": \"" + Name + "\",\"Case\": \"" + Case +
                            "\",\"date\": \"" + date + "\",\"time\":\"" + eventTime + "\",\"Duration\": \"" +
                            Convert.ToString(time) +
                            "\", \"Description\": \"" + appointment.Subject +
                            "\",\"user\": \"" + appointment.Organizer +
                            "\",\"Source\": \"Calender Meeting Actual Time\",\"msgRequestInsert\":\"insert\"" +
                            "}";

                        byte[] outStream = Encoding.ASCII.GetBytes(clientData);
                        serverStream.Write(outStream, 0, outStream.Length);
                        serverStream.Flush();
                        System.Threading.Thread.Sleep(100);
                        //capturing the time it took to setup a meeting
                        Microsoft.Office.Interop.Outlook.ItemProperty propTime = appointment.ItemProperties["time"];
                        if (propTime != null)
                        {
                            dtStart = Convert.ToDateTime(propTime.Value);
                        }
                        else
                        {
                            dtStart = DateTime.UtcNow.ToLocalTime();
                        }
                        dtEnd = DateTime.UtcNow.ToLocalTime();
                        time = ((int)(dtEnd - dtStart).TotalMinutes);

                        clientData = "{\"Name\": \"" + Name + "\",\"Case\": \"" + Case +
                            "\",\"date\": \"" + date + "\",\"time\":\"" + eventTime + "\",\"Duration\": \"" +
                            Convert.ToString(time) +
                            "\", \"Description\": \"" + appointment.Subject +
                            "\",\"user\": \"" + appointment.Organizer +
                            "\",\"Source\": \"Calender Meeting Setup Time\",\"msgRequestInsert\":\"insert\"" +
                            "}";


                        outStream = Encoding.ASCII.GetBytes(clientData);
                        serverStream.Write(outStream, 0, outStream.Length);
                        serverStream.Flush();
                    }


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
    }
}
