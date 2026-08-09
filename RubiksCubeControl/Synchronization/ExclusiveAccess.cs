using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeControl.Synchronization
{
    public static class ExclusiveAccess
    {
        private static UInt64 _lockObject = 0;
        public static bool TryObtainLock() => (0 == Interlocked.CompareExchange(ref _lockObject, 1, 0));
        public static void ReleaseLock() => Interlocked.Exchange(ref _lockObject, 0);
    }
}
