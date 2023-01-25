using System;
using System.Collections.Generic;

namespace AnignoraDataTypes
{
    public enum DataAddedUpdatedEnum
    {
        None,
        Added,
        Replaced
    }

    public static class DataTypesExtensions
    {
        #region Public Methods

        public static void DoForAll<T>(this IEnumerable<T> p_sequence, Action<T> p_action)
        {
            foreach (T item in p_sequence)
            {
                p_action(item);
            }
        }

        /// <summary> Returns the next element and increasing the given ref counter</summary>
        //public static T NextElement<T>(this IEnumerable<T> p_enumerable, ref int p_cnt)
        //{
        //    if (p_cnt < 0) p_cnt = 0;
        //    if (p_cnt >= p_enumerable.Count()) p_cnt = 0;
        //    return p_enumerable.ElementAt(p_cnt++);
        //}
        /// <summary> Verifies before trying to add that the key doesn't already exist. returns true if element was added, else return false </summary>
        public static bool AddDistinct<TKey, TValue>(this IDictionary<TKey, TValue> p_dictionary, TKey p_key, TValue p_value)
        {
            if (p_dictionary.ContainsKey(p_key)) return false;
            p_dictionary.Add(p_key, p_value);
            return true;
        }

        /// <summary> Replace if key exists or add new if not </summary>
        public static DataAddedUpdatedEnum AddReplace<TKey, TValue>(this IDictionary<TKey, TValue> p_dictionary, TKey p_key, TValue p_value)
        {
            if (p_dictionary.ContainsKey(p_key))
            {
                p_dictionary[p_key] = p_value;
                return DataAddedUpdatedEnum.Replaced;
            }
            p_dictionary.Add(p_key, p_value);
            return DataAddedUpdatedEnum.Added;
        }

        public static DataAddedUpdatedEnum AddReplace<T>(this List<T> p_list, T p_item, Func<T, T, bool> p_isEqual)
        {
            for (int index = 0; index < p_list.Count; index++)
            {
                T item = p_list[index];
                if (p_isEqual(item, p_item))
                {
                    p_list[index] = p_item;
                    return DataAddedUpdatedEnum.Replaced;
                }
            }
            p_list.Add(p_item);
            return DataAddedUpdatedEnum.Added;
        }

        public static bool IsExist<T>(this List<T> p_list, T p_item, Func<T, T, bool> p_isEqual)
        {
            foreach (T item in p_list)
            {
                if (p_isEqual(item, p_item)) return true;
            }
            return false;
        }

        public static TType GetItem<TType, TKey>(this List<TType> p_list, TKey p_key, Func<TType, TKey, bool> p_isExist)
        {
            foreach (TType item in p_list)
            {
                if (p_isExist(item, p_key)) return item;
            }
            return default(TType);
        }

        /// <summary>
        /// Set the date values to min (1,1,1), leaving time values as they were
        /// </summary>
        /// <param name="p_dateTime"></param>
        /// <returns></returns>
        public static DateTime Time(this DateTime p_dateTime)
        {
            return new DateTime(1, 1, 1, p_dateTime.Hour, p_dateTime.Minute, p_dateTime.Second, p_dateTime.Millisecond);
        }

        #endregion
    }
}