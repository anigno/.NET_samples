﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

#endregion

namespace DataGrid
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Customer : INotifyPropertyChanged
    {
        #region Public Properties

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyPropertyChanged("LastName");
            }
        }

        public Gender Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                NotifyPropertyChanged("Gender");
            }
        }

        public Uri WebSite
        {
            get { return _webSite; }
            set
            {
                _webSite = value;
                NotifyPropertyChanged("WebSite");
            }
        }

        public bool ReceiveNewsletter
        {
            get { return _newsletter; }
            set
            {
                _newsletter = value;
                NotifyPropertyChanged("Newsletter");
            }
        }

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                NotifyPropertyChanged("Image");
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Methods

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Fields

        private string _firstName;
        private string _lastName;
        private Gender _gender;
        private Uri _webSite;
        private bool _newsletter;
        private string _image;

        #endregion
    }
}