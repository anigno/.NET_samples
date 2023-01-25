#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace SpecialListOfItems
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Constructors

        public MainWindowViewModel()
        {
            Persons = new ObservableCollection<Person>();
            Persons.Add(new Person() {Name = "AAA", Dates = new List<DateTime>(new DateTime[] {new DateTime(2000, 1, 1), new DateTime(2001, 1, 1)})});
            Persons.Add(new Person() {Name = "BBB", Dates = new List<DateTime>(new DateTime[] {new DateTime(2002, 1, 1), new DateTime(2002, 2, 2), new DateTime(2002, 2, 2)})});
            Persons.Add(new Person() {Name = "CCC", Dates = new List<DateTime>(new DateTime[] {new DateTime(2003, 1, 1), new DateTime(2007, 1, 1)})});
            Persons.Add(new Person() {Name = "DDD", Dates = new List<DateTime>(new DateTime[] {new DateTime(2004, 1, 1), new DateTime(2012, 2, 2)})});
        }

        #endregion

        #region Public Properties

        public ObservableCollection<Person> Persons { get; set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion
    }
}