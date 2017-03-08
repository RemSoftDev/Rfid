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
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Rfid
{
    public partial class WindowSnapRfidPage : Page
    {
        public WindowSnapRfidPage(int id)
        {
            InitializeComponent();
            ID = id;
        }

        public int ID { get; private set; }
        public string RfidNumber { get; private set; }

        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(button, e);
        }
        public void Snap()
        {
            try
            {
                RfidContext db = new RfidContext();
                var temp = db.C_Rfids.Where(z => z.P_Users.ID == ID).Where(x => x.IsArhive == false);
                M_Users user;

                if (temp.Count() != 0)
                {
                    foreach (var rfid in db.C_Rfids.Where(z => z.P_Users.ID == ID).Where(x => x.IsArhive == false))
                    {
                        rfid.IsArhive = true;
                    }

                    M_Rfids addRfid = new M_Rfids();
                    addRfid.RfidID = Convert.ToInt64(RfidNumber);
                    addRfid.IsArhive = false;
                    addRfid.Date = DateTime.Now;
                    user = db.C_Users.Where(z => z.ID == ID).Single();
                    addRfid.P_Users = user;
                    user.P_Rfids.Add(addRfid);
                }
                else
                {
                    foreach (var rfid in db.C_Rfids.Where(z => z.P_Users.ID == ID).Where(x => x.IsArhive == false))
                    {
                        rfid.IsArhive = true;
                    }

                    M_Rfids addRfid = new M_Rfids();
                    addRfid.RfidID = Convert.ToInt64(RfidNumber);
                    addRfid.IsArhive = false;
                    addRfid.Date = DateTime.Now;
                    user = db.C_Users.Where(z => z.ID == ID).Single();
                    addRfid.P_Users = user;
                    user.P_Rfids.Add(addRfid);

                }

                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            LinkingRfidPage p = new LinkingRfidPage(ID);

            try
            {
              
                RfidNumber = TSnapRFID.Text;
                p.RFIDCodeTagsWriter = RfidNumber;

            }
            catch (FormatException)
            {
                MessageBox.Show((string)Application.Current.Resources["msb_enterRfid"], "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Snap();
            Singelton.Frame.NavigationService.Navigate(p);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            Singelton.MainWindow.ChangeStringAddres(Title);
            Keyboard.Focus(TSnapRFID);
        }

        private void TSnapRFID_TextChanged(object sender, TextChangedEventArgs e)
        {
            button.IsEnabled = Regex.Match(TSnapRFID.Text, @"^(\d{1,10})$").Success;
        }
    }
}
