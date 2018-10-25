using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using System.Collections;
using System.Net.Sockets;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace WordAddIn1
{
    
    public partial class ThisAddIn
    {
        static public Hashtable ht = new Hashtable();//temp persistance for meeting content
        private TcpClient _tcpclient;
        private WindowEventHandler WindowSize;
        private System.IO.StreamReader _sReader;
        private System.IO.StreamWriter _sWriter;
        public static List<string> lst_storeddata = new List<string>();
        private DocumentEvents2_NewEventHandler New;
        private Boolean _isConnected;
        string name;
        string phone;
        string address;
        string passport;

        private void DocumentNew()
        {
            this.New += new Microsoft.Office.Interop.Word.
                DocumentEvents2_NewEventHandler(
                ThisDocument_New);
        }

        void ThisDocument_New()
        {
            MessageBox.Show("The Document.New event has fired.");
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            
            this.Application.DocumentBeforeSave +=
                new Word.ApplicationEvents4_DocumentBeforeSaveEventHandler(Application_DocumentBeforeSave);
            this.Application.DocumentOpen += new Word.ApplicationEvents4_DocumentOpenEventHandler(Application_DocumentOpen);
            WordAddIn1.ThisAddIn.ht = new Hashtable();
            //DocumentWindowSize();
            DocumentNew();
        }

        

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        void Application_DocumentBeforeSave(Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            StartClient(Doc);
        }

        void Application_DocumentOpen(Word.Document Doc)
        {


            WordAddIn1.ThisAddIn.ht.Add(Doc.Name, DateTime.UtcNow.ToLocalTime());
                
            
            
        }

        private void StartClient(Word.Document Doc)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];
            //get client name
            
            DateTime dtStart = (DateTime)WordAddIn1.ThisAddIn.ht[Doc.Name];
            
            // Connect to a remote device.  
            try
            {
                //server ip
                String ipAddress = "127.0.0.1";
                //String ipAddress = "192.168.43.15";
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
                    
                    DateTime dtEnd = DateTime.UtcNow.ToLocalTime();
                    int hour = ((int)(dtEnd - dtStart).TotalMinutes) / 60;
                    int minute = ((int)(dtEnd - dtStart).TotalMinutes) % 60;
                    NetworkStream serverStream = _tcpclient.GetStream();
                    //capturing the meeting time
                    string clientData = "{\"Name\": \"" + Doc.Path.Substring(Doc.Path.LastIndexOf("\\")+1) + "\",\"Hour\": \"" +
                        Convert.ToString(hour) + "." + Convert.ToString(minute) + "\", \"Description\": \"Editing Document Named" + Doc.Name + "\",\"Source\": \"Document Edit\"" +
                        ",\"Age\": \"61\"," +
                        "\"Country\": \"6\"," +
                        "\"Address\":\"Ap #897-1459 Quam Avenue\",\"Married\": \"false\"}";
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
