using System;
using System.IO;
using System.Net;
using AnignoraCommunication.FTP.FtpByWebRequest;
using AnignoraCommunication.FTP.FtpByWebRequest.FtpResults;

namespace AnignoraCommunication.FTP
{
    public enum WebRequestListDirectoryEnum
    {
        ListDirectoryDetails,
        ListDirectory
    }
    public interface IFtpClient
    {
		#region (------  Operations  ------)

        FtpResult CheckConnection();

        FtpResult CreateDirectory(string p_remoteDirectory);

        FtpResult DeleteDirectory(string p_remoteDirectory);

        FtpResult DeleteFile(string p_remoteFile);

        FtpResult DownloadFile(string p_remoteFile, string p_localFile, ref long p_continueFlag);

        FtpResult DownloadStream(string p_remoteFile, Stream p_outputStream, ref long p_continueFlag);

        FtpResultStringArray GetDirectories(string p_remoteDirectory);

        FtpResultStringArray GetFiles(string p_remoteDirectory);

        FtpResultStringArray GetFilesDetails(string p_remoteDirectory, WebRequestListDirectoryEnum p_ftpWebRequestMethod);

        FtpResultLong GetFileSize(string p_remoteFile);

        FtpResult IsDirExist(string p_remoteDirectory);

        FtpResult IsFileExist(string p_remoteFile);

        FtpResult UploadFile(string p_localFile, string p_remoteFile, ref long p_continueFlag);

        FtpResult UploadStream(Stream p_localStream, string p_remoteFile, ref long p_continueFlag);

		#endregion (------  Operations  ------)

		#region (------  Data Members  ------)

        int BuffLength { get; set; }

        string FtpPassword { get; }

        IPAddress FtpServerlp { get; }

        string FtpUserld { get; }

		#endregion (------  Data Members  ------)
        event EventHandler<FtpFileTransferEventArgs> FtpFileDownloadEnded;
        event EventHandler<FtpFileTransferEventArgs> FtpFileDownloading;
        event EventHandler<FtpFileTransferEventArgs> FtpFileUploaded;
        event EventHandler<FtpFileTransferEventArgs> FtpFileUploadEnded;
    }
}