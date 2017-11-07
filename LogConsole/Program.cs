using System;
using System.Threading;
using Project.Log.Log4net;

namespace LogConsole
{
    class Program
    {
        private static readonly log4net.ILog Log = Logger.Log;
        private static Timer _timer;
        static void Main()
        {
            _timer = new Timer(Output, null,3000,3000);
            //var content = "lufytest";
            //Log.Info(content);
            //Log.Debug(content);
            //Log.Error(content);
            //Log.Info(content);
            //Log.Info(content);
            //while (true)
            //{
            //    var msg = Console.ReadLine();
            //    LogWriter(msg);
            //}
            Console.ReadLine();

        }

        private static void Output(object state)
        {
            Console.WriteLine("a");
        }

        private static void Output(string msg)
        {
            Console.WriteLine(msg);
        }
        private static void LogWriter(string content)
        {
            Log.Info(content);
            Log.Debug(content);
            Log.Error(content);
            Log.Info(content);
            Log.Info(content);
        }
    }
}
