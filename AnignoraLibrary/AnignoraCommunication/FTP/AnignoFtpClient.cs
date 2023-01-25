using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace AnignoraCommunication.FTP
{
    [Obsolete]
    public class AnignoFtpClient
    {
		#region (------  Fields  ------)

        private const string FTPSTR = @"ftp://";
        private readonly string m_password;
        private readonly string m_serverName;
        private readonly string m_username;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public AnignoFtpClient(string p_serverName, string p_username, string p_password )
        {
            m_serverName = p_serverName;
            m_username = p_username;
            m_password = p_password;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        public Exception LastError { get; private set; }

		#endregion (------  Properties  ------)

		#region (------  Public Methods  ------)

        public bool DownloadFile(string p_localFilename, string p_ftpFilename)
        {
            try
            {
                string uriString = FTPSTR + m_serverName + @"/" + p_ftpFilename;
                Uri uri = new Uri(uriString);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(m_username, m_password);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                TextWriter tw = new StreamWriter(p_localFilename, false);
                tw.Write(reader.ReadToEnd());
                tw.Flush();
                reader.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                LastError = ex;
                return false;
            }
        }

        public string[] ListDirectory()
        {
            List<string> dirs = new List<string>();
            try
            {
                string uriString = FTPSTR + m_serverName + @"/" ;
                Uri uri = new Uri(uriString);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(m_username, m_password);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                while (reader.Peek() > 0)
                {
                    dirs.Add(reader.ReadLine());
                }
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                LastError = ex;
            }
            return dirs.ToArray();
        }

        public void ResetLastError()
        {
            LastError = null;
        }

        public bool UploadFile( string p_localFilename, string p_ftpFilename)
        {
            try
            {
                StreamReader sourceStream = new StreamReader(p_localFilename);
                return UploadStream( sourceStream, p_ftpFilename);
            }
            catch (Exception ex)
            {
                LastError = ex;
                return false;
            }
        }

        public bool UploadStream( StreamReader p_sourceStream, string p_ftpFilename)
        {
            try
            {
                string uriString = FTPSTR + m_serverName + @"/" + p_ftpFilename;
                Uri uri = new Uri(uriString);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(m_username, m_password);
                byte[] fileContents = Encoding.UTF8.GetBytes(p_sourceStream.ReadToEnd());
                p_sourceStream.Close();
                request.ContentLength = fileContents.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                LastError = ex;
                return false;
            }
        }

        public bool UploadString(string p_sourceString, string p_ftpFilename)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(p_sourceString);
                MemoryStream ms = new MemoryStream(bytes);
                StreamReader sr = new StreamReader(ms);
                return UploadStream( sr, p_ftpFilename);
            }
            catch (Exception ex)
            {
                LastError = ex;
                return false;
            }
        }

		#endregion (------  Public Methods  ------)
    }
}