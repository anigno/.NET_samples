#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

#endregion

namespace WpfTreeViewExamples.Library
{
    public class WorldArea
    {
        #region Constructors

        public WorldArea()
        {
        }

        public WorldArea(string name)
            : this(name, null)
        {
        }

        public WorldArea(string name, ObservableCollection<Country> contries)
        {
            _name = name;
            _countries = contries;
        }

        #endregion

        #region Public Methods

        public static ObservableCollection<WorldArea> GetAll()
        {
            ObservableCollection<WorldArea> listToReturn = new ObservableCollection<WorldArea>();


            WorldArea treeItem = null;

            // North America 
            treeItem = new WorldArea();
            treeItem.Name = "North America";

            //treeItem.Countries.Add(  
            treeItem.Countries.Add(new Country("USA"));
            treeItem.Countries.Add(new Country("Canada"));
            treeItem.Countries.Add(new Country("Mexico"));

            listToReturn.Add(treeItem);


            // North America 
            treeItem = new WorldArea();
            treeItem.Name = "South America";

            treeItem.Countries.Add(new Country("Argentina"));
            treeItem.Countries.Add(new Country("Brazil"));
            treeItem.Countries.Add(new Country("Uruguay"));

            listToReturn.Add(treeItem);

            // Europe
            treeItem = new WorldArea();
            treeItem.Name = "Europe";

            treeItem.Countries.Add(new Country("UK"));
            treeItem.Countries.Add(new Country("Denmark"));
            treeItem.Countries.Add(new Country("France"));

            listToReturn.Add(treeItem);

            // Africa
            treeItem = new WorldArea();
            treeItem.Name = "Africa";

            treeItem.Countries.Add(new Country("Somalia"));
            treeItem.Countries.Add(new Country("Uganda"));
            treeItem.Countries.Add(new Country("Egypt"));

            listToReturn.Add(treeItem);

            // Asia
            treeItem = new WorldArea();
            treeItem.Name = "Asia";

            treeItem.Countries.Add(new Country("Pakistan"));
            treeItem.Countries.Add(new Country("China"));
            treeItem.Countries.Add(new Country("Japan"));

            listToReturn.Add(treeItem);

            // Australia
            treeItem = new WorldArea();
            treeItem.Name = "Australia";

            //treeItem.Countries.Add(new Country("Australia"));

            listToReturn.Add(treeItem);


            // Antarctica
            treeItem = new WorldArea();
            treeItem.Name = "Antarctica";


            listToReturn.Add(treeItem);


            return listToReturn;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string ImageUrl
        {
            get { return "../Images/" + Name.ToLower() + ".png"; }
        }

        public ObservableCollection<Country> Countries
        {
            get
            {
                if (_countries == null) _countries = new ObservableCollection<Country>();
                return _countries;
            }
            set { _countries = value; }
        }

        #endregion

        #region Fields

        private string _name = "";
        private ObservableCollection<Country> _countries = null;

        #endregion
    }

    public class Country
    {
        #region Constructors

        public Country() : this("")
        {
        }

        public Country(string name)
        {
            Name = name;
        }

        #endregion

        #region Public Properties

        public string Name { get; set; }

        public string ImageUrl
        {
            get { return "../Images/" + Name.ToLower() + ".png"; }
        }

        #endregion
    }
}