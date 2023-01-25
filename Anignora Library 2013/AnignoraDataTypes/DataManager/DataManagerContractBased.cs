using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AnignoraDataTypes.DataManager
{
    /// <summary>
    /// Serialization data manager. Automatically loads and saves data, item delivered as template.
    /// Thread safe
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class DataManagerContractBased<T> where T : class, new()
    {
        #region Constructors

        static DataManagerContractBased()
        {
            lock (_syncRoot)
            {
                _dataFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + DEFAULT_DATA_FILE_PARTIAL_PATH;
                _dataFileName = typeof (T) + ".DataManagerContractBased.XML";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Force DataItem read from file, used to create a starting point at user's application for
        /// first data item access (Eg. Sorting)
        /// </summary>
        public static void ForceDataItemRead()
        {
            lock (_syncRoot)
            {
                T temp = DataItem;
            }
        }

        /// <summary>
        /// Save current data to file
        /// </summary>
        public static void UpdateSavedData()
        {
            lock (_syncRoot)
            {
                SaveDataItem();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the data item class managed by this DataManager,
        /// only one DataManager exists per data item
        /// </summary>
        public static T DataItem
        {
            get
            {
                lock (_syncRoot)
                {
                    lock (_syncRoot)
                    {
                        if (_dataItem == null)
                        {
                            LoadDataItem();
                        }
                        if (_dataItem == null) throw new NullReferenceException("dat item is null");
                        return _dataItem;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the data file's full path, if the path doesn't exists,
        /// it will be created on first usage. Path must end with path delimitter.
        /// (value must be set before any use of data item!)
        /// </summary>
        public static string DataFilePath
        {
            get
            {
                lock (_syncRoot)
                {
                    return _dataFilePath;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _dataFilePath = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the data file's name, if the path doesn't exists,
        /// it will be created on first usage
        /// (value must be set before any use of data item!)
        /// </summary>
        public static string DataFileName
        {
            get
            {
                lock (_syncRoot)
                {
                    return _dataFileName;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _dataFileName = value;
                }
            }
        }

        public static string LastError
        {
            get { return _lastError; }
            set { _lastError = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after first succesfull load
        /// </summary>
        public static event AfterFirstLoadEventHandler OnAfterFirstLoad;

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the data item from DataFilePath and DataFileName, if path and file doesn't exists,
        /// They will be created.
        /// </summary>
        /// <returns>string.Empty if succeeded, else return the exception message</returns>
        private static void LoadDataItem()
        {
            lock (_syncRoot)
            {
                if (File.Exists(_dataFilePath + @"\" + _dataFileName))
                {
                    DataContractSerializer dcs = new DataContractSerializer(typeof (T));
                    FileStream fs = null;
                    try
                    {
                        fs = new FileStream(_dataFilePath + @"\" + _dataFileName, FileMode.Open);
                        _dataItem = (T) dcs.ReadObject(fs);
                        LastError = "";
                        if (OnAfterFirstLoad != null) OnAfterFirstLoad(typeof (T), _dataItem);
                    }
                    catch (Exception ex)
                    {
                        LastError = ex.Message;
                        return;
                    }
                    finally
                    {
                        if (fs != null) fs.Close();
                    }
                    return;
                }
                SaveDataItem();
            }
        }

        /// <summary>
        /// Saves the data item to DataFilePath and DataFileName, if path and file doesn't exists,
        /// They will be created.
        /// </summary>
        /// <returns>string.Empty is succeeded, else return the exception message</returns>
        private static void SaveDataItem()
        {
            lock (_syncRoot)
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof (T));
                FileStream fs = null;
                try
                {
                    string path = Path.GetDirectoryName(_dataFilePath + @"\" + _dataFileName);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Create backup of previous file
                    if (File.Exists(_dataFilePath + @"\" + _dataFileName))
                    {
                        File.Copy(_dataFilePath + @"\" + _dataFileName, _dataFilePath + @"\" + _dataFileName + ".bck", true);
                    }
                    fs = new FileStream(_dataFilePath + @"\" + _dataFileName, FileMode.Create);
                    if (_dataItem == null) _dataItem = new T();
                    dcs.WriteObject(fs, _dataItem);
                    LastError = "";
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    return;
                }
                finally
                {
                    if (fs != null) fs.Close();
                }
            }
        }

        #endregion

        #region Fields

        private static string _dataFileName;
        private static string _dataFilePath;
        private static T _dataItem;
        private static string _lastError;
        private static StaticDestructor _staticDestructor = new StaticDestructor();
        private static readonly List<T> _syncRoot = new List<T>();

        #endregion

        #region Constants

        private const string DEFAULT_DATA_FILE_PARTIAL_PATH = "\\_Anigno Documents\\_ApplicationsData\\";

        #endregion

        public delegate void AfterFirstLoadEventHandler(Type type, T dataItem);

        private class StaticDestructor
        {
            ~StaticDestructor()
            {
                SaveDataItem();
            }
        }
    }
}