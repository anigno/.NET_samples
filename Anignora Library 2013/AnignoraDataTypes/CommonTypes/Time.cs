using System;

namespace AnignoraDataTypes.CommonTypes
{
    public class Time : IComparable
    {
        #region Constructors

        public Time(int p_hour, int p_minute, int p_second, int p_millisecond)
        {
            Hour = p_hour;
            Minute = p_minute;
            Second = p_second;
            Millisecond = p_millisecond;
            repairTime(this);
        }

        public Time(DateTime p_dateTime)
            : this(p_dateTime.Hour, p_dateTime.Minute, p_dateTime.Second, p_dateTime.Millisecond)
        {
        }

        public Time(Time p_time)
            : this(p_time.Hour, p_time.Minute, p_time.Second, p_time.Millisecond)
        {
        }

        public Time()
            : this(0, 0, 0, 0)
        {
        }

        #endregion

        #region Public Methods

        public static Time FromMilliseconds(int p_milliseconds)
        {
            return new Time(0, 0, 0, p_milliseconds);
        }

        public int CompareTo(object p_obj)
        {
            Time t = (Time) p_obj;
            if (Hour < t.Hour) return -1;
            if (Hour > t.Hour) return 1;
            if (Minute < t.Minute) return -1;
            if (Minute > t.Minute) return 1;
            if (Second < t.Second) return -1;
            if (Second > t.Second) return 1;
            if (Millisecond < t.Millisecond) return -1;
            if (Millisecond > t.Millisecond) return 1;
            return 0;
        }

        public bool Equals(Time p_other)
        {
            if (ReferenceEquals(null, p_other)) return false;
            if (ReferenceEquals(this, p_other)) return true;
            return p_other.Hour == Hour && p_other.Millisecond == Millisecond && p_other.Minute == Minute && p_other.Second == Second;
        }

        public override bool Equals(object p_obj)
        {
            if (ReferenceEquals(null, p_obj)) return false;
            if (ReferenceEquals(this, p_obj)) return true;
            if (p_obj.GetType() != typeof (Time)) return false;
            return Equals((Time) p_obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Hour;
                result = (result*397) ^ Millisecond;
                result = (result*397) ^ Minute;
                result = (result*397) ^ Second;
                return result;
            }
        }

        public DateTime ToDateTime()
        {
            DateTime dt = new DateTime();
            dt = dt.AddMilliseconds(Millisecond);
            dt = dt.AddSeconds(Second);
            dt = dt.AddMinutes(Minute);
            dt = dt.AddHours(Hour);
            return dt;
        }

        public override string ToString()
        {
            return ToString("hh:mm:ss.fff");
        }

        /// <summary>
        /// Returns a formated representation of the time using place holders "hh, mm, ss, fff״
        /// </summary>
        public string ToString(string p_format)
        {
            return p_format.
                Replace("hh", Hour.ToString()).
                Replace("mm", Minute.ToString("00")).
                Replace("ss", Second.ToString("00")).
                Replace("fff", Millisecond.ToString("000"));
        }

        #endregion

        #region Public Properties

        public int Hour { get; set; }

        public int Millisecond { get; set; }

        public int Minute { get; set; }

        public int Second { get; set; }

        #endregion

        #region Private Methods

        private static void repairTime(Time p_time)
        {
            bool isRepairNeeded = true;
            p_time.Hour = p_time.Hour%24;
            while (isRepairNeeded)
            {
                isRepairNeeded = false;
                if (p_time.Millisecond > 999)
                {
                    p_time.Millisecond -= 1000;
                    p_time.Second++;
                    isRepairNeeded = true;
                }
                if (p_time.Second > 59)
                {
                    p_time.Second -= 60;
                    p_time.Minute++;
                    isRepairNeeded = true;
                }
                if (p_time.Minute > 59)
                {
                    p_time.Minute -= 60;
                    p_time.Hour++;
                    isRepairNeeded = true;
                }
                if (p_time.Hour > 23)
                {
                    p_time.Hour -= 24;
                    isRepairNeeded = true;
                }
                if (p_time.Millisecond < 0)
                {
                    p_time.Millisecond += 1000;
                    p_time.Second--;
                    isRepairNeeded = true;
                }
                if (p_time.Second < 0)
                {
                    p_time.Second += 60;
                    p_time.Minute--;
                    isRepairNeeded = true;
                }
                if (p_time.Minute < 0)
                {
                    p_time.Minute += 60;
                    p_time.Hour--;
                    isRepairNeeded = true;
                }
                if (p_time.Hour < 0)
                {
                    p_time.Hour += 24;
                    isRepairNeeded = true;
                }
            }
        }

        #endregion

        public static Time operator -(Time p_time1, Time p_time2)
        {
            Time t = new Time(
                p_time1.Hour - p_time2.Hour,
                p_time1.Minute - p_time2.Minute,
                p_time1.Second - p_time2.Second,
                p_time1.Millisecond - p_time2.Millisecond);
            repairTime(t);
            return t;
        }

        public static bool operator !=(Time p_time1, Time p_time2)
        {
            return !(p_time1 == p_time2);
        }

        public static Time operator +(Time p_time1, Time p_time2)
        {
            Time t = new Time(p_time1.Hour + p_time2.Hour, p_time1.Minute + p_time2.Minute, p_time1.Second + p_time2.Second, p_time1.Millisecond + p_time2.Millisecond);
            repairTime(t);
            return t;
        }

        public static bool operator ==(Time p_time1, Time p_time2)
        {
            return p_time1.CompareTo(p_time2) == 0;
        }

        public static bool operator <(Time p_time1, Time p_time2)
        {
            return p_time1.CompareTo(p_time2) == -1;
        }

        public static bool operator >(Time p_time1, Time p_time2)
        {
            return p_time1.CompareTo(p_time2) == 1;
        }
    }
}