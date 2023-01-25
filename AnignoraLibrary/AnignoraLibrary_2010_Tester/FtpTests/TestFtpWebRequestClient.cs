using System;
using System.IO;
using System.Text;
using AnignoraCommonAndHelpers;
using AnignoraCommunication.FTP;
using AnignoraCommunication.FTP.FtpByWebRequest;
using AnignoraCommunication.FTP.FtpByWebRequest.FtpResults;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester.FtpTests
{
    [TestFixture]
    public class TestFtpWebReguestClient
    {

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            m_ftp = new FtpWebRequestClient(TEST_FTP_HOST_IP2, TEST_FTP_USERNAME2, TEST_FTP_PASSWORD2,4000);
            m_ftp.FtpFileDownloading += onFtpFileDownloading;
            m_ftp.FtpFileUploadEnded += onFtpFileUploadEnded;
            m_ftp.FtpFileDownloadEnded += onFtpFileDownloadEnded;
            m_ftp.FtpFileUploaded += onFtpFileUploaded;
        }


        [Test]
        public void TestConnection()
        {
            IFtpClient ftpClient = new FtpWebRequestClient(TEST_FTP_HOST_IP2, TEST_FTP_USERNAME2, TEST_FTP_PASSWORD2, 4000);
            FtpResult result = ftpClient.CheckConnection();
            Console.WriteLine(result);
            Assert.IsTrue(result.IsSuccessfull);
            
            ftpClient = new FtpWebRequestClient("", "", "");
            result = ftpClient.CheckConnection();
            Console.WriteLine(result);
            Assert.IsFalse(result.IsSuccessfull);
        }

        [Test]
        public void TestFolderCreationAndFileUpload()
        {
            const string DIR1 = "dir1";
            const string DIR1_DIR2 = "dir1/dir2";
            m_ftp.DeleteDirectory(DIR1_DIR2);
            m_ftp.DeleteDirectory(DIR1);
            FtpResult r1 = m_ftp.CreateDirectory(DIR1);
            FtpResult r2 = m_ftp.CreateDirectory(DIR1_DIR2);
            FtpResult r3 = m_ftp.IsDirExist(DIR1);
            FtpResult r4 = m_ftp.IsDirExist(DIR1_DIR2);
            Assert.IsTrue(r1.IsSuccessfull);
            Assert.IsTrue(r2.IsSuccessfull);
            Assert.IsTrue(r3.IsSuccessfull);
            Assert.IsTrue(r4.IsSuccessfull);
            FtpResult r5 = m_ftp.DeleteDirectory(DIR1_DIR2);
            FtpResult r6 = m_ftp.DeleteDirectory(DIR1);
            Assert.IsTrue(r5.IsSuccessfull);
            Assert.IsTrue(r6.IsSuccessfull);
        }

        [Test]
        public void TestFileUploadDownload()
        {
            string file1 = "File1.txt";
            string dir1 = "dir1";
            File.WriteAllText(file1,"1234");
            m_ftp.CreateDirectory(dir1);
            long continueFlag=1;
            FtpResult r1 = m_ftp.UploadFile(file1, file1, ref continueFlag);
            string dir1File1 = Path.Combine(dir1, file1);
            FtpResult r2 = m_ftp.UploadFile(file1, dir1File1, ref continueFlag);
            Assert.IsTrue(r1.IsSuccessfull,r1.ToString());
            Assert.IsTrue(r2.IsSuccessfull,r2.ToString());
            MemoryStream memoryStream=new MemoryStream(new byte[]{1,2,3,4});
            string file2="file2.txt";
            File.WriteAllText(file2, "1234");
            FtpResult r3 = m_ftp.UploadStream(memoryStream, file2, ref continueFlag);
            Assert.IsTrue(r3.IsSuccessfull);
            memoryStream = new MemoryStream();
            FtpResult r4 = m_ftp.DownloadStream(file2, memoryStream, ref continueFlag);
            Assert.IsTrue(r4.IsSuccessfull);
            FtpResult r5 = m_ftp.DownloadFile(dir1File1, file1, ref continueFlag);
            FtpResult r6 = m_ftp.DownloadFile(file1, file1, ref continueFlag);
            Assert.IsTrue(r5.IsSuccessfull);
            Assert.IsTrue(r6.IsSuccessfull);

            m_ftp.DeleteFile(file1);
            m_ftp.DeleteFile(dir1File1);
            m_ftp.DeleteFile(file2);
            m_ftp.DeleteDirectory(dir1);
            File.Delete(file1);

        }


        #region (------  Constants  ------)
        //private const string TEST_FTP_HOST_NAME2 = "anignora.com";
        //private const string TEST_FTP_HOST_IP2 = "184.168.152.47";
        //private const string TEST_FTP_PASSWORD2 = "Aa!12345";
        //private const string TEST_FTP_USERNAME2 = "anignora";
        private const string TEST_FTP_HOST_IP2 = "10.0.0.4";
        private const string TEST_FTP_PASSWORD2 = "1234";
        private const string TEST_FTP_USERNAME2 = "anigno";
		#endregion (------  Constants  ------)

		#region (------  Fields  ------)
        private FtpWebRequestClient m_ftp;
		#endregion (------  Fields  ------)

		#region (------  Events Handlers  ------)
  

        private void onFtpFileUploaded(object sender, FtpFileTransferEventArgs e)
        {
            Console.WriteLine("Uploaded " + e);
        }

        private void onFtpFileDownloadEnded(object sender, FtpFileTransferEventArgs e)
        {
            Console.WriteLine("Downloaded " + e);
        }

        private void onFtpFileUploadEnded(object sender, FtpFileTransferEventArgs e)
        {
            Console.WriteLine("Uploading " + e);
        }

        private void onFtpFileDownloading(object sender, FtpFileTransferEventArgs e)
        {
            Console.WriteLine("Downloading " + e);
        }
		#endregion (------  Events Handlers  ------)

        #region (------  Public Methods  ------)
        //[Test]
        //public void TestCreateDirectory()
        //{
        //    const string REMOTE_DIRECTORY1 = "dir1";
        //    const string REMOTE_DIRECTORY2 = "dir1/dir2";
        //    if (m_ftp.IsDirExist(REMOTE_DIRECTORY2)) Assert.IsTrue(m_ftp.DeleteDirectory(REMOTE_DIRECTORY2));
        //    if (m_ftp.IsDirExist(REMOTE_DIRECTORY1)) Assert.IsTrue(m_ftp.DeleteDirectory(REMOTE_DIRECTORY1));
            
        //    Assert.IsTrue(m_ftp.CreateDirectory(REMOTE_DIRECTORY1));
        //    Assert.IsTrue(m_ftp.CreateDirectory(REMOTE_DIRECTORY2));
        //    Assert.IsTrue(m_ftp.IsDirExist(REMOTE_DIRECTORY2));
        //    Assert.IsTrue(m_ftp.DeleteDirectory(REMOTE_DIRECTORY2));
        //    Assert.IsFalse(m_ftp.IsDirExist(REMOTE_DIRECTORY2));
        //    Assert.IsTrue(m_ftp.DeleteDirectory(REMOTE_DIRECTORY1));
        //    Assert.IsFalse(m_ftp.IsDirExist(REMOTE_DIRECTORY1));
        //}

        //[Test]
        //public void TestFilesAndDirs()
        //{
        //    const string TEST_DIR = "/TestDir";
        //    const string TEST_DIR1 = "/TestDir/Dir1";
        //    const string TEST_DIR2 = "/TestDir/Dir2";
        //    const string TEST_FILE_LOCAL = "TestFile.txt";
        //    const string TEST_FILE1 = "/TestDir/filel.txt";
        //    const string TEST_FILE2 = "/TestDir/file2.txt";
        //    Assert.IsTrue(m_ftp.CreateDirectory(TEST_DIR));
        //    Assert.IsTrue(m_ftp.IsDirExist(TEST_DIR));
        //    Assert.IsTrue(m_ftp.CreateDirectory(TEST_DIR1));
        //    Assert.IsTrue(m_ftp.CreateDirectory(TEST_DIR2));
        //    Assert.IsTrue(m_ftp.IsDirExist(TEST_DIR1));
        //    Assert.IsTrue(m_ftp.IsDirExist(TEST_DIR2));
        //    File.WriteAllText(TEST_FILE_LOCAL, "1234567890");
        //    long l = 1;
        //    Assert.IsTrue(m_ftp.UploadFile(TEST_FILE_LOCAL, TEST_FILE1, ref l));
        //    Assert.IsTrue(m_ftp.UploadFile(TEST_FILE_LOCAL, TEST_FILE2, ref l));
        //    string[] dirs = m_ftp.GetDirectories(TEST_DIR);
        //    string[] files = m_ftp.GetFiles(TEST_DIR);
        //    Assert.AreEqual(2, dirs.Length);
        //    Assert.AreEqual(2, files.Length);
        //    Assert.IsTrue(m_ftp.DeleteDirectory(TEST_DIR));

        //}


        //[Test]
        //public void TestStreamUploadDownload()
        //{
        //    MemoryStream ms = new MemoryStream();
        //    ASCIIEncoding asciiEncoding = new ASCIIEncoding();
        //    const string TEST_STRING = "This is a string";
        //    byte[] bytesUpload = asciiEncoding.GetBytes(TEST_STRING);
        //    ms.Write(bytesUpload, 0, bytesUpload.Length);
        //    const string TEST_REMOTE_FILENAME = "StreamFile.txt";
        //    m_ftp.DeleteFile(TEST_REMOTE_FILENAME);
        //    Assert.IsFalse(m_ftp.IsFileExist(TEST_REMOTE_FILENAME));
        //    long l = 1;
        //    m_ftp.UploadStream(ms, TEST_REMOTE_FILENAME, ref l);
        //    Assert.IsTrue(m_ftp.IsFileExist(TEST_REMOTE_FILENAME));
        //    ms = new MemoryStream();
        //    m_ftp.DownloadStream(TEST_REMOTE_FILENAME, ms, ref l);
        //    byte[] bytesDownload = new byte[ms.Length];
        //    ms.Read(bytesDownload, 0, (int) ms.Length);
        //    string s = asciiEncoding.GetString(bytesDownload);
        //    Assert.AreEqual(TEST_STRING, s);
        //}

        //[Test]
        //public void TestUploadAndDownload()
        //{
        //    const string TEST_FILE_NAME_LOCAL = "TestFile.txt";
        //    const string TEST_FILE_NAME_REMOTE = "TestDir/TestFile.txt";
        //    StringBuilder sb = new StringBuilder();
        //    for (int a = 0; a < 1000; a++)
        //    {
        //        sb.Append("1234567890");
        //    }
        //    File.WriteAllText(TEST_FILE_NAME_LOCAL, sb.ToString());
        //    //Upload
        //    m_ftp.DeleteFile(TEST_FILE_NAME_REMOTE);
        //    Assert.IsFalse(m_ftp.IsFileExist(TEST_FILE_NAME_REMOTE));
        //    long l = 1;
        //    m_ftp.UploadFile(TEST_FILE_NAME_LOCAL, TEST_FILE_NAME_REMOTE, ref l);
        //    Assert.IsTrue(m_ftp.IsFileExist(TEST_FILE_NAME_REMOTE)); //Download
        //    File.Delete(TEST_FILE_NAME_LOCAL);
        //    Assert.IsFalse(File.Exists(TEST_FILE_NAME_LOCAL));
        //    m_ftp.DownloadFile(TEST_FILE_NAME_REMOTE, TEST_FILE_NAME_LOCAL, ref l);
        //    Assert.IsTrue(File.Exists(TEST_FILE_NAME_LOCAL));
        //    //Cleaning
        //    m_ftp.DeleteFile(TEST_FILE_NAME_REMOTE);
        //    Assert.IsFalse(m_ftp.IsFileExist(TEST_FILE_NAME_REMOTE));
        //    File.Delete(TEST_FILE_NAME_LOCAL);
        //    Assert.IsFalse(File.Exists(TEST_FILE_NAME_LOCAL));
        //}
        #endregion (------  Public Methods  ------)
    }
}