using System;
using System.Threading.Tasks;
using System.Timers;

namespace BackupZipTool.ViewModel.Services
{
    public sealed class BackupScheaduler
    {
        private static BackupScheaduler instance = null;
        private static readonly object padlock = new object();

        private static Action externalAction;
        private Timer timer;

        BackupScheaduler() {
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            syncHour();

            timer.Start();
        }

        public static BackupScheaduler Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new BackupScheaduler();
                    }
                    return instance;
                }
            }
        }

        public void addAction(Action backupZip)
        {
            externalAction = backupZip;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            externalAction();
            syncHour();
            timer.Start();
        }

        private void syncHour()
        {
            TimeSpan nextWork = DateTime.Today.AddDays(1).AddMinutes(1) - DateTime.Now;
            timer.Interval = nextWork.TotalMilliseconds;
        }
    }
}
