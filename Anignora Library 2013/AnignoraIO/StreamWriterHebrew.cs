using System.Text;
using System.IO;

namespace AnignoraIO
{
    public class StreamWriterHebrew : StreamWriter
    {
        #region Constructors

        public StreamWriterHebrew(string path, bool append)
            : base(path, append, HEBREW_ENCODING)
        {
        }

        #endregion

        #region Fields

        internal static readonly Encoding HEBREW_ENCODING = Encoding.GetEncoding("windows-1255");

        #endregion
    }
}