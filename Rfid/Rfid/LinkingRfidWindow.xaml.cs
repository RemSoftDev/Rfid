using Rfid.Context;
using Rfid.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for LinkingRfidWindow.xaml
    /// </summary>
    public partial class LinkingRfidWindow : Window
    {
        int ID;
        public LinkingRfidWindow(int Id)
        {
            InitializeComponent();
            ID = Id;
            run(Id);

        }

        void run(int Id)
        {
            RfidContext db = new RfidContext();

            M_Users user = new M_Users();

            user = db.C_Users.Where(z => z.ID == Id).Single();

            List<M_Rfids> prfids = new List<M_Rfids>();
            prfids = user.P_Rfids.ToList();
            var rfideCode = prfids.Where(x => x.IsArhive == false).Single();
            RFIDCodeTags.Content = rfideCode.RfidID.ToString();
            
            var queryAllCustomers = from cust in db.C_Rfids
                                    where cust.IsArhive == true && cust.P_Users.ID == Id
                                    select new { cust.Date, cust.RfidID, cust.Comment };

            List<M_Rfids> queryAllCustomers1 = db.C_Rfids.Where(x => x.IsArhive == true && x.P_Users.ID == Id).ToList();
            var ListQueryAllCustomers = queryAllCustomers.ToList();
            LAadded.Content = user.P_Rfids.ToList()[0].Date.ToShortDateString();

            GridRegistryChangesRFIDtags.ItemsSource = queryAllCustomers.ToList();
            dynamic sdsdd = GridRegistryChangesRFIDtags.Items.CurrentItem;
            dynamic sdsdd1 = GridRegistryChangesRFIDtags.Items;
            if (GridRegistryChangesRFIDtags.Columns.Count>0)
            {
                this.GridRegistryChangesRFIDtags.Columns[0].ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
            }
            GridRegistryChangesRFIDtags.Items.Refresh();

        }

        private void ClickUlinkRFID(object sender, RoutedEventArgs e)
        {
            RfidContext db = new RfidContext();

            var temp = db.C_Rfids.Where(z => z.P_Users.ID == ID).Where(x => x.IsArhive == false);

            db.C_Rfids.Where(z => z.P_Users.ID == ID).Where(x => x.IsArhive == false).Single().IsArhive = true;
            db.C_Rfids.Where(z => z.P_Users.ID == ID).Where(x => x.IsArhive == false).Single().Comment = "louse";

            RFIDCodeTags.Content = "";
            db.SaveChanges();
        }

        private void SnapRFIDTag(object sender, RoutedEventArgs e)
        {
            try
            {

                RfidContext db = new RfidContext();
                var temp = db.C_Rfids.Where(z => z.P_Users.ID == ID).Where(x => x.IsArhive == false);

                WindowSnapRFID snapRfid = new WindowSnapRFID();

                snapRfid.ShowDialog();


                if (temp.Count() != 0)
                {
                    foreach (var rfid in db.C_Rfids.Where(z => z.P_Users.ID == ID).Where(x => x.IsArhive == false))
                    {
                        rfid.IsArhive = true;
                    }

                    M_Rfids addRfid = new M_Rfids();
                    addRfid.RfidID = Convert.ToInt64(snapRfid.TSnapRFID.Text);
                    addRfid.IsArhive = false;

                    db.C_Users.Where(z => z.ID == ID).Single().P_Rfids.Add(addRfid);
                }
                else
                {
                    foreach (var rfid in db.C_Rfids.Where(z => z.P_Users.ID == ID).Where(x => x.IsArhive == false))
                    {
                        rfid.IsArhive = true;
                    }

                    M_Rfids addRfid = new M_Rfids();
                    addRfid.RfidID = Convert.ToInt64(snapRfid.TSnapRFID.Text);
                    addRfid.IsArhive = false;
                    addRfid.Date = DateTime.Now;

                    db.C_Users.Where(z => z.ID == ID).Single().P_Rfids.Add(addRfid);
                }


                RFIDCodeTags.Content = snapRfid.TSnapRFID.Text;

                db.SaveChanges();

            }
            catch {
               // throw;
            }

        }
    }
}
