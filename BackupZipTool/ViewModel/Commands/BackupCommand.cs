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
            try
            {
                ZipFile.CreateFromDirectory(mainWindowViewModel.ToZipFolder, mainWindowViewModel.BackupFolder, CompressionLevel.Fastest, true);
            }
            catch(UnauthorizedAccessException ex)
            {
                Console.WriteLine("Você não tem permissão de escrita no diretório escolhido.", ex.StackTrace);
            }
        }
    }
}
