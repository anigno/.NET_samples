namespace AnignoraCommonAndHelpers.Tracers
{
    public interface ITracer
    {
        #region Public Methods

        void Trace(string p_format, params object[] p_params);

        #endregion
    }
}