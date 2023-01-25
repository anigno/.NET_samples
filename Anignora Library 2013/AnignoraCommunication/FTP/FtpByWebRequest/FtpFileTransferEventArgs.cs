using System;

namespace AnignoraCommunication.FTP.FtpByWebRequest
{
    public class FtpFileTransferEventArgs : EventArgs
    {
        public string LocalFile { get; private set; }
        public string RemoteFile { get; private set; }
        public long BytesTransfered { get; private set; }
        public long FileSize { get; private set; }
        public bool IsSucceeded { get; private set; }
        public string ErrorMessage { get; set; }

        public FtpFileTransferEventArgs(bool p_isSucceeded,string p_localFile, string p_remoteFile, long p_bytesTransfered, long p_fileSize)
        {
            IsSucceeded = p_isSucceeded;
            LocalFile = p_localFile;
            RemoteFile = p_remoteFile;
            BytesTransfered = p_bytesTransfered;
            FileSize = p_fileSize;
        }

        public override string ToString()
        {
            return string.Format("******I:{0} L:{1} R:{2} T:{3} S:{4} ",IsSucceeded, LocalFile, RemoteFile, BytesTransfered, FileSize);
        }
    }
}