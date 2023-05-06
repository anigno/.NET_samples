#region

using System;
using System.Windows;
using System.Windows.Threading;
using Castle.Windsor;
using MvvmPatternExample.Installers;
using MvvmPatternExample.ViewModels;
using MvvmPatternExample.Views;
using Rafael.Infra.WPF.Interfaces;
using Rafael.Infra.WPF.Services;

#endregion

namespace MvvmPatternExample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constructors

        public App()
        {
            //Build the windsor container
            m_windsorContainer = WindsorContainerInstaller.BuildWindsorContainer();

            //Init UiFacade and UiMappingInstaller
            UIFacade.Init(m_windsorContainer.Resolve<IViewViewModelMapper>(), m_windsorContainer.Resolve<ICommandDataService>());
            UiMappingInstaller uiMappingInstaller = m_windsorContainer.Resolve<UiMappingInstaller>();
            uiMappingInstaller.RegisterViews();

            //Starting main view
            MainWindowView mainWindowView = m_windsorContainer.Resolve<MainWindowView>();
            Application.Current.MainWindow = mainWindowView;
            Application.Current.Dispatcher.BeginInvoke((Action) onIdleAfterAppLoad, DispatcherPriority.SystemIdle);

            //Alternative, Could use simple Show without onIdleAfterAppLoad() being called after window init
            //main.Show();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// An event handler for after main view is intialized before show
        /// </summary>
        private void onIdleAfterAppLoad()
        {
            //Start demo event publisher
            SomeGlobalEventPublisher someGlobalEventPublisher = m_windsorContainer.Resolve<SomeGlobalEventPublisher>();
            someGlobalEventPublisher.Start();
            //Show main window
            Application.Current.MainWindow.Show();
        }

        #endregion

        #region Fields

        private readonly IWindsorContainer m_windsorContainer;

        #endregion
    }
}