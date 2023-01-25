using System;

namespace AnignoraDataTypes
{
    /// <summary>
    /// Queue implementation, using circular pattern over an array. Dynamically resized when needed
    /// </summary>
    public class QueueCircularDynamic<T>
    {
        #region Constructors

        public QueueCircularDynamic(int initialSize)
        {
            if (initialSize <= 0)
            {
                throw new ArgumentException(string.Format("QueueCircularDynamic for type: {0}, Initial size must be greater then zero!", typeof (T)));
            }
            mItemsArray = new T[initialSize];
        }

        #endregion

        #region Public Methods

        public T Dequeue()
        {
            if (mAvaliable == 0)
            {
                throw new InvalidOperationException(string.Format("QueueCircularDynamic for type: {0} is empty!", typeof (T)));
            }
            T retItem = mItemsArray[mTail++];
            if (mTail > mItemsArray.Length - 1) mTail = 0;
            mAvaliable--;
            return retItem;
        }

        public T[] Dequeue(int numberOfItems)
        {
            T[] retBuffer = new T[numberOfItems];
            for (int a = 0; a < numberOfItems; a++)
            {
                retBuffer[a] = Dequeue();
            }
            return retBuffer;
        }

        public void Enqueue(T item)
        {
            if (mAvaliable + 1 > mItemsArray.Length)
            {
                //Enlarge array
                T[] tArray = mItemsArray;
                QueueCircularDynamic<T> newQueue = new QueueCircularDynamic<T>(mItemsArray.Length*2);
                while (mAvaliable > 0)
                {
                    T t = Dequeue();
                    newQueue.Enqueue(t);
                }
                mItemsArray = newQueue.mItemsArray;
                mHead = newQueue.mHead;
                mTail = newQueue.mTail;
                mAvaliable = newQueue.mAvaliable;
                //Clear old array's data
                for (int a = 0; a < tArray.Length; a++)
                {
                    tArray[a] = default(T);
                }
                if (OnQueueCircularDynamicSizeChanged != null) OnQueueCircularDynamicSizeChanged(mItemsArray.Length);
            }
            mItemsArray[mHead++] = item;
            if (mHead > mItemsArray.Length - 1) mHead = 0;
            mAvaliable++;
        }

        public void Enqueue(T[] items, int offset, int count)
        {
            for (int a = offset; a < offset + count; a++)
            {
                Enqueue(items[a]);
            }
        }

        #endregion

        #region Public Properties

        public int Avaliable
        {
            get { return mAvaliable; }
        }

        #endregion

        #region Events

        public event OnQueueCircularDynamicSizeChangedDelegate OnQueueCircularDynamicSizeChanged;

        #endregion

        #region Fields

        private T[] mItemsArray;

        private int mHead = 0;
        private int mTail = 0;
        private int mAvaliable = 0;

        #endregion

        public delegate void OnQueueCircularDynamicSizeChangedDelegate(int newSize);
    }
}