#region

using Castle.Windsor;
using MvvmPatternExample.ViewModels;
using MvvmPatternExample.Views;
using Rafael.Infra.WPF.Interfaces;

#endregion

namespace MvvmPatternExample.Installers
{
    public class UiMappingInstaller
    {
        #region Constructors

        public UiMappingInstaller(IWindsorContainer p_windsorContainer, IViewViewModelMapper p_viewViewModelMapper)
        {
            m_windsorContainer = p_windsorContainer;
            m_viewViewModelMapper = p_viewViewModelMapper;
        }

        #endregion

        #region Public Methods

        public void RegisterViews()
        {
            MainWindowViewModel mainWindowViewModel = m_windsorContainer.Resolve<MainWindowViewModel>();
            m_viewViewModelMapper.RegisterView(typeof (MainWindowView).Name, mainWindowViewModel);
        }

        #endregion

        #region Fields

        private readonly IWindsorContainer m_windsorContainer;

        private readonly IViewViewModelMapper m_viewViewModelMapper;

        #endregion
    }
}