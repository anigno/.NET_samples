#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

#endregion

namespace WpfTreeViewExamples.Library
{
    public class ImageTreeViewItem : TreeViewItem
    {
        #region Constructors

        public ImageTreeViewItem()
        {
            CreateTreeViewItemTemplate();
        }

        #endregion

        #region Public Properties

        public Uri ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;

                _image.Source = new BitmapImage(value);
            }
        }


        public string Text
        {
            get { return _textBlock.Text; }
            set { _textBlock.Text = value; }
        }

        #endregion

        #region Private Methods

        private void CreateTreeViewItemTemplate()
        {
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;


            _image = new Image();
            _image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            _image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            _image.Width = 16;
            _image.Height = 16;
            _image.Margin = new Thickness(2);

            stack.Children.Add(_image);

            _textBlock = new TextBlock();
            _textBlock.Margin = new Thickness(2);
            _textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            stack.Children.Add(_textBlock);


            Header = stack;
        }

        #endregion

        #region Fields

        private Uri _imageUrl = null;


        private Image _image = null;
        private TextBlock _textBlock = null;

        #endregion
    }
}