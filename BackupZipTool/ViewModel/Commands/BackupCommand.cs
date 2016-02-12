using BackupZipTool.ViewModel.Services;
using System;
using System.IO.Compression;
using System.Windows.Input;

namespace BackupZipTool.ViewModel.Commands
{
    class BackupCommand : ICommand
    {
        private MainWindowViewModel mainWindowViewModel;

        public BackupCommand(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            bool canExecute = String.IsNullOrWhiteSpace(mainWindowViewModel.ToZipFolder) || String.IsNullOrWhiteSpace(mainWindowViewModel.BackupFolder);
            return !canExecute;
        }

        public void Execute(object parameter)
        {
            //TODO CREATE SCHEADULE TASK
            BackupScheaduler backupScheaduler = BackupScheaduler.Instance;
            backupScheaduler.addAction(BackupZip);
        }

        private void BackupZip()
        {
            try
            {
                ZipFile.CreateFromDirectory(mainWindowViewModel.ToZipFolder, mainWindowViewModel.BackupFolder, CompressionLevel.Optimal, false);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Você não tem permissão de escrita no diretório escolhido.", ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao cifrar pasta", ex.StackTrace);
            }
        }
    }
}
