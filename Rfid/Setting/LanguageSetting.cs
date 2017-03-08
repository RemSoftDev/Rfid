using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rfid.Setting
{
    class LanguageSetting
    {
        public int SelectedLanguage
        {
            get
            {
                return App.Languages.IndexOf(App.Language);
            }
            set
            {
                if(App.Languages.Count<value)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    App.Language = App.Languages[value];
                }
            }
        }

        private ResourceDictionary resourse
        {
            get
            {

                Uri uri=null;
                switch (SelectedLanguage)
                {
                    case 0:
                        uri = new Uri(@"Resource\lang.xaml", UriKind.Relative);
                        break;
                    case 1:
                        uri = new Uri(@"Resource\lang.uk-UA.xaml", UriKind.Relative);
                        break;
                    case 2:
                        break;
                }
                return new ResourceDictionary() { Source = uri };
            }
        }
        public string GetString(string resourseName)
        {
            return (string)Application.Current.Resources[resourseName];
        }
    }
}
