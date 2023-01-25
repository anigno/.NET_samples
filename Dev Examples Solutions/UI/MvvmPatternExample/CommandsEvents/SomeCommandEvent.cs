#region

using Rafael.Infra.WPF.Commands;

#endregion

namespace MvvmPatternExample.CommandsEvents
{
    public class SomeCommandEvent : CommandParameterEvent<object>
    {
        #region Constructors

        public SomeCommandEvent(object p_commandParameter = null)
            : base(p_commandParameter)
        {
        }

        #endregion
    }
}