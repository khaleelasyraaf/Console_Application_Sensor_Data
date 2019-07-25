using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectorHub;

namespace LHAppConsoleApplication2
{
    /// <summary>
    /// Class that handles all learning hub related interactions
    /// </summary>
    public class LearningHubManager
    {
        ConnectorHub.ConnectorHub myConnector;
        TCPManager myTCPmanager;

        private string s = "s";
        private string Question = "";
        private string Acc_X = "";
        private string Acc_Y = "";
        private string Acc_Z = "";
        private string Gyro_X = "";
        private string Gyro_Y = "";
        private string Gyro_Z = "";

        public LearningHubManager(TCPManager myTCPmanager)
        {
            initializeLearningHub();
            myConnector.startRecordingEvent += MyConnector_startRecordingEvent;
            myConnector.stopRecordingEvent += MyConnector_stopRecordingEvent;
            this.myTCPmanager = myTCPmanager;
            myTCPmanager.AccDataChanged += MyTCPmanager_AccDataChanged;
        }

        private void MyTCPmanager_AccDataChanged(object sender, TCPManager.AccelerometerDataChangedEventArgs e)
        {
            s = e.s;
            string indexParameter = s.Substring(0,2);
            Console.WriteLine("indexParameter: "+ indexParameter);
            switch (indexParameter)
            {
                case "AX":
                    Acc_X = s.Substring(s.IndexOf(":") + 1);
                    break;
                case "AY":
                    Acc_Y = s.Substring(s.IndexOf(":") + 1);
                    break;
                case "AZ":
                    Acc_Z = s.Substring(s.IndexOf(":") + 1);
                    break;
                case "GX":
                    Gyro_X = s.Substring(s.IndexOf(":") + 1);
                    break;
                case "GY":
                    Gyro_Y = s.Substring(s.IndexOf(":") + 1);
                    break;
                case "GZ":
                    Gyro_Z = s.Substring(s.IndexOf(":") + 1);
                    break;
                case "QQ":
                    Question = s.Substring(s.IndexOf(":") + 1);
                    break;

            }
            SendData();
        }

        #region EventHandlers
        /// <summary>
        /// Stop recording message received
        /// </summary>
        /// <param name="sender"></param>
        private void MyConnector_stopRecordingEvent(object sender)
        {
            Console.WriteLine("Stop recording Received");

        }

        /// <summary>
        /// start recording message received
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private static void MyConnector_startRecordingEvent(object sender)
        {
            Console.WriteLine("Start recording Received");
        }
        #endregion

        /// <summary>
        /// Method for init Learning Hub
        /// </summary>
        public void initializeLearningHub()
        {
            myConnector = new ConnectorHub.ConnectorHub();
            myConnector.init();
            myConnector.sendReady();
            SetValueNames();
            Console.WriteLine("Learning Hub Successfully initialized");
        }

        /// <summary>
        /// Method for setting the values that LH has to store
        /// </summary>
        public void SetValueNames()
        {
            List<String> names = new List<string>();
            names.Add("Question");
            names.Add("Acc_X");
            names.Add("Acc_Y");
            names.Add("Acc_Z");
            names.Add("Gyro_X");
            names.Add("Gyro_Y");
            names.Add("Gyro_Z");
            myConnector.setValuesName(names);

        }
        /// <summary>
        /// Call this  method to store frames. Calls Learning Hub's Storeframe Method
        /// </summary>
        public void SendData()
        {
            List<string> values = new List<string>();
            values.Add(Question);
            values.Add(Acc_X);
            values.Add(Acc_Y);
            values.Add(Acc_Z);
            values.Add(Gyro_X);
            values.Add(Gyro_Y);
            values.Add(Gyro_Z);

            myConnector.storeFrame(values);
            
            Console.WriteLine("AccX:" + Acc_X);
            Console.WriteLine("AccY:" + Acc_Y);
            Console.WriteLine("AccZ:" + Acc_Z);
            Console.WriteLine("GyroX:" + Gyro_X);
            Console.WriteLine("GyroY:" + Gyro_Y);
            Console.WriteLine("GyroZ:" + Gyro_Z);
            Console.WriteLine("Question:" + Question);
            Question = "";
        }

        public void StartRecording()
        {
            Console.WriteLine("Start Recording Called");
        }
    }
}
