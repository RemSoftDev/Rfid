using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Models
{
    public class M_InOutValidTimes : M_Base
    {
        //[ForeignKey("P_Departments")]
        //public int DepartmentsID { get; set; }

        //[ForeignKey("P_Users")]
        //public int UserID { get; set; }

        public DateTime Start { get; set; }


        public DateTime And { get; set; }


        public DateTime Valid { get; set; }
       

        public virtual ICollection<M_Departments> P_Departments { get; set; }
        public virtual ICollection<M_Users> P_Users { get; set; }
    }
}
