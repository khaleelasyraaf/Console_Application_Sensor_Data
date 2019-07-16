using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LHAppConsoleApplication2
{

    class Program
    {       

        static void Main(string[] args)
        {
            TCPManager myTCPManager = new TCPManager();
            LearningHubManager myLHManager = new LearningHubManager(myTCPManager);
            

            do
            {
                Console.WriteLine("The service is running. Press Enter to exit");
                Console.ReadKey();
            } while (Console.ReadKey().Key != ConsoleKey.Enter);
            myTCPManager.CloseTCPListener();
            CloseApplication();

        }

        static void CloseApplication()
        {
            Environment.Exit(0);
        }


    }
}
