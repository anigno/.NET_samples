#region

using System;
using System.Collections;
using System.Reflection;

#endregion

namespace PropertiesGetSetValues
{
    public static class PropertyReflection
    {
        #region Public Methods

        public static void CopyProperties(object p_source, object p_dest)
        {
            Type typeSource = p_source.GetType();
            Type typeDest = p_dest.GetType();
            PropertyInfo[] sourceProperties = typeSource.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in sourceProperties)
            {
                PropertyInfo valuePropertyInSource = typeSource.GetProperty(propertyInfo.Name, BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo valuePropertyInDest = typeDest.GetProperty(propertyInfo.Name, BindingFlags.Public | BindingFlags.Instance);
                Type propertyType = valuePropertyInSource.PropertyType;
                if (propertyType.IsPrimitive)
                {
                    valuePropertyInDest.SetValue(p_dest, valuePropertyInSource.GetValue(p_source));
                }
                if (typeof (IEnumerable).IsAssignableFrom(propertyType))
                {
                }
            }
        }


        /// <summary>
        /// Copy value properties to other value properties with same structure
        /// </summary>
        public static void CopyValues(object p_source, object p_dest)
        {
            Type typeSource = p_source.GetType();
            Type typeDest = p_dest.GetType();
            PropertyInfo valuePropertyInSource = typeSource.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo valuePropertyInDest = typeDest.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            if (valuePropertyInSource != null)
            {
                //Console.WriteLine("_" + typeSource.Name + " Value");
                var sourceValue = valuePropertyInSource.GetValue(p_source);
                valuePropertyInDest.SetValue(p_dest, sourceValue);
                return;
            }
            if (typeSource.IsArray)
            {
                object[] arraySource = (object[]) p_source;
                object[] arrayDest = (object[]) p_dest;
                for (int i = 0; i < arraySource.Length; i++)
                {
                    CopyValues(arraySource[i], arrayDest[i]);
                }
                return;
            }
            PropertyInfo[] sourceProperties = typeSource.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo propertyInfo in sourceProperties)
            {
                var source = propertyInfo.GetValue(p_source);
                PropertyInfo from = typeDest.GetProperty(propertyInfo.Name);
                var dest = from.GetValue(p_dest);
                //Console.Write("-" + propertyInfo.Name);
                if (from.PropertyType.IsPrimitive)
                {
                    //Console.WriteLine(" Primitive");
                }
                else if (from.PropertyType.GetProperties().Length > 0)
                {
                    //Console.WriteLine(" Recurse");
                    CopyValues(source, dest);
                }
            }
        }

        #endregion
    }
}