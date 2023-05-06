#region

using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Releasers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MvvmPatternExample.CommandsEvents;
using MvvmPatternExample.Config;
using MvvmPatternExample.ViewModels;
using MvvmPatternExample.Views;
using Rafael.Infra.WPF.Interfaces;
using Rafael.Infra.WPF.Services;

#endregion

namespace MvvmPatternExample.Installers
{
    public class WindsorContainerInstaller : IWindsorInstaller
    {
        #region Public Methods

        public static IWindsorContainer BuildWindsorContainer()
        {
            WindsorContainer windsorContainer = new WindsorContainer();
            windsorContainer.AddFacility<StartableFacility>();
            windsorContainer.Kernel.ReleasePolicy = new NoTrackingReleasePolicy();
            windsorContainer.Install(new WindsorContainerInstaller());
            return windsorContainer;
        }

        public void Install(IWindsorContainer p_container, IConfigurationStore p_store)
        {
            registerServices(p_container);
            registerViewsAndViewsModels(p_container);
            registerConfigurations(p_container);
            registerGlobalEvents(p_container);
        }

        #endregion

        #region Private Methods

        private void registerConfigurations(IWindsorContainer p_container)
        {
            p_container.Install(Configuration.FromXmlFile(@"Config\ConfigurationSample.xml")).Register(Component.For<ConfigurationSampleFirst>().Named("ConfigurationSampleFirstComponent"));
            p_container.Install(Configuration.FromXmlFile(@"Config\ConfigurationSample.xml")).Register(Component.For<ConfigurationSampleSecond>().Named("ConfigurationSampleSecondComponent"));
        }

        private void registerGlobalEvents(IWindsorContainer p_container)
        {
            p_container.Register(Component.For<SomeCommandEvent>().LifeStyle.Transient);
        }

        private void registerServices(IWindsorContainer p_container)
        {
            //Self registering Windsorcontainer
            p_container.Register(Component.For<IWindsorContainer>().Instance(p_container));
            //Register services
            p_container.Register(Component.For<IViewViewModelMapper>().ImplementedBy<ViewViewModelMapper>());
            p_container.Register(Component.For<UiMappingInstaller>());
            p_container.Register(Component.For<ICommandDataService>().ImplementedBy<CommandDataService>().ServiceOverrides(new {p_commandEventResolver = "CommandEventResolver"}));
            p_container.Register(Component.For<CommandEventResolver>().ImplementedBy<WindsorCommandEventResolver>().Named("CommandEventResolver"));
        }

        private void registerViewsAndViewsModels(IWindsorContainer p_container)
        {
            p_container.Register(Component.For<MainWindowView>());
            p_container.Register(Component.For<MainWindowViewModel>());
            p_container.Register(Component.For<SomeGlobalEventPublisher>());
        }

        #endregion
    }
}