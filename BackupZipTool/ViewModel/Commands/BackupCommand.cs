using BackupZipTool.ViewModel.Services;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Input;

namespace BackupZipTool.ViewModel.Commands
{
    class BackupCommand : ICommand
    {
        private MainWindowViewModel mainWindowViewModel;
        private bool hasStarted = false;

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
            if (hasStarted) return false;

            bool canExecute = String.IsNullOrWhiteSpace(mainWindowViewModel.ToZipFolder) || String.IsNullOrWhiteSpace(mainWindowViewModel.BackupFolder);
            return !canExecute;
        }

        public void Execute(object parameter)
        {
            BackupScheaduler backupScheaduler = BackupScheaduler.Instance;
            backupScheaduler.addAction(BackupZip);
            hasStarted = true;
        }

        private void remove8thBackup()
        {
            string date = DateTime.Today.AddDays(-7).ToString("yyyyMMdd");
            string fileToBeDeleted = string.Format("{0}\\user_{1}.zip", mainWindowViewModel.BackupFolder, date);
            Console.WriteLine(fileToBeDeleted);

            try
            {
                File.Delete(fileToBeDeleted);
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BackupZip()
        {
            try
            {
                string date = DateTime.Now.ToString("yyyyMMdd");
                string backupFile = string.Format("{0}\\user_{1}.zip", mainWindowViewModel.BackupFolder, date);

                ZipFile.CreateFromDirectory(mainWindowViewModel.ToZipFolder, backupFile, CompressionLevel.Optimal, false);
                mainWindowViewModel.LastBackup = DateTime.Now.ToString("yyyy/MM/dd");
                remove8thBackup();
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
