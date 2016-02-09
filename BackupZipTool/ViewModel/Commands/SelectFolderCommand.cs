using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace BackupZipTool.ViewModel.Commands
{
    class SelectFolderCommand : ICommand
    {
        private MainWindowViewModel mainWindowViewModel;

        public SelectFolderCommand(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if(result.Equals(DialogResult.OK))
            {
                if (parameter.ToString().Equals("toBackupText"))
                {
                    mainWindowViewModel.BackupFolder = dialog.SelectedPath.ToString();
                }
                else
                {
                    mainWindowViewModel.ToZipFolder = dialog.SelectedPath.ToString();
                }
                Console.WriteLine(dialog.SelectedPath);
            }
        }
    }
}
