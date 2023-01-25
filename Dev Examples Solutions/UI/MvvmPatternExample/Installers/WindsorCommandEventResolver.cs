#region

using Castle.Windsor;
using Rafael.Infra.WPF.Services;

#endregion

namespace MvvmPatternExample.Installers
{
    public class WindsorCommandEventResolver : CommandEventResolver
    {
        #region Constructors

        public WindsorCommandEventResolver(IWindsorContainer p_windsorContainer)
        {
            m_windsorContainer = p_windsorContainer;
        }

        #endregion

        #region Public Methods

        public override TCommandEvent Create<TCommandEvent, TCommandParameter>(TCommandParameter p_theCommandParameter)
        {
            return m_windsorContainer.Resolve<TCommandEvent>(new {p_commandParameter = p_theCommandParameter});
        }

        public override TCommandEvent Create<TCommandEvent>()
        {
            return m_windsorContainer.Resolve<TCommandEvent>();
        }

        #endregion

        #region Fields

        private readonly IWindsorContainer m_windsorContainer;

        #endregion
    }
}