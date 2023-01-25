namespace AnignoraProcesses
{
    /// <summary>
    /// Provide a thread safe property using lock mechanism
    /// includes an implicit T=ThreadSafeProperty(of T) and explicit for casting operators
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadSafeProperty<T>
    {
		#region (------  Fields  ------)
        private readonly object m_syncRoot=new object();
        private T m_value;
		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)
        public ThreadSafeProperty(ThreadSafeProperty<T> p_property)
        {
            Value = p_property.Value;
        }

        public ThreadSafeProperty(T p_value)
        {
            Value = p_value;
        }

        public ThreadSafeProperty()
        {
            Value = default(T);
        }
		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)
        public T Value
        {
            get
            {
                lock (m_syncRoot)
                {
                    return m_value;
                }

            }
            set
            {
                lock (m_syncRoot)
                {
                    m_value = value;
                }
            }
        }
		#endregion (------  Properties  ------)

		#region (------  Public static Methods  ------)
        public static implicit operator T(ThreadSafeProperty<T> p_property)
        {
            return p_property.m_value;
        }

        public static explicit operator ThreadSafeProperty<T>(T p_value)
        {
            return new ThreadSafeProperty<T>(p_value);
        }
		#endregion (------  Public static Methods  ------)

		#region (------  Public Methods  ------)
        public override string ToString()
        {
            return string.Format("(ThreadSafe){0}", Value);
        }
		#endregion (------  Public Methods  ------)
    }
}

/*
Example:
            ThreadSafeProperty<int> myInt=new ThreadSafeProperty<int>(10);
            int a = myInt;  //implicit
            myInt = (ThreadSafeProperty<int>)5;     //explicit
*/