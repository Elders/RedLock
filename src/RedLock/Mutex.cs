using System;

namespace RedLock
{
    public class Mutex
    {
        public Mutex(object resource, byte[] value, TimeSpan validity)
        {
            Resource = resource;
            Value = value;
            Validity = validity;
        }

        public object Resource { get; private set; }

        public byte[] Value { get; private set; }

        public TimeSpan Validity { get; private set; }
    }
}
