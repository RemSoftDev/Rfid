using Rfid.Context;
using Rfid.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Rfid.Setting
{
    public class LimitInsideSetting
    {
        private Dictionary<int, Stopwatch> _allUserTimeInside;
        private DispatcherTimer _ticker;
        private RfidContext _context;

        public LimitInsideSetting()
        {
            _allUserTimeInside = new Dictionary<int, Stopwatch>();

            _ticker = new DispatcherTimer()
            {
                Interval = new TimeSpan(1, 0, 0)

            };

            _ticker.Tick += Ticker_Tick;
        }

        public TimeSpan MaxTimeInside { get; set; } = new TimeSpan(10, 0, 0);
        public TimeSpan Interval
        {
            get
            {
                return _ticker.Interval;
            }
            set
            {
                _ticker.Interval = value;
            }
        }

        
        public void Ticker_Tick(object sender, EventArgs e)
        {
            _context = new RfidContext();
            foreach (var user in _context.C_Users)
            {
                if (user.isInside)
                {
                    if (user.P_UserTime.Count != 0)
                    {
                        user.P_UserTime.Last().TimeOut = DateTime.Now;
                        TimeSpan? lengthOfInside = user.P_UserTime.Last().TimeOut - user.P_UserTime.Last().TimeIn;
                        if (lengthOfInside > Singelton.WatcherSetting.MaxTimeInside)
                        {
                            lengthOfInside = Singelton.WatcherSetting.MaxTimeInside;
                            user.P_UserTime.Last().TimeOut = user.P_UserTime.Last().TimeIn + MaxTimeInside;
                            DateTime? dtInsige = DateTime.Today.Add(lengthOfInside.Value);
                            user.P_UserTime.Last().LengthOfInside = dtInsige;
                            user.isInside = false;
                            _context.SaveChanges();
                            
                        }
                        else
                        {

                        }
                    }
                }
            }
            _context.Dispose();

        }
        public void StartWatching()
        {
            _ticker.Start();
        }
        public void StopWatching()
        {
            _ticker.Stop();
        }
    }
}


