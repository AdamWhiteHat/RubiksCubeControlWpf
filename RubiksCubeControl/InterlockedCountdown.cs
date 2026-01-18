using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeControl
{
    public class InterlockedCountdown
    {
        public event EventHandler CountdownComplete;
        private UInt64 _lockObject = 0;
        private UInt64 _resetValue;

        public InterlockedCountdown(int count)
        {
            _resetValue = (UInt64)count;
        }

        public bool IsCompleted()
        {
            return (Interlocked.Read(ref _lockObject) == 0);
        }

        public void Reset()
        {
            Interlocked.Exchange(ref _lockObject, _resetValue);
        }

        public void Signal()
        {
            if (Interlocked.Decrement(ref _lockObject) == 0)
            {
                RaiseCountdownComplete();
            }
        }

        private void RaiseCountdownComplete()
        {
            EventHandler handler = CountdownComplete;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
