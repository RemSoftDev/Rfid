using System;

namespace Rfid.Models
{
    public class M_LimitTimerSetting : M_Base
    {
        public DateTime MaxTimeInside { get; set; }
        public DateTime Interval { get; set; }
      
        public virtual M_Setting P_Setting { get; set; }

    }
}