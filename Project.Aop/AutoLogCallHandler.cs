using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Diagnostics;
using System.Reflection;
using Project.Log.Log4net;

namespace Project.Aop
{
    public class AutoLogCallHandler : ICallHandler
    {

        public AutoLogCallHandler() { }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            StringBuilder sb = null;
            ParameterInfo pi = null;

            string methodName = input.MethodBase.Name;
            Logger.Log.Info(string.Format("Enter method " + methodName));


            if (input.Arguments != null && input.Arguments.Count > 0)
            {
                sb = new StringBuilder();
                for (int i = 0; i < input.Arguments.Count; i++)
                {
                    pi = input.Arguments.GetParameterInfo(i);
                    sb.Append(pi.Name).Append(" : ").Append(input.Arguments[i]).AppendLine();
                }
                Logger.Log.Info(sb.ToString());
            }


            Stopwatch sw = new Stopwatch();
            sw.Start();

            IMethodReturn result = getNext()(input, getNext);
            //如果发生异常则，result.Exception != null 
            if (result.Exception != null)
            {
                Logger.Log.Info("Exception:" + result.Exception.Message);
                //必须将异常处理掉，否则无法继续执行 
                result.Exception = null;
            }

            sw.Stop();
            Logger.Log.Info(string.Format("Exit method {0}, use {1}.", methodName, sw.Elapsed));

            return result;
        }

        public int Order { get; set; }
    }
}
