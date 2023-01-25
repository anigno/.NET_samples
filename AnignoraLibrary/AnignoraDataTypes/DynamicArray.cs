using System.Text;
using System.Collections;

namespace AnignoraDataTypes
{
    /// <summary>
    /// Dynamically size changed array
    /// </summary>
    public class DynamicArray<T> : IEnumerable
    {
        #region Constructors

        public DynamicArray(int initSize)
        {
            _buffer = new T[initSize];
        }

        #endregion

        #region Public Methods

        public void Append(T[] items, int start, int length)
        {
            lock (_syncObject)
            {
                //If needed, dynamically increase size
                if (_buffer.Length < _CurrentLength + length)
                {
                    T[] newBuffer = new T[(_CurrentLength + length)*2];
                    for (int a = 0; a < _CurrentLength; a++)
                    {
                        newBuffer[a] = _buffer[a];
                    }
                    T[] oldBuffer = _buffer;
                    _buffer = newBuffer;
                    oldBuffer = null;
                }
                for (int a = start; a < start + length; a++)
                {
                    _buffer[_CurrentLength++] = items[a];
                }
            }
        }

        public T[] Cut(int start, int length)
        {
            T[] retItemsThatWasCut = new T[length];
            int ItemsToSkipForPostfix = 0;
            lock (_syncObject)
            {
                for (int index = 0; index < _CurrentLength; index++)
                {
                    if (index >= start && index <= start + length - 1)
                    {
                        retItemsThatWasCut[index - start] = _buffer[index];
                        ItemsToSkipForPostfix++;
                    }
                    else
                    {
                        _buffer[index - ItemsToSkipForPostfix] = _buffer[index];
                    }
                }
                _CurrentLength -= length;
            }
            return retItemsThatWasCut;
        }

        public T[] ToArray()
        {
            T[] retItems = null;
            lock (_syncObject)
            {
                retItems = new T[_CurrentLength];
                for (int a = 0; a < _CurrentLength; a++)
                {
                    retItems[a] = _buffer[a];
                }
            }
            return retItems;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int a = 0; a < _CurrentLength; a++)
            {
                sb.Append(_buffer[a]);
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public IEnumerator GetEnumerator()
        {
            return ToArray().GetEnumerator();
        }

        #endregion

        #region Public Properties

        public T this[int index]
        {
            get { return _buffer[index]; }
        }

        public int Length
        {
            get { return _CurrentLength; }
        }

        public int DynamicSize
        {
            get { return _buffer.Length; }
        }

        #endregion

        #region Fields

        private object _syncObject = new object();
        private int _CurrentLength;
        private T[] _buffer;

        #endregion
    }
}