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
using Rfid.Models;
using Rfid.Context;
using Rfid.Helpers;
using System.IO;
using Rfid.Sql;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for UserInfo_FindResult.xaml
    /// </summary>
    public partial class FindResultPage : Page
    {
        public FindResultPage()
        {
            InitializeComponent();
        }
        private RfidContext db = new RfidContext();

        public int SelectedId { get; private set; }
        public M_Users SelectedUser { get; private set; }
        public FindResultPage(UserFinderHelper find )
        {
            InitializeComponent();

            Func<ICollection<M_Phones>, string> genListPhones = (cust) =>
                {
                    string str = string.Empty;
                    foreach (M_Phones phone in cust)
                    {
                        str += phone.PhoneNumber + ", ";
                    }
                    if(str != string.Empty)
                    str = str.Substring(0, str.Length - 2);
                        return str;
                };

            using (var context = new RfidContext())
            {
                find.Context = context;
                var SearchResult = (from cust in find.Find()
                                    select new
                                    {
                                        cust.ID,
                                        cust.P_Names.FirstOrDefault().NameFirst,
                                        cust.P_Names.FirstOrDefault().NameLast,
                                        cust.P_Names.FirstOrDefault().NameThird,
                                        cust.P_Departments.Name,
                                        cust.P_InOutValidTimes.Start,
                                        cust.P_InOutValidTimes.End,
                                        cust.P_InOutValidTimes.Valid,
                                        cust.P_InOutValidTimes.Dinner,
                                        cust.P_Phones
                                       
                                    }).ToList();
                genereteColumns();
                List<TabItem> basicTable = new List<TabItem>();

                foreach(var item in SearchResult)
                {
                    var tableItem = new TableItem()
                    {
                        Id = item.ID,
                        FirstName = item.NameFirst,
                        LastName = item.NameLast,
                        ThirdName = item.NameThird,
                        Departament = item.Name,
                        Start = item.Start,
                        End = item.End,
                        Dinner = (DateTime)item.Dinner,
                        Valid = item.Valid,
                        Phone = genListPhones(item.P_Phones)
                    };

                    GridBasicInformation.Items.Add(tableItem);
                }

            }
                                    
        }
        public void genereteColumns()
        {
            DataGridTextColumn IdColumn = new DataGridTextColumn();
            IdColumn.Header = "Id";
            IdColumn.Binding = new Binding("Id");


            DataGridTextColumn FirstNameColumn = new DataGridTextColumn();
            FirstNameColumn.Header = "First Name";
            FirstNameColumn.Binding = new Binding("FirstName");


            DataGridTextColumn LastNameColumn = new DataGridTextColumn();
            LastNameColumn.Header = "Last Name";
            LastNameColumn.Binding = new Binding("LastName");


            DataGridTextColumn ThridNameColumn = new DataGridTextColumn();
            ThridNameColumn.Header = "Third Name";
            ThridNameColumn.Binding = new Binding("ThirdName");


            DataGridTextColumn DepartamentNameColumn = new DataGridTextColumn();
            DepartamentNameColumn.Header = "Departament Name";
            DepartamentNameColumn.Binding = new Binding("Departament");


            DataGridTextColumn StartColumn = new DataGridTextColumn();
            StartColumn.Header = "Start";
            StartColumn.Binding = new Binding("Start");
            StartColumn.ClipboardContentBinding.StringFormat = "HH:mm";


            DataGridTextColumn EndColumn = new DataGridTextColumn();
            EndColumn.Header = "End";
            EndColumn.Binding = new Binding("End");
            EndColumn.ClipboardContentBinding.StringFormat = "HH:mm";


            DataGridTextColumn ValidColumn = new DataGridTextColumn();
            ValidColumn.Header = "Valid";
            ValidColumn.Binding = new Binding("Valid");
            ValidColumn.ClipboardContentBinding.StringFormat = "HH:mm";

            DataGridTextColumn DinnerColumn = new DataGridTextColumn();
            DinnerColumn.Header = "Dinner";
            DinnerColumn.Binding = new Binding("Dinner");
            DinnerColumn.ClipboardContentBinding.StringFormat = "HH:mm";


            DataGridTextColumn Phone = new DataGridTextColumn();
            Phone.Header = "Phone";
            Phone.Binding = new Binding("Phone");

            if(Singelton.MainWindow.WindowState == WindowState.Maximized)
                Phone.Width = new DataGridLength(1,DataGridLengthUnitType.Star);
            else
                Phone.Width = new DataGridLength(1,DataGridLengthUnitType.Auto);

            GridBasicInformation.Columns.Add(IdColumn);
            GridBasicInformation.Columns.Add(FirstNameColumn);
            GridBasicInformation.Columns.Add(LastNameColumn);
            GridBasicInformation.Columns.Add(ThridNameColumn);
            GridBasicInformation.Columns.Add(DepartamentNameColumn);
            GridBasicInformation.Columns.Add(StartColumn);
            GridBasicInformation.Columns.Add(EndColumn);
            GridBasicInformation.Columns.Add(ValidColumn);
            GridBasicInformation.Columns.Add(DinnerColumn);
            GridBasicInformation.Columns.Add(Phone);

        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dynamic s = GridBasicInformation.CurrentCell.Item;
            if (s == DependencyProperty.UnsetValue)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_setSelectUser"], "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else

            {
                int id = s.Id;

                AboutUserPage p = new AboutUserPage(id);
                this.NavigationService.Navigate(p);
            }
        }
        private void GridBasicInformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridBasicInformation.SelectedIndex == -1)
            {
                return;
            }

            dynamic s = GridBasicInformation.CurrentCell.Item;
            SelectedId = s.Id;

            RfidContext db = new RfidContext();
            SelectedUser = (from d in db.C_Users
                            where d.ID == SelectedId
                            select d).Single();
            var dir = new DirectoryInfo(Singelton.PathToPhoto);

            var queryPhoto = from cust in db.C_Users
                             where cust.ID == SelectedId
                             select new { cust.Photo };

            M_Names names = db.Database.SqlQuery<M_Names>(new SqlGetUserName()
                .SQl_GetUserFirstName, SelectedId).SingleOrDefault();
            var firstName = names.NameFirst;
            var lastName = names.NameLast;
            var z = queryPhoto.ToList();
            NameUser.Text = firstName + " " + lastName;
            string str = dir.FullName + "/" + z[0].Photo;

            try
            {
                string NameImage = z[0].Photo;
                string[] words = NameImage.Split('_');
                int indexdot = words[2].IndexOf('.');
                NameUser.Text = words[1] + " " + words[2].Remove(indexdot);
                imageReport.ImageSource = ImageLoaderHelper.GetImageFromFolder(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_userDontHavePhoto"] + ex.ToString());
            }

            db.Dispose();

        }
        private void GridBasicInformation_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NameUser.Text = Application.Current.Resources["u_l_FirstName"] +
               " " + Application.Current.Resources["u_l_SecondName"];
            Singelton.MainWindow.ChangeStringAddres(Title);
        }
        private void GridBasicInformation_AutoGeneratedColumns(object sender, EventArgs e)
        {
            GridBasicInformation.Columns[1].Header = Application.Current.Resources["u_l_FirstName"];
            GridBasicInformation.Columns[2].Header = Application.Current.Resources["u_l_SecondName"];
            GridBasicInformation.Columns[3].Header = Application.Current.Resources["u_l_ThirdName"];
            GridBasicInformation.Columns[4].Header = Application.Current.Resources["u_l_DepartmentName"];
            GridBasicInformation.Columns[5].Header = Application.Current.Resources["dgc_stat"];
            GridBasicInformation.Columns[6].Header = Application.Current.Resources["dgc_end"];
            GridBasicInformation.Columns[7].Header = Application.Current.Resources["dgc_valid"];
            GridBasicInformation.Columns[8].Header = Application.Current.Resources["dgc_dinner"];
            GridBasicInformation.Columns[9].Header = Application.Current.Resources["u_l_Phone"];

        }
    }

    public class TableItem
    {
        public TableItem()
        {
            
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ThirdName { get; set; }
        public string Departament { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Valid { get; set; }
        public DateTime Dinner { get; set; }
        public string Phone { get; set; }
    }

}
