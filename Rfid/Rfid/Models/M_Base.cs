using System.ComponentModel.DataAnnotations;

namespace Rfid.Models
{
    public class M_Base
    {
        [Key]
        public int ID { get; set; }
        public string Comment { get; set; }
    }
}
