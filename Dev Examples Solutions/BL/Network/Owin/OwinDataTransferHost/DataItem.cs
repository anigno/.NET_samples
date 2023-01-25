#region

using BlCommon;

#endregion

namespace OwinDataTransferHost
{
    public class DataItem
    {
        #region Public Properties

        public string Descriptor { get; set; }
        public byte[] Data { get; set; }
        public string Label { get; set; }

        #endregion
    }
}