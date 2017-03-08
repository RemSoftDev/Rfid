using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Models
{
    public class M_Rfids : M_Base
    {
        public long RfidID { get; set; }
        public bool IsArhive { get; set; }
        public DateTime Date { get; set; }

        public virtual M_Users P_Users { get; set; }
    }
}
