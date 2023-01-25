using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace AnignoraCommonAndHelpers.Helpers
{
    public class SerializationHelper
    {
        #region Public Methods

        /// <summary>
        /// Serialize an object using binary formatter
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <returns>True if succeded, else false</returns>
        public static bool BinarySerialize(Stream stream, object obj)
        {
            bool succeeded = true;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
            }
            catch
            {
                succeeded = false;
            }
            finally
            {
                stream.Close();
            }
            return succeeded;
        }

        /// <summary>
        /// Deserialize an object using binary formatter
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>True if succeded, else false</returns>
        public static object BinaryDeserialize(Stream stream)
        {
            object ret;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                ret = formatter.Deserialize(stream);
            }
            catch
            {
                ret = null;
            }
            finally
            {
                stream.Close();
            }
            return ret;
        }

        /// <summary>
        /// Read class data using XmlSerializer
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object ReadConfiguration(string fileName, Type t)
        {
            object obj = null;
            lock (_XmlSerializerSyncObject)
            {
                FileStream fs = null;
                try
                {
                    XmlSerializer xs = new XmlSerializer(t);
                    fs = new FileStream(fileName, FileMode.Open);
                    obj = xs.Deserialize(fs);
                }
                catch
                {
                }
                finally
                {
                    if (fs != null) fs.Close();
                }
            }
            return obj;
        }

        /// <summary>
        /// Write class data using XmlSerializer
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="obj"></param>
        public static void WriteConfiguration(string fileName, object obj)
        {
            lock (_XmlSerializerSyncObject)
            {
                FileStream fs = null;
                try
                {
                    Type t = obj.GetType();
                    XmlSerializer xs = new XmlSerializer(t);
                    fs = new FileStream(fileName, FileMode.Create);
                    xs.Serialize(fs, obj);
                }
                catch
                {
                }
                finally
                {
                    if (fs != null) fs.Close();
                }
            }
        }

        public static bool DataContractSerialize(string filename, object obj, params Type[] knownTypes)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create);
                DataContractSerializer dsc = new DataContractSerializer(obj.GetType(), knownTypes);
                dsc.WriteObject(fs, obj);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        public static object DataContractDeserialize(string filename, object obj, params Type[] knownTypes)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                DataContractSerializer dsc = new DataContractSerializer(obj.GetType(), knownTypes);
                return dsc.ReadObject(fs);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        #endregion

        #region Fields

        private static readonly object _XmlSerializerSyncObject = new object();

        #endregion

        /*
        Usage:
        [DataContract(Name = "MainData", Namespace = "http://www.anignora.com")]
        public class MyClass
        {
            [DataMember(Name = "MyDirectoryName")]
            public Dictionary<int, string> MyDictionary = new Dictionary<int, string>();
        }
        */
    }
}