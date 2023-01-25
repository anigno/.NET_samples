using System;
using System.IO;

namespace AnignoraIO.NetworkShares
{
    /// <summary>
    /// Information about a local share
    /// </summary>
    public class ShareItem
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ShareItem(string server, string netName, string path, ShareTypeEnum shareType, string remark)
        {
            if (ShareTypeEnum.Special == shareType && "IPC$" == netName)
            {
                shareType |= ShareTypeEnum.IPC;
            }

            _server = server;
            _netName = netName;
            _path = path;
            _shareType = shareType;
            _remark = remark;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the path to this share
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_server))
            {
                return string.Format(@"\\{0}\{1}", Environment.MachineName, _netName);
            }
            return string.Format(@"\\{0}\{1}", _server, _netName);
        }

        /// <summary>
        /// Returns true if this share matches the local path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool MatchesPath(string path)
        {
            if (!IsFileSystem) return false;
            if (string.IsNullOrEmpty(path)) return true;

            return path.ToLower().StartsWith(_path.ToLower());
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the computer that this share belongs to
        /// </summary>
        public string Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Share name
        /// </summary>
        public string NetName
        {
            get { return _netName; }
        }

        /// <summary>
        /// Local path
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Share type
        /// </summary>
        public ShareTypeEnum ShareType
        {
            get { return _shareType; }
        }

        /// <summary>
        /// Comment
        /// </summary>
        public string Remark
        {
            get { return _remark; }
        }

        /// <summary>
        /// Returns true if this is a file system share
        /// </summary>
        public bool IsFileSystem
        {
            get
            {
                // Shared device
                if (0 != (_shareType & ShareTypeEnum.Device)) return false;
                // IPC share
                if (0 != (_shareType & ShareTypeEnum.IPC)) return false;
                // Shared printer
                if (0 != (_shareType & ShareTypeEnum.Printer)) return false;

                // Standard disk share
                if (0 == (_shareType & ShareTypeEnum.Special)) return true;

                // Special disk share (e.g. C$)
                if (ShareTypeEnum.Special == _shareType && !string.IsNullOrEmpty(_netName))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Get the root of a disk-based share
        /// </summary>
        public DirectoryInfo Root
        {
            get
            {
                if (IsFileSystem)
                {
                    if (string.IsNullOrEmpty(_server))
                        if (string.IsNullOrEmpty(_path))
                            return new DirectoryInfo(ToString());
                        else
                            return new DirectoryInfo(_path);
                    return new DirectoryInfo(ToString());
                }
                return null;
            }
        }

        #endregion

        #region Fields

        private readonly string _server;
        private readonly string _netName;
        private readonly string _path;
        private readonly ShareTypeEnum _shareType;
        private readonly string _remark;

        #endregion
    }
}