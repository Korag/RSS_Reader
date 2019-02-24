using System.ServiceProcess;

namespace RSS_Downloader
{
    public partial class DownloaderService : ServiceBase
    {
        public DownloaderService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
            timer.Start();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            
        }

        protected override void OnStop()
        {
        }
    }
}
