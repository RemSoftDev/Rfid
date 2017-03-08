using Rfid.Context;
using Rfid.Models;
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
    /// Interaction logic for UsersBigInfo.xaml
    /// </summary>
    public partial class UsersBigInfo : Page
    {
        public UsersBigInfo()
        {
            InitializeComponent();

            RfidContext db = new RfidContext();

            List<M_Users> lastUsers = new List<M_Users>();
            List<M_Names> contactNames = new List<M_Names>();
            List<M_UserTime> userTime = new List<M_UserTime>();

            lastUsers = db.C_Users.OrderByDescending(x => x.ID).Where(z => z.IsUser).ToList();

            var grid = UsersTimeInfo as Grid;

            int row = 1;

            foreach (M_Users cUser in lastUsers)
            {
                contactNames = cUser.P_Names.ToList();
                userTime = cUser.P_UserTime.OrderByDescending(x=>x.P_Users.ID).ToList();

                Label labelUserName = new Label();
                labelUserName.Content = contactNames[0].NameFirst + " " +
                                        contactNames[0].NameLast + " " +
                                        contactNames[0].NameThird + " ";

                Grid.SetColumn(labelUserName, 0);
                Grid.SetRow(labelUserName, row);

                grid.Children.Add(labelUserName);

                //------------------------------------------------
                Label labelUserDepartmen = new Label();
                labelUserDepartmen.Content = cUser.P_Departments.Name;

                Grid.SetColumn(labelUserDepartmen, 1);
                Grid.SetRow(labelUserDepartmen, row);

                grid.Children.Add(labelUserDepartmen);

                //------------------------------------------------
                Label labelUserTimeIn = new Label();
                labelUserTimeIn.Content = userTime.Last().TimeIn ;

                Grid.SetColumn(labelUserTimeIn, 2);
                Grid.SetRow(labelUserTimeIn, row);

                grid.Children.Add(labelUserTimeIn);

                //------------------------------------------------
                Label labelUserTimeOut = new Label();
                labelUserTimeOut.Content = userTime.Last().TimeOut;

                Grid.SetColumn(labelUserTimeOut, 3);
                Grid.SetRow(labelUserTimeOut, row);

                grid.Children.Add(labelUserTimeOut);

                //------------------------------------------------
                Label labelUserInside = new Label();
                if (userTime.Last().TimeOut == null)
                {

                    labelUserInside.Content = "Yes";
                }
                else
                {
                    labelUserInside.Content = "No";
                }
                Grid.SetColumn(labelUserInside, 4);
                Grid.SetRow(labelUserInside, row);

                grid.Children.Add(labelUserInside);

                row++;

            }
        }

    }
}
