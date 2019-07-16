using Eneter.Messaging.EndPoints.TypedMessages;
using Eneter.Messaging.MessagingSystems.MessagingSystemBase;
using Eneter.Messaging.MessagingSystems.TcpMessagingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHAppConsoleApplication2
{
    public class MyRequest
    {
        public string Text { get; set; }
    }

    // Response message type
    public class MyResponse
    {
        public int Length { get; set; }
    }

    public class TCPManager
    {
        #region Events
        public event EventHandler<AccelerometerDataChangedEventArgs> AccDataChanged;
        /// <summary>
        /// Event raised when the grip pressure has changed over the last iteration
        /// </summary>
        protected virtual void OnAccDataChanged(AccelerometerDataChangedEventArgs AccEvent)
        {
            EventHandler<AccelerometerDataChangedEventArgs> handler = AccDataChanged;
            if (handler != null)
            {
                handler(this, AccEvent);
            }
        }

        public class AccelerometerDataChangedEventArgs : EventArgs
        {
            public string s { get; set; }
        }

        #endregion

        #region Variables

        public IDuplexTypedMessageReceiver<MyResponse, MyRequest> myReceiver;
        public IDuplexTypedMessagesFactory aReceiverFactory;
        public IMessagingSystemFactory aMessaging;
        public IDuplexInputChannel anInputChannel;
        public string IPAddress = "tcp://192.168.0.100:8800/";
        #endregion

        #region Properties
        private static readonly Lazy<TCPManager> lazy = new Lazy<TCPManager>(() => new TCPManager());
        public static TCPManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        #endregion
        public TCPManager()
        {
            // Create message receiver receiving 'MyRequest' and receiving 'MyResponse'.
            aReceiverFactory = new DuplexTypedMessagesFactory();
            myReceiver = aReceiverFactory.CreateDuplexTypedMessageReceiver<MyResponse, MyRequest>();

            // Subscribe to handle messages.
            myReceiver.MessageReceived += OnMessageReceived;

            // Create TCP messaging.
            // Note: 192.168.0.100 is the IP from the wireless router (no internet)
            // and 8800 is the socket.
            aMessaging = new TcpMessagingSystemFactory();
            anInputChannel = aMessaging.CreateDuplexInputChannel(IPAddress);

            // Attach the input channel and start to listen to messages.
            try
            {
                myReceiver.AttachDuplexInputChannel(anInputChannel);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        /// <summary>
        /// CLoses the thread when application is stopped
        /// 
        /// </summary>
        public void CloseTCPListener()
        {

            // Detach the input channel and stop listening.
            // It releases the thread listening to messages.
            myReceiver.DetachDuplexInputChannel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMessageReceived(object sender,
              TypedRequestReceivedEventArgs<MyRequest> e)
        {
            Console.WriteLine("Received: " + e.RequestMessage.Text);
            AccelerometerDataChangedEventArgs args = new AccelerometerDataChangedEventArgs();
            args.s = e.RequestMessage.Text;
            OnAccDataChanged(args);
            // Create the response message.
            MyResponse aResponse = new MyResponse();
            aResponse.Length = e.RequestMessage.Text.Length;

            // Send the response message back to the client.
            myReceiver.SendResponseMessage(e.ResponseReceiverId, aResponse);
        }

    }
}
