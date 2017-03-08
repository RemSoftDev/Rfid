using Rfid.Context;
using Rfid.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid.Models
{
    public partial class M_Setting : M_Base
    {
        public AppTheme AppTheme { get; set; }
        public AccentTheme AccentTheme { get; set; }

        public virtual M_EmailSetting P_EmailSetting { get; set; }
        public virtual M_ExcelSetting P_ExcelSetting { get; set; }
        public virtual M_LanguageSetting P_LanguageSetting { get; set; }
        public virtual M_LimitTimerSetting P_LimitTimerSetting { get; set; }
    }

    public partial class M_Setting
    {
        public void ImportInBdSetting()
        {
            AppColors.CurrentAppAccent = AccentTheme;
            AppColors.CurrentAppTheme = AppTheme;
            Singelton.EmailSetting = new EmailSetting
            {
                MailFrom = P_EmailSetting.MailFrom,
                Password = P_EmailSetting.Password,
                PathAttachmentFile = P_EmailSetting.PathAttachmentFile,
                Subject = P_EmailSetting.Subject,
                Body = P_EmailSetting.Body,
                SmtpClient = P_EmailSetting.SmtpClient
            };

            Singelton.ExcelSetting = new ExcelSetting
            {
                Path = P_ExcelSetting.Path
            };

            Singelton.LanguageSetting = new LanguageSetting
            {
                SelectedLanguage = P_LanguageSetting.SelectedLanguage
            };

            Singelton.WatcherSetting = new LimitInsideSetting()
            {
                Interval = P_LimitTimerSetting.Interval.TimeOfDay,
                MaxTimeInside = P_LimitTimerSetting.MaxTimeInside.TimeOfDay
            };
        }
        public void ExportInBdSetting()
        {
            AccentTheme = Singelton.CurrentAccentTheme;
            AppTheme = Singelton.CurrentAppTheme;

            using (var db = new RfidContext())
            {
                if (this.P_EmailSetting == null)
                {
                    P_EmailSetting = new M_EmailSetting
                    {
                        MailFrom = Singelton.EmailSetting.MailFrom,
                        Password = Singelton.EmailSetting.Password,
                        PathAttachmentFile = Singelton.EmailSetting.PathAttachmentFile,
                        Subject = Singelton.EmailSetting.Subject,
                        Body = Singelton.EmailSetting.Body,
                        SmtpClient = Singelton.EmailSetting.SmtpClient,
                    };
                }
                else
                {
                    P_EmailSetting.MailFrom = Singelton.EmailSetting.MailFrom;
                    P_EmailSetting.Password = Singelton.EmailSetting.Password;
                    P_EmailSetting.PathAttachmentFile = Singelton.EmailSetting.PathAttachmentFile;
                    P_EmailSetting.Subject = Singelton.EmailSetting.Subject;
                    P_EmailSetting.Body = Singelton.EmailSetting.Body;
                    P_EmailSetting.SmtpClient = Singelton.EmailSetting.SmtpClient;
                }

                if (this.P_ExcelSetting == null)
                {
                    P_ExcelSetting = new M_ExcelSetting
                    {
                        Path = Singelton.ExcelSetting.Path
                    };
                }
                else
                {
                    P_ExcelSetting.Path = Singelton.ExcelSetting.Path;
                }

                if (this.P_LanguageSetting == null)
                {
                    P_LanguageSetting = new M_LanguageSetting
                    {
                        SelectedLanguage = Singelton.LanguageSetting.SelectedLanguage
                    };
                }
                else
                {
                    P_LanguageSetting.SelectedLanguage = Singelton.LanguageSetting.SelectedLanguage;
                }

                var standartTime = new DateTime(2000, 01, 01);

                if (this.P_LimitTimerSetting == null)
                {
                    P_LimitTimerSetting = new M_LimitTimerSetting
                    {
                        MaxTimeInside = standartTime + Singelton.WatcherSetting.MaxTimeInside,
                        Interval = standartTime + Singelton.WatcherSetting.Interval
                    };
                }
                else
                {
                    P_LimitTimerSetting.MaxTimeInside = standartTime + Singelton.WatcherSetting.MaxTimeInside;
                    P_LimitTimerSetting.Interval = standartTime + Singelton.WatcherSetting.Interval;
                }
            }
        }
    }
}


