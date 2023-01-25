using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using AnignoraCommunication.FTP.FtpByWebRequest.FtpResults;

namespace AnignoraCommunication.FTP.FtpByWebRequest
{
    public class FtpWebRequestClient:IFtpClient
    {
		#region (------  Constants  ------)
        public const string FTP_PREFIX = "ftp://";
		#endregion (------  Constants  ------)

		#region (------  Events  ------)
        public event EventHandler<FtpFileTransferEventArgs> FtpFileDownloadEnded = delegate { };

        public event EventHandler<FtpFileTransferEventArgs> FtpFileDownloading = delegate { };

        public event EventHandler<FtpFileTransferEventArgs> FtpFileUploaded = delegate { };

        public event EventHandler<FtpFileTransferEventArgs> FtpFileUploadEnded = delegate { };
		#endregion (------  Events  ------)

		#region (------  Constructors  ------)
        public FtpWebRequestClient(string p_ftpServer, string p_username, string p_password,int p_requestTimeoutMs=20000, int p_transferBufferLength = 2048)
        {
            BuffLength = p_transferBufferLength;
            FtpServerlp = Dns.GetHostAddresses(p_ftpServer).FirstOrDefault();
            FtpUserld = p_username;
            FtpPassword = p_password;
            RequestTimeoutMs = p_requestTimeoutMs;
        }
		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)
        public int BuffLength { get; set; }

        public string FtpPassword { get; private set; }

        public IPAddress FtpServerlp { get; private set; }

        public string FtpUserld { get; private set; }

        public int RequestTimeoutMs { get; private set; }
		#endregion (------  Properties  ------)

		#region (------  Public Methods  ------)
        public FtpResult CheckConnection()
        {
            try
            {
                string uniqueFileName = Guid.NewGuid().ToString();
                long continueFlag = 1;
                MemoryStream memoryStream = new MemoryStream(new byte[] {1, 2, 3, 4});
                FtpResult result = UploadStream(memoryStream, uniqueFileName, ref continueFlag);
                if (!result.IsSuccessfull) return result;
                memoryStream = new MemoryStream();
                result = DownloadStream(uniqueFileName, memoryStream, ref continueFlag);
                if (!result.IsSuccessfull) return result;
                result = DeleteFile(uniqueFileName);
                return result;
            }
            catch (Exception ex)
            {
                return new FtpResult(false, ex);
            }
        }

