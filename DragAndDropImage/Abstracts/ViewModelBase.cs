using DevExpress.Mvvm;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DragAndDropImage.Abstracts
{
    class ViewModelBase:RaisePropertyChanged
    {
        #region Commands
        public DelegateCommand<DragEventArgs> DropCommand { get; set; }

        public DelegateCommand DragEnterCommand { get; set; }

        public DelegateCommand DragLeaveCommand { get; set; }

        public DelegateCommand BrowseImageCommand { get; set; }
        #endregion

        #region "Properties"
        private Visibility _DragVisibility=Visibility.Hidden;

        public Visibility DragVisibility
        {
            get { return _DragVisibility; }
            set { _DragVisibility = value;
                OnPropertyChanged(nameof(DragVisibility));
            }
        }

        private string _SelectedImage;

        public string SelectedImage
        {
            get { return _SelectedImage; }
            set { _SelectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }
        #endregion


        #region Constructor
        public ViewModelBase()
        {
            DropCommand = new DelegateCommand<DragEventArgs>(DropEvent);
            DragEnterCommand = new DelegateCommand(DragEnterEvent);
            DragLeaveCommand = new DelegateCommand(DragLeaveEvent);
            BrowseImageCommand = new DelegateCommand(BrowseImageEvent);
        }
        #endregion

        #region Methods

        private void BrowseImageEvent()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Filter = "Image Files|*.jpg;*.png;*.bmp;*.tiff;"
            };

            if (openFileDialog.ShowDialog() != true) return;

            SelectedImage = openFileDialog.FileName;
        }

        private void DragLeaveEvent() => DragVisibility = Visibility.Hidden;
        
        private void DragEnterEvent() => DragVisibility = Visibility.Visible;
        
        private void DropEvent(DragEventArgs obj)
        {
            try
            {
                DragVisibility = Visibility.Hidden;
                if (!obj.Data.GetDataPresent(DataFormats.FileDrop)) return;
                var files = obj.Data.GetData(DataFormats.FileDrop,true) as string[];
                files.ToList().ForEach(file => SelectedImage = file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\n\n"+ex.Source);
            }
        }
        #endregion


    }
}
