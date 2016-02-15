using BackupZipTool.ViewModel.Commands;
using BackupZipTool.ViewModel.Services;
using System;
using System.ComponentModel;

namespace BackupZipTool.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private String _toZipFolder = "";
        public String ToZipFolder
        {
            get { return _toZipFolder; }
            set
            {
                _toZipFolder = value;
                RaisePropertyChanged("ToZipFolder");

            }
        }

        private String _backupFolder = "";
        public String BackupFolder
        {
            get { return _backupFolder; }
            set
            {
                _backupFolder = value;
                RaisePropertyChanged("BackupFolder");

            }
        }

        private string _lastBackup = "";
        public string LastBackup
        {
            get { return "Last Backup " + _lastBackup; }
            set
            {
                _lastBackup = value;
                RaisePropertyChanged("LastBackup");
            }
        }

        #region Commands
        public BackupCommand backupCommand { get; set; }
        public SelectFolderCommand selectFolderCommand { get; set; }

        #endregion

        public MainWindowViewModel()
        {
            backupCommand = new BackupCommand(this);
            selectFolderCommand = new SelectFolderCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
