using System;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Project.Aop;
using Project.Log.Log4net;

namespace LogConsole
{
    class Program
    {
        private static Timer _timer;
        static void Main()
        {
            //PolicyInjection.SetPolicyInjector(new PolicyInjector());
            Employee emp = PolicyInjection.Create<Employee>();

            emp.Name = "Lele";

            emp.Work();
            Console.WriteLine(emp);
            Console.Read();
        }

        private static void Output(object state)
        {
            Console.WriteLine("a");
        }
        [AutoLogCallHandler()]
        private static void Output(string msg)
        {
            Console.WriteLine("Now is {0}", msg);
        }
        
        private static void LogWriter(string content)
        {
            Logger.Log.Info(content);
            Logger.Log.Debug(content);
            Logger.Log.Error(content);
            Logger.Log.Info(content);
            Logger.Log.Info(content);
        }
    }
}
