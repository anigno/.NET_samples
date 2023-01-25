using System.Collections.Generic;
using System.IO;
using log4net;

namespace AnignoraIO
{
    public static class CsvFileReaderWriter
    {
        #region Public Methods

        public static string[][] ReadCsvFile(string p_filename, bool p_isHeaderExist = true, char[] p_splitters = null)
        {
            string absolutePath = Path.GetFullPath(p_filename);
            s_log.DebugFormat("ReadCsvFile(), {0} HeaderExist={1}", absolutePath, p_isHeaderExist);
            if (p_splitters == null) p_splitters = new[] {','};
            List<string[]> linesList = new List<string[]>();
            string[] lines = File.ReadAllLines(p_filename);
            int startValue = p_isHeaderExist ? 1 : 0;
            for (int a = startValue; a < lines.Length; a++)
            {
                string[] parts = lines[a].Split(p_splitters);
                linesList.Add(parts);
            }
            s_log.DebugFormat("ReadCsvFile(), Read {0} lines", linesList.Count);
            return linesList.ToArray();
        }

        public static void WriteCsvFile(string p_filename, string[][] p_data, char p_splitter = ',')
        {
            string absolutePath = Path.GetFullPath(p_filename);
            s_log.DebugFormat("{0}", absolutePath);
            string[] lines = new string[p_data.Length];
            for (int a = 0; a < p_data.Length; a++)
            {
                for (int b = 0; b < p_data[a].Length; b++)
                {
                    lines[a] += p_data[a][b];
                    if (b < p_data[a].Length - 1) lines[a] += p_splitter;
                }
            }
            File.WriteAllLines(p_filename, lines);
        }

        #endregion

        #region Fields

        private static readonly ILog s_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}