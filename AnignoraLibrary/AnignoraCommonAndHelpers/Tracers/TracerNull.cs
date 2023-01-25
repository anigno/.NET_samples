namespace AnignoraCommonAndHelpers.Tracers
{
    public class TracerNull : ITracer
    {
        #region Public Methods

        public void Trace(string p_format, params object[] p_params)
        {
            //Nothing
        }

        #endregion
    }
}