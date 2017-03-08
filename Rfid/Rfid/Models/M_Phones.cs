namespace Rfid.Models
{
    public class M_Phones : M_Base
    {
        public string PhoneNumber { get; set; }
        
        public virtual M_Departments P_Departments { get; set; }
        public virtual M_Users P_Users { get; set; }
    }
}
