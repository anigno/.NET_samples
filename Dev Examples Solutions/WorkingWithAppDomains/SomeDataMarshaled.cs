using System;

namespace WorkingWithAppDomains
{
    public class SomeDataMarshaled : MarshalByRefObject
    {
        public string Data { get; set; }
    }
}