using System;
using System.Collections.Generic;

namespace Rfid.Models
{
    public class M_Users : M_Base
    {
        public virtual ICollection<M_Names> P_Names { get; set; }
        public bool IsUser { get; set; }
        public string Address { get; set; }
        public bool isInside { get; set; }

        public string Photo { get; set; }
        public DateTime? D_Birth { get; set; }

        public virtual M_Users P_Users { get; set; }
        public virtual ICollection<M_Rfids> P_Rfids { get; set; }
        public virtual ICollection<M_Users> P_ManForContact { get; set; }
        public virtual ICollection<M_Phones> P_Phones { get; set; }
        public virtual ICollection<M_UserTime> P_UserTime { get; set; }
        public virtual M_Departments P_Departments { get; set; }
        public virtual M_InOutValidTimes P_InOutValidTimes { get; set; }
    }
}
