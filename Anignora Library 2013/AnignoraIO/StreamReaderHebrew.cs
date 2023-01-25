using System.IO;
using System.Text;

namespace AnignoraIO
{
    public class StreamReaderHebrew : StreamReader
    {
        #region Constructors

        public StreamReaderHebrew(string path)
            : base(path, (Encoding) StreamWriterHebrew.HEBREW_ENCODING)
        {
        }

        #endregion
    }
}