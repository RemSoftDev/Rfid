using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Rfid
{
    public partial class App : Application
    {
        public App()
        {
            App.LanguageChanged += App_LanguageChanged;
            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US"));
            m_Languages.Add(new CultureInfo("uk-UA"));
            m_Languages.Add(new CultureInfo("ru-RU"));
        }

        private static List<CultureInfo> m_Languages = new List<CultureInfo>();

        public static List<CultureInfo> Languages
        {
            get
            {
                return m_Languages;
            }
        }    
        public static CultureInfo Language
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value == Thread.CurrentThread.CurrentUICulture)
                {
                    return;
                }
                //2. Создаём ResourceDictionary для новой культуры
                Thread.CurrentThread.CurrentUICulture = value;
                var dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "uk-UA":
                        {
                            dict.Source = new Uri($"Resource/lang.{value.Name}.xaml", UriKind.Relative);
                            break;
                        }
                    case "ru-RU":
                        {
                            dict.Source = new Uri($"Resource/lang.{value.Name}.xaml", UriKind.Relative);
                            break;
                        }
                    default:
                        {
                            dict.Source = new Uri("Resource/lang.xaml", UriKind.Relative);
                            break;
                        }
                }
                //3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Resource/lang.")
                                              select d).First();

                if (oldDict != null)
                {
                    int index = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(index, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }
                LanguageChanged(Application.Current, new EventArgs());
            }

        }

        public static event EventHandler LanguageChanged;
        
        private void App_LanguageChanged(object sender, EventArgs e)
        {
            Rfid.Properties.Settings.Default.DefaultLanguage = Language;
        }
        private void App_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Language = Rfid.Properties.Settings.Default.DefaultLanguage;
        }
    }
}
