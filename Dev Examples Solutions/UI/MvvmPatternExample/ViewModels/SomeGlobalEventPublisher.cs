#region

using System;
using System.Threading;
using System.Windows.Input;
using MvvmPatternExample.CommandsEvents;
using Rafael.Infra.WPF.Commands;
using Rafael.Infra.WPF.Common;
using Rafael.Infra.WPF.Interfaces;

#endregion

namespace MvvmPatternExample.ViewModels
{
    public class SomeGlobalEventPublisher : BaseViewModel
    {
        #region Constructors

        public SomeGlobalEventPublisher(ICommandDataService p_commandDataService)
            : base(p_commandDataService)
        {
            m_timer = new Timer(TimerCallback);
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            m_timer.Change(2000, 5000);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// RegisteredSubscriber activation ICommand property
        /// </summary>
        public ICommand ActivateSomeCommandEvent
        {
            get { return new CommandEx(p => { m_commandDataService.GetCommand<SomeCommandEvent>().Execute(p); }, p => true); }
        }

        #endregion

        #region Private Methods

        private void TimerCallback(object p_state)
        {
            ActivateSomeCommandEvent.Execute(DateTime.Now);
        }

        #endregion

        #region Fields

        private readonly Timer m_timer;

        #endregion
    }
}