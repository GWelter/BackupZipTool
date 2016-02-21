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
            string fileToBeDeleted = string.Format("{0}\\user_{1}", mainWindowViewModel.BackupFolder, date);

            try
            {
                if (Directory.Exists(fileToBeDeleted))
                    Directory.Delete(fileToBeDeleted, true);
                else
                    Console.WriteLine("Arquivo inexistente");
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

        private string createDirectory(string path)
        {
            string backupZipFolder = string.Format("{0}\\user_zip", path);

            if (!Directory.Exists(backupZipFolder))
            {
                Directory.CreateDirectory(backupZipFolder);
            }

            return backupZipFolder;
        }

        private void BackupZip()
        {
            try
            {
                string date = DateTime.Now.ToString("yyyyMMdd");

                string rootBackupFolder = string.Format("{0}\\user_{1}", mainWindowViewModel.BackupFolder, date);

                string backupZipFolder = createDirectory(mainWindowViewModel.BackupFolder);
                string backupZipFile = string.Format("{0}\\user_{1}.zip", backupZipFolder, date);

                CopyDirectory.DirectoryCopy(mainWindowViewModel.ToZipFolder, rootBackupFolder, true);

                ZipFile.CreateFromDirectory(rootBackupFolder, backupZipFile, CompressionLevel.NoCompression, false);
                mainWindowViewModel.LastBackup = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
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
