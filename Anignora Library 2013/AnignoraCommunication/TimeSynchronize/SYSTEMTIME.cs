using System.Runtime.InteropServices;

namespace AnignoraCommunication.TimeSynchronize
{
    // SYSTEMTIME structure used by SetSystemTime
    
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public short year;
        public short month;
        public short dayOfWeek;
        public short day;
        public short hour;
        public short minute;
        public short second;
        public short milliseconds;
    }
}