using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeControl.Synchronization
{
    public class InterlockedCountdown : IDisposable
    {
        public event EventHandler CountdownComplete;

        public string Name { get { return _name; } }
        public int TriggerValue { get { return (int)_resetValue; } }
        public bool IsDisposed { get { return _isDisposed; } }

        private bool _isDisposed = true;
        private ulong _lockObject = 0;
        private ulong _resetValue;
        private string _name;

        public InterlockedCountdown(string name, int count)
        {
            _isDisposed = false;
            _resetValue = (ulong)count;
            _lockObject = 0;
            _name = name;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Delegate[] registeredListeners = CountdownComplete.GetInvocationList();

                foreach (Delegate listener in registeredListeners)
                {
                    CountdownComplete -= (EventHandler)listener;
                }

                _isDisposed = true;
            }
        }

        public bool IsCompleted()
        {
            return Interlocked.Read(ref _lockObject) == 0;
        }

        public void Reset()
        {
            Interlocked.Exchange(ref _lockObject, _resetValue);
        }

        public void Signal()
        {
            ulong newValue = Interlocked.Decrement(ref _lockObject);
            if (newValue == 0)
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
