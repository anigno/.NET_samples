#region

using System.Collections.ObjectModel;
using System.Windows.Input;
using Castle.Windsor;
using MvvmPatternExample.CommandsEvents;
using MvvmPatternExample.Config;
using MvvmPatternExample.Utils;
using MvvmPatternExample.Views;
using Rafael.Infra.WPF.Commands;
using Rafael.Infra.WPF.Common;
using Rafael.Infra.WPF.Interfaces;

#endregion

namespace MvvmPatternExample.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Constructors

        public MainWindowViewModel(ICommandDataService p_commandDataService, ConfigurationSampleFirst p_configurationSampleFirst, IWindsorContainer p_windsorContainer)
            : base(p_commandDataService)
        {
            m_windsorContainer = p_windsorContainer;
            ActivitiesCollection = new ObservableCollection<string>();

            //Register and subscribe to some global event
            registerCommandAndSubscribe<SomeCommandEvent, object>(SomeGlobalCommandEventHandler1, p => true);
            //Subscribe again with different handler, (already registered!)
            m_commandDataService.Subscribe<SomeCommandEvent>(SomeGlobalCommandEventHandler2);

            //Read Configuration
            ConfigurationSampleFirst configurationSampleFirst = p_configurationSampleFirst;
            ActivitiesCollection.Add("Read Id: " + configurationSampleFirst.Id);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Global event handler
        /// </summary>
        public void SomeGlobalCommandEventHandler1(SomeCommandEvent p_commandEvent)
        {
            MainWindowView mainWindowView = m_windsorContainer.Resolve<MainWindowView>();
            CrossThreadedActivities.Do(mainWindowView, () => { ActivitiesCollection.Add("SomeGlobalCommandEventHandler 1: " + p_commandEvent.CommandParameter); });
        }

        public void SomeGlobalCommandEventHandler2(SomeCommandEvent p_commandEvent)
        {
            MainWindowView mainWindowView = m_windsorContainer.Resolve<MainWindowView>();
            CrossThreadedActivities.Do(mainWindowView, () => { ActivitiesCollection.Add("SomeGlobalCommandEventHandler 2: " + p_commandEvent.CommandParameter); });
        }

        #endregion

        #region Public Properties

        public ObservableCollection<string> ActivitiesCollection { get; set; }

        /// <summary>
        /// ICommand property as appears in the view's control command attribute with binding
        /// </summary>
        public ICommand ButtonFirstClickedCommand
        {
            get
            {
                return new CommandEx(
                    //An event handler to execute
                    p_o => ActivitiesCollection.Add("ButtonFirstClickedCommand"),
                    //A condition for enabling the control
                    p_o1 => true);
            }
        }

        /// <summary>
        /// ICommand property as appear in the view's AttachedCmdBehavior command
        /// </summary>
        public ICommand LabelMouseDownAttachedCommand
        {
            get
            {
                return new CommandEx(
                    //An event handler to execute
                    p_o => ActivitiesCollection.Add("LabelMouseDownAttachedCommand"),
                    //A condition for enabling the control
                    p_o1 => true);
            }
        }

        #endregion

        #region Fields

        private readonly IWindsorContainer m_windsorContainer;

        #endregion
    }
}