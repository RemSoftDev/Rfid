namespace Rfid.Models
{
    public class M_Names : M_Base
    {
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameThird { get; set; }

        public virtual M_Departments P_Departments { get; set; }
        public virtual M_Users P_Users { get; set; }
    }
}
