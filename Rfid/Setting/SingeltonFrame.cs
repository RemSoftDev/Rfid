using MahApps.Metro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Rfid.Setting;
using System.Windows.Media;
using Rfid.Models;

namespace Rfid
{
    class Singelton
    {
        public Singelton()
        {
        }
        
        private static EmailSetting _emailSetting;
        private static ExcelSetting _excelSetting;
        private static LimitInsideSetting _watcherSetting;
        private static LanguageSetting _languageSetting;
        private static bool _isToolTipEnabled = true;

        
        public static string PathToPhoto = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Rfid" + "\\Photo" + ".{2559a1f2-21d7-11d4-bdaf-00c04f60b9f0}";
        public static string PathToDb = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Rfid";
        public static AccentTheme CurrentAccentTheme { get; set; }
        public static AppTheme CurrentAppTheme { get; set; }
        public static M_Users AuthorizedUser { get; set; }
        public static bool IsAthorizationAdmin
        {
            get
            {
                if(AuthorizedUser != null)
                {
                    return (bool)AuthorizedUser.IsAdmin;
                }
                return false;
            }
            
        }
        public static Frame Frame { get; set; }
        public static LimitInsideSetting WatcherSetting
        {
            get
            {
                if (_watcherSetting == null)
                 _watcherSetting = new LimitInsideSetting();

              
                return _watcherSetting;
            }
            set
            {
                _watcherSetting = value;
            }
        }
        public static EmailSetting EmailSetting
        {
            get
            {
                if (_emailSetting == null)
                    _emailSetting = new EmailSetting();
                return _emailSetting;
            }
            set
            {
                _emailSetting = value;
            }
        }
        public static ExcelSetting ExcelSetting
        {
            get
            {
                if (_excelSetting == null)
                    _excelSetting = new ExcelSetting();
                return _excelSetting;
            }
            set
            {
                _excelSetting = value;
            }
        }
        public static LanguageSetting LanguageSetting
        {
            get
            {
                if (_languageSetting == null)
                    _languageSetting = new LanguageSetting();
                return _languageSetting;
            }   
            set
            {
                _languageSetting = value;
            }
        }
        public static MainWindow MainWindow { get; set; }
        public static void ChangeAccent(AccentTheme accent)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                                              ThemeManager
                                                .GetAccent(accent.ToString()),
                                              ThemeManager
                                                .GetAppTheme(AppColors
                                                .CurrentAppTheme.ToString()));
            CurrentAccentTheme = accent;
        }
        public static void ChangeTheme(AppTheme theme)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                                              ThemeManager
                                                .GetAccent(AppColors
                                                .CurrentAppAccent.ToString()),
                                              ThemeManager
                                                .GetAppTheme(theme.ToString()));
            CurrentAppTheme = theme;
        }
        public static bool IsToolTipEnabled
        {
            get
            {
                return _isToolTipEnabled;
            }
            set
            {
                _isToolTipEnabled = value;
            }
        }
    }

    public static class AppColors
    {
        private static SolidColorBrush  _colorBackground;
        private static SolidColorBrush _colorForeground;
        private static SolidColorBrush _colorRegular;      
        private static AppTheme _currentAppTheme;
        private static AccentTheme _currentAppAccent;
        private static SolidColorBrush _colorHover;
        private static SolidColorBrush _colorPressed;

        public static AppTheme CurrentAppTheme
        {
            get
            {
                return _currentAppTheme;
            }
            set
            {
                if(value == AppTheme.BaseLight)
                {
                    _colorBackground = new SolidColorBrush(Colors.White);                 
                    _colorForeground = new SolidColorBrush(Colors.Black);
                    _colorRegular = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    _colorHover = new SolidColorBrush(Color.FromRgb(179, 179, 179));
                    _colorPressed = new SolidColorBrush(Color.FromRgb(159, 159, 159));
                }
                else
                {
                    _colorBackground = new SolidColorBrush(Color.FromRgb(43, 43, 43));
                    _colorForeground = new SolidColorBrush(Colors.White);
                    _colorRegular = new SolidColorBrush(Color.FromRgb(23, 23, 23));
                    _colorHover = new SolidColorBrush(Color.FromRgb(46, 46, 46));
                    _colorPressed = new SolidColorBrush(Color.FromRgb(68, 68, 68));

                }
                Singelton.ChangeTheme(value);
                ChangeResourses();
                _currentAppTheme = value;
                
            }
        }
        public static AccentTheme CurrentAppAccent
        {
            get
            {
                return _currentAppAccent;
            }
            set
            {
                Singelton.ChangeAccent(value);
                _currentAppAccent = value;
            }
        }
        public static SolidColorBrush ColorBackground
        {
            get
            {
                return _colorBackground;
            }
        }
        public static SolidColorBrush ColorForeground
        {
            get
            {
                return _colorForeground;
            }
        }
        public static SolidColorBrush ColorRegular
        {
            get
            {
                return _colorRegular;
            }
        }
        public static SolidColorBrush ColorHover
        {
            get
            {
                return _colorHover;
            }
        }
        public static SolidColorBrush ColorPressed
        {
            get
            {
                return _colorPressed;
            }
        }

        public static void ChangeResourses()
        {
            ResourceDictionary dict = App.Current.Resources;
            dict["ColorBackground"] = ColorBackground;
            dict["ColorForeground"] = ColorForeground;
            dict["ColorRegular"] = ColorRegular;
            dict["ColorHover"] = ColorHover;
            dict["ColorPressed"] = ColorPressed;
        }
    }

    public enum AppTheme
    {
        BaseDark,
        BaseLight
    }

    public enum AccentTheme
    {
        Red,
        Green,
        Blue,
        Purple,
        Orange,
        Lime,
        Teal,
        Cyan,
        Indigo,
        Violet,
        Pink,
        Magenta,
        Crimson, 
        Yellow,
        Brown,
        Olive,
        Sienna
    }


}
