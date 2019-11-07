using System;

namespace QueuingSystem
{
    public class SafeIdGenerator
    {
        private static int _nextId = 1;
        private static Array _lock = new double[0];
        
        public string GetNextId()
        {
            // TODO: use Interlocked?
            lock (_lock)
            {
                var res = _nextId.ToString();
                _nextId += 1;
                return res;
            }
        }
    }
}