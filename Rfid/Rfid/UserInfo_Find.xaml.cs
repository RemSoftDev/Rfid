using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for UserInfo_Find.xaml
    /// </summary>
    public partial class UserInfo_Find : Page
    {
        public UserInfo_Find()
        {
            InitializeComponent();
        }

        private void Button_FindUser(object sender, RoutedEventArgs e)
        {
            UserInfo_FindRes p = new UserInfo_FindRes(
                TB_User_FirstName.Text,
                TB_User_SecondName.Text,
                TB_User_ThirdName.Text,
                TB_User_PhoneNumber.Text,
                TB_User_DepartmentName.Text,
                TB_User_DepartmentDirector.Text,
                TB_User_Address.Text
                );

            this.NavigationService.Navigate(p);
        }
    }
}
