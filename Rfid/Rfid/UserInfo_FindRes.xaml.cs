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
using Rfid.XamlProperties;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for UserInfo_FindRes.xaml
    /// </summary>
    public partial class UserInfo_FindRes : Page
    {
        private int ID = 0;
        RfidContext db = new RfidContext();
        List<M_Users> users = new List<M_Users>();

        public UserInfo_FindRes(
            string TB_User_FirstName,
            string TB_User_SecondName,
            string TB_User_ThirdName,
            string TB_User_PhoneNumber,
            string TB_User_DepartmentName,
            string TB_User_DepartmentDirector,
            string TB_User_Address
            )
        {
            InitializeComponent();
            M_Phones contacPhones = new M_Phones { PhoneNumber = TB_User_PhoneNumber };
            var byNames = db.C_Names.Where(x =>

            x.P_Users.IsUser &&

            !string.IsNullOrEmpty(TB_User_FirstName)
            && !string.IsNullOrEmpty(TB_User_SecondName)
            && !string.IsNullOrEmpty(TB_User_ThirdName)
            ? x.NameFirst.Contains(TB_User_FirstName) && x.NameLast.Contains(TB_User_SecondName) && x.NameThird.Contains(TB_User_ThirdName) :

            !string.IsNullOrEmpty(TB_User_FirstName)
            && !string.IsNullOrEmpty(TB_User_SecondName)
            ? x.NameFirst.Contains(TB_User_FirstName) && x.NameLast.Contains(TB_User_SecondName) :

            !string.IsNullOrEmpty(TB_User_FirstName)
            && !string.IsNullOrEmpty(TB_User_ThirdName)
            ? x.NameFirst.Contains(TB_User_FirstName) && x.NameThird.Contains(TB_User_ThirdName) :

            !string.IsNullOrEmpty(TB_User_SecondName)
            && !string.IsNullOrEmpty(TB_User_ThirdName)
            ? x.NameLast.Contains(TB_User_SecondName) && x.NameThird.Contains(TB_User_ThirdName) :

            !string.IsNullOrEmpty(TB_User_FirstName)
            ? x.NameFirst.Contains(TB_User_FirstName) :

            !string.IsNullOrEmpty(TB_User_SecondName)
            ? x.NameLast.Contains(TB_User_SecondName) :

            !string.IsNullOrEmpty(TB_User_ThirdName)
            ? x.NameThird.Contains(TB_User_ThirdName) :
            false
            
            //!string.IsNullOrEmpty(TB_User_Address)  


            //&& x.P_Users.P_Phones.Where(m => m.PhoneNumber.Contains(TB_User_PhoneNumber) && !string.IsNullOrEmpty(TB_User_PhoneNumber)).Count() > 0 
            //?
            //x.P_Users.P_Phones.Where(m => m.PhoneNumber.Contains(TB_User_PhoneNumber)).ToList()
            //: new List<M_Phones>() 

            ).ToList();

            //  zxc.Where(z => z.Contains("12")).Where(z => z.Contains("12").ToList();
            //  var byPhone = db.C_Phones.Where(x => x.PhoneNumber.Contains(TB_User_PhoneNumber) && !string.IsNullOrEmpty(TB_User_PhoneNumber)).ToList();

            var byDepartment = db.C_Departments.Where(x => x.Name.Contains(TB_User_DepartmentName) && !string.IsNullOrEmpty(TB_User_DepartmentName)).ToList();

            var byAddress = db.C_Users.Where(x => x.Address.Contains(TB_User_Address) && !string.IsNullOrEmpty(TB_User_Address)).ToList();

            users.AddRange(byAddress);

            foreach (var item in byNames)
            {
                if (item.P_Users != null)
                {
                    users.Add(item.P_Users);
                }
            }

            //foreach (var item in byPhone)
            //{
            //    if (item.P_Users != null)
            //    {
            //        users.Add(item.P_Users);
            //    }
            //}

            foreach (var item in byDepartment)
            {

                if (item.P_Users != null)
                {
                    foreach (var us in item.P_Users)
                    {
                        users.Add(us);
                    }
                }

            }

            ShowUser();
        }


        private void ShowUser()
        {
            int count = 0;

            foreach (M_Users item in users)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(300);
                MainGrid.RowDefinitions.Add(row);

                ID = item.ID;

                Grid DynamicGrid = CreateGridForFinfRes();

                Label Label;
                TextBlock txtBlock;
                Button button;

                SetLabelAndTB("First Name", item.P_Names.Single().NameFirst, 0, 1, out Label, out button, out txtBlock, DynamicGrid, ID);
                SetLabelAndTB("Second Name", item.P_Names.Single().NameLast, 1, 1, out Label, out button, out txtBlock, DynamicGrid, ID);
                SetLabelAndTB("Third Name", item.P_Names.Single().NameThird, 2, 1, out Label, out button, out txtBlock, DynamicGrid, ID);

                int countUserPhones = 1;

                foreach (M_Phones phones in item.P_Phones)
                {
                    SetLabelAndTB("Phone Number", phones.PhoneNumber.ToString(), 3, countUserPhones, out Label, out button, out txtBlock, DynamicGrid, ID);
                    countUserPhones++;
                }
                if (item.P_Departments != null)
                {
                    SetLabelAndTB("Department Name", item.P_Departments.Name.ToString(), 4, 1, out Label, out button, out txtBlock, DynamicGrid, ID);
                    SetLabelAndTB("Department Director", item.P_Departments.DepartmentDirectorName.Single().NameFirst.ToString(), 5, 1, out Label, out button, out txtBlock, DynamicGrid, ID);
                    SetLabelAndTB("Department Director", item.P_Departments.DepartmentDirectorName.Single().NameLast.ToString(), 5, 2, out Label, out button, out txtBlock, DynamicGrid, ID);
                    SetLabelAndTB("Department Director", item.P_Departments.DepartmentDirectorName.Single().NameThird.ToString(), 5, 3, out Label, out button, out txtBlock, DynamicGrid, ID);
                }

                SetLabelAndTB("Address", item.Address.ToString(), 6, 1, out Label, out button, out txtBlock, DynamicGrid, ID);

                Grid.SetRow(DynamicGrid, count);
                MainGrid.Children.Add(DynamicGrid);

                count++;
            }
        }



        private void SetLabelAndTB(string L_Content, string TB_Text, int row, int coloumn, out Label Label, out Button Button, out TextBlock txtBlock, Grid DynamicGrid, int ID)
        {
            Label = new Label();
            Label.Content = L_Content;
            Grid.SetRow(Label, row);
            Grid.SetColumn(Label, 0);

            txtBlock = new TextBlock();
            txtBlock.Text = TB_Text;

            Grid.SetRow(txtBlock, row);
            Grid.SetColumn(txtBlock, coloumn);

            Button = new Button();
            XP_Custom_Id.SetMyProperty(Button, ID.ToString());
            Button.Click += new RoutedEventHandler(ButtonClickEvent);

            Button.Content = "More";
            Grid.SetRow(Button, 6);
            Grid.SetColumn(Button, 4);

            Label line = new Label();
            line.Content = "-------------------------------------";

            Grid.SetRow(line, 7);
            Grid.SetColumn(line, 0);

            DynamicGrid.Children.Add(txtBlock);
            DynamicGrid.Children.Add(Label);
            DynamicGrid.Children.Add(Button);
            DynamicGrid.Children.Add(line);

        }

        private void ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            UserInfo p = new UserInfo(Convert.ToInt32((XP_Custom_Id.GetMyProperty(but))));
            this.NavigationService.Navigate(p);
        }

        private Grid CreateGridForFinfRes()
        {
            Grid DynamicGrid = new Grid();

            //ColumnDefinition gridCol1 = new ColumnDefinition();

            DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());


            // RowDefinition gridRow1 = new RowDefinition();

            DynamicGrid.RowDefinitions.Add(new RowDefinition());
            DynamicGrid.RowDefinitions.Add(new RowDefinition());
            DynamicGrid.RowDefinitions.Add(new RowDefinition());
            DynamicGrid.RowDefinitions.Add(new RowDefinition());
            DynamicGrid.RowDefinitions.Add(new RowDefinition());
            DynamicGrid.RowDefinitions.Add(new RowDefinition());
            DynamicGrid.RowDefinitions.Add(new RowDefinition());
            DynamicGrid.RowDefinitions.Add(new RowDefinition());

            return DynamicGrid;
        }

        private void Button_GoToUserProfile(object sender, RoutedEventArgs e)
        {
            UserInfo p = new UserInfo();
            this.NavigationService.Navigate(p);
        }
    }
}
