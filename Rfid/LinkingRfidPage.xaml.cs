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
using Microsoft.VisualBasic;

namespace Rfid
{
    public partial class LinkingRfidPage : Page
    {
        public LinkingRfidPage(int Id)
        {
            InitializeComponent();
            ID = Id;
            run(Id);

        }

        private int ID;
      
        public string RFIDCodeTagsWriter
        {
            set
            {
                RFIDCodeTags.Text = value;
            }
        }
     
        private void run(int Id)
        {
            RfidContext db = new RfidContext();
            M_Users user = new M_Users();
            user = db.C_Users.Where(z => z.ID == Id).Single();
            List<M_Rfids> prfids = new List<M_Rfids>();
            prfids = user.P_Rfids.ToList();
            M_Rfids rfideCode;
            rfideCode = prfids.Where(x => x.IsArhive == false).LastOrDefault();

            if (prfids.Where(x => x.IsArhive == false).LastOrDefault() != null)
            {
                RFIDCodeTags.Text = rfideCode.RfidID.ToString();
            }
            else
            {
                RFIDCodeTags.Text = "";
            }

            var queryAllCustomers = from cust in db.C_Rfids
                                    where cust.IsArhive == true && cust.P_Users.ID == Id
                                    select new { cust.Date, cust.RfidID, cust.Comment };
            List<M_Rfids> queryAllCustomers1 = db.C_Rfids.Where(x => x.IsArhive == true && x.P_Users.ID == Id).ToList();

            LAadded.Text = user.P_Rfids.ToList()[0].Date.ToShortDateString();

            if (queryAllCustomers.Count() > 0)
            {
                GridRegistryChangesRFIDtags.ItemsSource = queryAllCustomers.ToList();
            }

            if (GridRegistryChangesRFIDtags.Columns.Count > 0)
            {
                this.GridRegistryChangesRFIDtags.Columns[0].ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
            }

            GridRegistryChangesRFIDtags.Items.Refresh();

            if (GridRegistryChangesRFIDtags.Columns.Count != 0)
            {
                GridRegistryChangesRFIDtags.Columns[0].Header = Singelton.LanguageSetting.GetString("lin_l_AddingDate");
                GridRegistryChangesRFIDtags.Columns[1].Header = Singelton.LanguageSetting.GetString("lin_l_rfidCode");
                GridRegistryChangesRFIDtags.Columns[2].Header = Singelton.LanguageSetting.GetString("lin_l_Comment");
            }

        }
        public void SetEnableToolTip()
        {
            bool e = Singelton.IsToolTipEnabled;
            ToolTipService.SetIsEnabled(BUnlinkRFIDTag, e);
            ToolTipService.SetIsEnabled(SnapRFIDtag, e);
        }

        private void ClickUlinkRFID(object sender, RoutedEventArgs e)
        {
            RfidContext db = new RfidContext();
            var UsedRfid = db.C_Rfids.Where(z => z.P_Users.ID == ID).Where(x => x.IsArhive == false).SingleOrDefault();

            if (UsedRfid != null)
            {
                UsedRfid.IsArhive = true;
                UsedRfid.Comment = Application.Current.Resources["LR_LouseRfidComment"].ToString();
                db.SaveChanges();
            }
            else
            {
                MessageBox.Show(Application.Current.Resources["GRW_AllRfidArhive"].ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RFIDCodeTags.Text = "";   
        }
        private void SnapRFIDTag(object sender, RoutedEventArgs e)
        {
            WindowSnapRfidPage p = new WindowSnapRfidPage(ID);
            Singelton.Frame.NavigationService.Navigate(p);           
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetEnableToolTip();
            Singelton.MainWindow.ChangeStringAddres(Title);
        }
    }
}
