#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace SpecialListOfItems
{
    public class Person
    {
        #region Public Properties

        public string Name { get; set; }
        public List<DateTime> Dates { get; set; }

        public CommandImplementor OnButtonClick { get; set; } = new CommandImplementor(p_o =>
        {
            Person person = p_o as Person;
        }, p_o => true);

        #endregion
    }
}