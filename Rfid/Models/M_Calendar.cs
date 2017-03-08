using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Models
{
    public class M_Calendar : M_Base
    {
        public CalendarType TypeDate { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual M_Users P_Users { get; set; }
    }

    public enum CalendarType
    {
        TimeOff,
        Truancy,
        Holideys,
        Vacation
    }
}