        public FtpResult CreateDirectory(string p_remoteDirectory)
        {
            try
            {
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(FTP_PREFIX + FtpServerlp + "/" + p_remoteDirectory));
                reqFtp.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(FtpUserld, FtpPassword);
                reqFtp.Timeout = RequestTimeoutMs;
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                if (ftpStream != null) ftpStream.Close();
                response.Close();
                return new FtpResult(true);
            }
            catch (Exception ex)
            {
                return new FtpResult(false,ex);
            }
        }

        public FtpResult DeleteDirectory(string p_remoteDirectory)
        {
            try
            {
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(FTP_PREFIX + FtpServerlp + "/" + p_remoteDirectory));
                reqFtp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(FtpUserld, FtpPassword);
                reqFtp.Timeout = RequestTimeoutMs;
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                if (ftpStream != null) ftpStream.Close();
                response.Close();
                return new FtpResult(true);
            }
            catch (Exception ex)
            {
                return new FtpResult(false,ex);
            }
        }

        public virtual FtpResult DeleteFile(string p_remoteFile)
        {
            try
            {
                string dir = Path.GetDirectoryName(p_remoteFile);
                string remoteFileName = Path.GetFileName(p_remoteFile);
                Uri requestUriString = new Uri(FTP_PREFIX + FtpServerlp + "/" + dir + "/" + remoteFileName);
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(requestUriString);
                reqFtp.Credentials = new NetworkCredential(FtpUserld,FtpPassword);
                reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFtp.Timeout = RequestTimeoutMs;
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                response.Close();
                return new FtpResult(true);
            }
            catch (Exception ex)
            {
                return new FtpResult(false,ex);
            }
        }

        public virtual FtpResult DownloadFile(string p_remoteFile, string p_localFile, ref long p_continueFlag)
        {
            long readTotal = 0;
            FtpResultLong fileSizeResult = GetFileSize(p_remoteFile);
            if (!fileSizeResult.IsSuccessfull) return fileSizeResult;
            try
            {
                FileStream outputStream = new FileStream(p_localFile, FileMode.Create);
                string uriString = "ftp://" + FtpServerlp + "/" + p_remoteFile;
                Uri requestUriString = new Uri(uriString);
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(requestUriString);
                reqFtp.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFtp.UseBinary = true;
                reqFtp.KeepAlive = false;
                reqFtp.Credentials = new NetworkCredential(FtpUserld, FtpPassword);
                reqFtp.Timeout = RequestTimeoutMs;
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                if (ftpStream == null) throw new NullReferenceException("Couldn't get a response stream");
                //long contentLength = response.ContentLength;
                byte[] buffer = new byte[BuffLength];
                int readCount = ftpStream.Read(buffer, 0, BuffLength);
                readTotal = readCount;
                while (readCount > 0 && Interlocked.Read(ref p_continueFlag) == 1)
                {
                    FtpFileDownloading(this, new FtpFileTransferEventArgs(true,p_localFile, p_remoteFile, readTotal, fileSizeResult.Value));
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, BuffLength);
                    readTotal += readCount;
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                FtpFileDownloadEnded(this, new FtpFileTransferEventArgs(true,p_localFile, p_remoteFile, readTotal, fileSizeResult.Value));
                return new FtpResult(true);
            }
            catch(Exception ex)
            {
                FtpFileDownloadEnded(this, new FtpFileTransferEventArgs(false, p_localFile, p_remoteFile, readTotal, fileSizeResult.Value));
                return new FtpResult(false,ex);
            }
        }

        public FtpResult DownloadStream(string p_remoteFile, Stream p_outputStream, ref long p_continueFlag)
        {
            long readTotal = 0;
            long fileSize = -1;
            FtpResultLong fileSizeResult = GetFileSize(p_remoteFile);
            if (!fileSizeResult.IsSuccessfull) return fileSizeResult;
            try
            {
                string uriString = "ftp://" + FtpServerlp + "/" + p_remoteFile;
                Uri requestUriString = new Uri(uriString);
                FtpWebRequest reqFtp =
                    (FtpWebRequest)WebRequest.Create(requestUriString);
                reqFtp.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFtp.UseBinary = true;
                reqFtp.KeepAlive = false;
                reqFtp.Credentials = new NetworkCredential(FtpUserld, FtpPassword);
                reqFtp.Timeout = RequestTimeoutMs;
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                if (ftpStream == null) throw new NullReferenceException("Couldn't get a response stream");
                //long contentLength = response.ContentLength;
                byte[] buffer = new byte[BuffLength];
                int readCount = ftpStream.Read(buffer, 0, BuffLength);
                readTotal = readCount;
                while (readCount > 0 && Interlocked.Read(ref p_continueFlag) == 1)
                {
                    FtpFileDownloading(this, new FtpFileTransferEventArgs(true,"DownloadStream", p_remoteFile, readTotal, fileSizeResult.Value));
                    p_outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, BuffLength);
                    readTotal += readCount;
                }
                ftpStream.Close();
                response.Close();
                fileSize = p_outputStream.Length;
                p_outputStream.Position = 0;
                FtpFileDownloadEnded(this, new FtpFileTransferEventArgs(true,"Downloadstream", p_remoteFile, readTotal, fileSize));
                return new FtpResult(true);
            }
            catch(Exception ex)
            {
                FtpFileDownloadEnded(this, new FtpFileTransferEventArgs(false, "Downloadstream", p_remoteFile, readTotal, fileSize));
                return new FtpResult(false, ex);
            }
        }

        public FtpResultStringArray GetDirectories(string p_remoteDirectory)
        {
            try
            {
                FtpResultStringArray filesAndDirsDetailsResult = GetFilesDetails(p_remoteDirectory, WebRequestListDirectoryEnum.ListDirectoryDetails);
                if (!filesAndDirsDetailsResult.IsSuccessfull) return filesAndDirsDetailsResult;

                FtpResultStringArray filesAndDirsResult = GetFilesDetails(p_remoteDirectory,WebRequestListDirectoryEnum.ListDirectory);
                if (!filesAndDirsResult.IsSuccessfull) return filesAndDirsResult;

                string[] dirs = filesAndDirsResult.Array.Where(
                    (p_item, p_index) =>
                    {
                        string filesAndDirsDetail = filesAndDirsDetailsResult.Array[p_index];
                        return isDirectoryGenericCheck(filesAndDirsDetail) || isDirectoryGoDaddyCheck(filesAndDirsDetail);
                    }).ToArray();
                return new FtpResultStringArray(true,dirs);
            }
            catch (Exception ex)
            {
                return new FtpResultStringArray(false,null,ex);
            }
        }

        public FtpResultStringArray GetFiles(string p_remoteDirectory)
        {
            try
            {
                FtpResultStringArray filesAndDirsResult = GetFilesDetails(p_remoteDirectory, WebRequestListDirectoryEnum.ListDirectory);
                if (!filesAndDirsResult.IsSuccessfull) return filesAndDirsResult;

                FtpResultStringArray dirsResult = GetDirectories(p_remoteDirectory);
                if (!dirsResult.IsSuccessfull) return dirsResult;
                string[] files = filesAndDirsResult.Array.Except(dirsResult.Array).ToArray();
                return new FtpResultStringArray(true,files);
            }
            catch (Exception ex)
            {
                return new FtpResultStringArray(false,null,ex);
            }
        }

        public FtpResultStringArray GetFilesDetails(string p_remoteDirectory, WebRequestListDirectoryEnum p_ftpWebRequestMethod)
        {
            try
            {
                Uri requestUriString = new Uri(FTP_PREFIX + FtpServerlp + "/" + p_remoteDirectory);
                FtpWebRequest reqFtp =(FtpWebRequest)WebRequest.Create(requestUriString);
                reqFtp.Credentials = new NetworkCredential(FtpUserld, FtpPassword);
                reqFtp.UseBinary = true;
                reqFtp.KeepAlive = false;

                reqFtp.Method = p_ftpWebRequestMethod == WebRequestListDirectoryEnum.ListDirectoryDetails ? WebRequestMethods.Ftp.ListDirectoryDetails : WebRequestMethods.Ftp.ListDirectory;
                reqFtp.Timeout = RequestTimeoutMs;
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                if(ftpStream==null)throw new NullReferenceException("Couldn't get a response stream");
                StreamReader reader = new StreamReader(ftpStream);
                string readToEnd = reader.ReadToEnd();
                string[] linesSeparator = new[] { Environment.NewLine };
                string[] lines = readToEnd.Split(linesSeparator,StringSplitOptions.RemoveEmptyEntries);
                response.Close();
                return new FtpResultStringArray(true,lines); 
            }
            catch (Exception ex)
            {
                return new FtpResultStringArray(false,null,ex);
            }
        }

        public FtpResultLong GetFileSize(string p_remoteFile)
        {
            try
            {
                string dir = Path.GetDirectoryName(p_remoteFile);
                string remoteFileName = Path.GetFileName(p_remoteFile);
                Uri requestUriString = new Uri(FTP_PREFIX + FtpServerlp + "/" + dir + "/" + remoteFileName);
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(requestUriString);
                reqFtp.Credentials = new NetworkCredential(FtpUserld, FtpPassword);
                reqFtp.UseBinary = true;
                reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long fileSize = response.ContentLength;
                if (ftpStream != null) ftpStream.Close();
                response.Close();
                return new FtpResultLong(true, fileSize);
            }
            catch (Exception ex)
            {
                return new FtpResultLong(false,-1,ex);
            }
        }

        public FtpResult IsDirExist(string p_remoteDirectory)
        {
            try
            {
                string parentDir = Path.GetDirectoryName(p_remoteDirectory);
                //string dir = Path.GetFileNameWithoutExtension(p_remoteDirectory);

                FtpResult isDirExist = IsDirExist(parentDir);
                if (parentDir != "" && !isDirExist.IsSuccessfull) return new FtpResult(false);

                FtpResultStringArray dirs = GetDirectories(parentDir);
                bool dirExist = dirs.Array.Contains(p_remoteDirectory);
                return new FtpResult(dirExist);
            }
            catch (Exception ex)
            {
                return new FtpResult(false,ex);
            }
        }

        public FtpResult IsDirExistOld(string p_remoteDirectory)
        {
            try
            {
                Uri requestUriString = new Uri(FTP_PREFIX + FtpServerlp + "/" + p_remoteDirectory);
                FtpWebRequest reqFtp = (FtpWebRequest)
                                       WebRequest.Create(requestUriString);
                reqFtp.Credentials = new NetworkCredential(FtpUserld, FtpPassword);
                reqFtp.UseBinary = true;
                reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                if (ftpStream != null) ftpStream.Close();
                response.Close();
                return new FtpResult(true);
            }
            catch (Exception ex)
            {
                return new FtpResult(false,ex);
            }
        }

        public FtpResult IsFileExist(string p_remoteFile)
        {
            FtpResultLong fileSizeResult = GetFileSize(p_remoteFile);
            if (!fileSizeResult.IsSuccessfull) return fileSizeResult;
            bool isFileExist = fileSizeResult.Value >= 0;
            return new FtpResult(isFileExist);
        }

        public virtual FtpResult UploadFile(string p_localFile, string p_remoteFile, ref long p_continueFlag)
        {
            long readTotal = 0;
            long fileSize = -1;
            try
            {
                string dir = Path.GetDirectoryName(p_remoteFile);
                string remoteFileName = Path.GetFileName(p_remoteFile);
                FileInfo filelnf = new FileInfo(p_localFile);
                fileSize = filelnf.Length;
                string uriString = FTP_PREFIX + FtpServerlp + "/" + dir + "/" + remoteFileName;
                Uri requestUriString = new Uri(uriString);
                FtpWebRequest reqFtp = (FtpWebRequest)
                                       WebRequest.Create(requestUriString);
                reqFtp.Credentials = new NetworkCredential(FtpUserld,
                                                           FtpPassword);
                reqFtp.KeepAlive = false;
                //if (dir != "") CreateDirectory(dir);
                reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
                reqFtp.UseBinary = true;
                reqFtp.ContentLength = filelnf.Length;
                byte[] buff = new byte[BuffLength];
                FileStream fileStream = filelnf.OpenRead();
                Stream remoteStream = reqFtp.GetRequestStream();
                int readCount = fileStream.Read(buff, 0, BuffLength);
                readTotal = readCount;
                while (readCount != 0 && Interlocked.Read(ref p_continueFlag) == 1)
                {
                    FtpFileUploadEnded(this, new FtpFileTransferEventArgs(true,p_localFile, p_remoteFile, readTotal, fileSize));
                    remoteStream.Write(buff, 0, readCount);
                    readCount = fileStream.Read(buff, 0, BuffLength);
                    readTotal += readCount;
                }
                remoteStream.Close();
                fileStream.Close();
                FtpFileUploaded(this, new FtpFileTransferEventArgs(true,p_localFile, p_remoteFile, readTotal, fileSize));
                return new FtpResult(true);
            }
            catch(Exception ex)
            {
                FtpFileUploaded(this, new FtpFileTransferEventArgs(false, p_localFile, p_remoteFile, readTotal, fileSize));
                return new FtpResult(false, ex);
            }
        }

        public FtpResult UploadStream(Stream p_localStream, string p_remoteFile, ref long p_continueFlag)
        {
            long readTotal = 0;
            long fileSize = -1;
            try
            {
                string dir = Path.GetDirectoryName(p_remoteFile);
                string remoteFileName = Path.GetFileName(p_remoteFile);
                string uriString = FTP_PREFIX + FtpServerlp + "/" + dir + "/" + remoteFileName;
                Uri requestUriString = new Uri(uriString);
                FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(requestUriString);
                reqFtp.Credentials = new NetworkCredential(FtpUserld, FtpPassword);
                reqFtp.KeepAlive = false;
                //if (dir != "") CreateDirectory(dir);
                reqFtp.Method = WebRequestMethods.Ftp.UploadFile;
                reqFtp.UseBinary = true;
                byte[] buff = new byte[BuffLength];
                fileSize = p_localStream.Length;
                p_localStream.Position = 0;
                reqFtp.Timeout = RequestTimeoutMs;
                Stream remoteStream = reqFtp.GetRequestStream();
                int readCount = p_localStream.Read(buff, 0, BuffLength);
                readTotal = readCount;
                while (readCount != 0 && Interlocked.Read(ref p_continueFlag) == 1)
                {
                    FtpFileUploadEnded(this, new FtpFileTransferEventArgs(true,"UploadStream", p_remoteFile, readTotal, fileSize));
                    remoteStream.Write(buff, 0, readCount);
                    readCount = p_localStream.Read(buff, 0, BuffLength);
                    readTotal += readCount;
                }
                remoteStream.Close();
                p_localStream.Close();
                FtpFileUploaded(this, new FtpFileTransferEventArgs(true,"UploadStream", p_remoteFile, readTotal, fileSize));
                return new FtpResult(true);
            }
            catch(Exception ex)
            {
                FtpFileUploaded(this, new FtpFileTransferEventArgs(false, "UploadStream", p_remoteFile, readTotal, fileSize) { ErrorMessage = ex.Message});
                return new FtpResult(false, ex);
            }
        }
		#endregion (------  Public Methods  ------)

		#region (------  Protected Methods  ------)
        protected void RaiseFtpFileDownloaded(FtpFileTransferEventArgs p_fileTransferEventArgs)
        {
            FtpFileDownloadEnded(this, p_fileTransferEventArgs);
        }

        protected void RaiseFtpFileDownloading(FtpFileTransferEventArgs p_fileTransferEventArgs)
        {
            FtpFileDownloading(this, p_fileTransferEventArgs);
        }

        protected void RaiseFtpFileUploaded(FtpFileTransferEventArgs p_fileTransferEventArgs)
        {
            FtpFileUploaded(this, p_fileTransferEventArgs);
        }

        protected void RaiseFtpFileUploading(FtpFileTransferEventArgs p_fileTransferEventArgs)
        {
            FtpFileUploadEnded(this, p_fileTransferEventArgs);
        }
		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)
        private bool isDirectoryGenericCheck(string p_item)
        {
            return p_item.Substring(0, 1).ToLower() == "d";
        }

        private bool isDirectoryGoDaddyCheck(string p_item)
        {
            return p_item.ToUpper().Contains("<DIR>");
        }
		#endregion (------  Private Methods  ------)
    }
}