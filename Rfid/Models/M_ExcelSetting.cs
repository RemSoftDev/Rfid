namespace Rfid.Models
{
    public class M_ExcelSetting : M_Base
    {
        public string Path { get; set; }

        public virtual M_Setting P_Setting { get; set; }
    }
}