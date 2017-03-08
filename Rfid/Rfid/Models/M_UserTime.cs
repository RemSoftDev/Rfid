using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rfid.Models
{
    public class M_UserTime : M_Base
    {
        public DateTime? SingleDate { get; set; }
        public string Day { get; set;}

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public DateTime? LengthOfInside { get; set; }

        public DateTime? LengthOfOutside { get; set; }

        

        public virtual M_Users P_Users { get; set; }
    }
}
