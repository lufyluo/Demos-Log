using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Project.Aop
{
    public class AutoLogCallHandlerAttribute : HandlerAttribute
    {

        public override ICallHandler CreateHandler(Microsoft.Practices.Unity.IUnityContainer container)
        {
            return new AutoLogCallHandler() { Order = this.Order };
        }
    }
}
