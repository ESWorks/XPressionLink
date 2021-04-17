using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPressionService
{
    public class CallbackHandler : ITimerDuplexCallback
    {
        public Action<long> Millisecond;

        public CallbackHandler(Action<long> Millisecond)
        {
            this.Millisecond = Millisecond;
        }

        public void MillisecondCounter(long result)
        {
            Millisecond(result);
        }
    }
}
