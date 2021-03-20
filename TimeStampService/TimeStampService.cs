using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TimeStampService
{
    class TimeStampService
    {
        private readonly Timer _timer;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TimeStampService()
        {
            _timer = new Timer();
            _timer.Interval = 60*1000;
            _timer.Elapsed += new ElapsedEventHandler(SixtySecondsEvent);
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private static void SixtySecondsEvent(object sender, ElapsedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            string TimeStamp = DateTime.Now.ToString("s");
            String FormattedTimeStamp = TimeStamp.Replace(":",".");
            string FileName = Path.Combine(path, FormattedTimeStamp + ".txt");
            try
            {
                File.WriteAllText(FileName, TimeStamp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                logger.Error("Catched exception:" + ex);

            }
            logger.Info("Timestamp file successfully created");
           
        }
        public void Start() {
            _timer.Start();
            logger.Info("Service started!");
        }
        public void Stop()
        {
            _timer.Stop();
            logger.Info("Service stopped!");
        }
    }
}
