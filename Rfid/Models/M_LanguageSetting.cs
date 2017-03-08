namespace Rfid.Models
{
    public class M_LanguageSetting : M_Base
    {
        public int SelectedLanguage { get; set; }

        public virtual M_Setting P_Setting { get; set; }
    }
}