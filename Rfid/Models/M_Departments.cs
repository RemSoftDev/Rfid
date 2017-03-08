using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rfid.Models
{
    public class M_Departments : M_Base
    {
        [Key]
        public int ID { get; set; }
        [Index(IsUnique = true)]
        public string Name { get; set; }
        [Index(IsUnique = true)]
        public string CodeFull { get; set; }
        public virtual ICollection<M_Names> DepartmentDirectorName { get; set; }
        public virtual ICollection<M_Phones> DepartmentDirectorPhone { get; set; }
        public virtual ICollection<M_Users> P_Users { get; set; }
        public virtual M_InOutValidTimes P_InOutValidTimes { get; set; }

    }
}
