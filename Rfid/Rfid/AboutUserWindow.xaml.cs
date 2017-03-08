using Rfid.Context;
using Rfid.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Rfid
{
    /// <summary>
    /// Interaction logic for AboutUserWindow.xaml
    /// </summary>
    public partial class AboutUserWindow : Window
    {
        int ID;
        public AboutUserWindow(int Id)
        {
            InitializeComponent();
            ID = Id;
            run(Id);
        }

        public void run(int Id)
        {
            RfidContext db = new RfidContext();

            M_Users user = new M_Users();

            user = db.C_Users.Where(z => z.ID == Id).Single();

            M_Names names = user.P_Names.ToList()[0];

            var applicationPath = Directory.GetCurrentDirectory();
            var dir = new DirectoryInfo(System.IO.Path.Combine(applicationPath, "Photos"));
            string str = dir.FullName + "/" + user.Photo.ToString();

            imageUser.Source = new BitmapImage(new Uri(str));

            LNameR.Content = names.NameFirst;
            LMiddleNameR.Content = names.NameLast;
            LSurnameR.Content = names.NameThird;

            try
            {
                LDateOfBirthR.Content = user.D_Birth.Value.ToShortDateString();
            }
            catch (Exception ex)
            {

            }

            LAddressR.Content = user.Address;

            List<M_Phones> userPhones = new List<M_Phones>();

            userPhones = user.P_Phones.ToList();

            foreach (M_Phones phone in userPhones)
            {
                LPhoneR.Content += phone.PhoneNumber;

                if (phone != userPhones[userPhones.Count - 1])
                {
                    LPhoneR.Content += ", ";
                }
            }

            M_Names departmentsName = new M_Names();
            departmentsName = user.P_Departments.DepartmentDirectorName.ToList()[0];

            LDepartmentR.Content = user.P_Departments.Name;

            LHeadOfDepartmentR.Content += departmentsName.NameFirst + " ";
            LHeadOfDepartmentR.Content += departmentsName.NameLast + " ";
            LHeadOfDepartmentR.Content += departmentsName.NameThird + " ";

            List<M_Phones> departmentsPhones = new List<M_Phones>();
            departmentsPhones = user.P_Departments.DepartmentDirectorPhone.ToList();


            foreach (M_Phones phone in departmentsPhones)
            {
                LPhoneNumberR.Content += phone.PhoneNumber;

                if (phone != departmentsPhones[departmentsPhones.Count - 1])
                {
                    LPhoneR.Content += ", ";
                }
            }

            try
            {
                List<M_Users> contactUsers = new List<M_Users>();
                contactUsers = user.P_ManForContact.ToList();

                M_Names contactNames = new M_Names();
                List<M_Phones> contactPhones = new List<M_Phones>();

                contactNames = contactUsers[0].P_Names.ToList()[0];
                contactPhones = contactUsers[0].P_Phones.ToList();

                LContactDetailsFace1R.Content = contactNames.NameFirst + " " +
                                                contactNames.NameLast + " " +
                                                contactNames.NameThird + " ";

                foreach (M_Phones cPhones in contactPhones)
                {
                    LPhoneNumber1R.Content += cPhones.PhoneNumber;

                    if (cPhones != contactPhones[contactPhones.Count - 1])
                    {
                        LPhoneR.Content += ", ";
                    }
                }

                contactNames = contactUsers[1].P_Names.ToList()[0];
                contactPhones = contactUsers[1].P_Phones.ToList();

                LContactDetailsFace2R.Content = contactNames.NameFirst + " " +
                                                contactNames.NameLast + " " +
                                                contactNames.NameThird + " ";

                foreach (M_Phones cPhones in contactPhones)
                {
                    LPhoneNumber2R.Content += cPhones.PhoneNumber + " ";
                }
            }
            catch
            {

            }
            List<M_Rfids> prfids = new List<M_Rfids>();
            prfids = user.P_Rfids.ToList();

            var rfideCode = prfids.Where(x => x.IsArhive == false).Single();

            LRfidCode.Content = rfideCode.RfidID.ToString();

            if (user.Comment != null)
            {
                TBComment.Text = user.Comment.ToString();
            }
            if (prfids[0].Comment != null)
            {
                TBRfidCOmment.Text = prfids[0].Comment.ToString();
            }
        }

        private void ClickLinkingButton(object sender, RoutedEventArgs e)
        {
            LinkingRfidWindow lingRfid = new LinkingRfidWindow(ID);
            lingRfid.Show();
        }
    }
}
