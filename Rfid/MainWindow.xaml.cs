using MahApps.Metro;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Rfid.Context;
using Rfid.Helpers;
using Rfid.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
            {
                try
                {
                    db.C_Users.ToList();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

            СryptographerHelper.Decrypt("C:\\Users\\ostap\\Documents\\Rfid\\Photo");

            InitializeComponent();
                Singelton s = new Singelton();
                Singelton.Frame = RfitFrame;
                Singelton.MainWindow = this;
                db.C_Setting.Single().ImportInBdSetting();
                Singelton.WatcherSetting.StartWatching();
                db.SaveChanges();
                var p = new StartUpPage(); 
                Singelton.Frame.NavigationService.Navigate(p);
            }

        List<string> pnoneNumbersUser = new List<string>();
        List<string> pnoneNumbersContactMan = new List<string>();
        List<string> pnoneNumbersDirector = new List<string>();
        List<M_Users> contactMan = new List<M_Users>();       
        UserHelper HU = new UserHelper();
        RfidContext db = new RfidContext();

        private bool IsOpenFlayout
        {
            get
            {
                return SettingThemeFlyout.IsOpen;
            }
            set
            {
                SettingThemeFlyout.IsOpen = value;
            }
        }
        public void OpenFlayout()
        {
            this.IsOpenFlayout = !this.IsOpenFlayout;
        }
        public void SaveSetting()
        {
            db.C_Setting.SingleOrDefault().ExportInBdSetting();
            db.SaveChanges();
        }
        public void ChangeStringAddres(String Addres)
        {
            if (Addres != null)
            {
                if (Addres.Count() > 45)
                {
                    Addres = Addres.Remove(45) + "...";

                }
            }

            TitleAdress.Content = Addres;
        }
        
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            Singelton.Frame.NavigationService.Navigate(new StartUpPage());
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (Singelton.Frame.CanGoForward)
            {
                Singelton.Frame.GoForward();
                
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Singelton.MainWindow.Close();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Singelton.Frame.CanGoBack)
            {
                Singelton.Frame.GoBack();
            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void MaxRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaxRestoreButton.Content = Char.ConvertFromUtf32(0xE922);
                
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaxRestoreButton.Content = Char.ConvertFromUtf32(0xE923);
               
            }
        }
        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SettingThemeFlyout.IsOpen = !SettingThemeFlyout.IsOpen;
        }
        private void RedTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Red;
            SaveSetting();

        }
        private void GreenTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Green;
            SaveSetting();
        }
        private void BlueTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Blue;
            SaveSetting();
        }
        private void PurpleTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Purple;
            SaveSetting();
        }
        private void OrangeTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Orange;
            SaveSetting();
        }
        private void LimeTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Lime;
            SaveSetting();
        }
        private void TealTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Teal;
            SaveSetting();
        }
        private void CyanTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Cyan;
            SaveSetting();
        }
        private void IndigoTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Indigo;
            SaveSetting();
        }
        private void VioletTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Violet;
            SaveSetting();
        }
        private void PinkTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Pink;
            SaveSetting();
        }
        private void MagentaTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Magenta;
            SaveSetting();
        }
        private void CrimsonTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Crimson;
            SaveSetting();
        }
        private void YellowTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Yellow;
            SaveSetting();
        }
        private void BrownTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Brown;
            SaveSetting();
        }
        private void OliveTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Olive;
            SaveSetting();
        }
        private void SiennaTheme_Click(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppAccent = AccentTheme.Sienna;
            SaveSetting();
        }
        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppTheme = AppTheme.BaseDark;
            SaveSetting();
        }
        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            AppColors.CurrentAppTheme = AppTheme.BaseLight;
            SaveSetting();
        }
        private void ToolTipSetting_Checked(object sender, RoutedEventArgs e)
        {
            Singelton.IsToolTipEnabled = true;
            Singelton.Frame.Navigate(new SettingPage());
        }
        private void ToolTipSetting_Unchecked(object sender, RoutedEventArgs e)
        {
            Singelton.IsToolTipEnabled = false;
            Singelton.Frame.Navigate(new SettingPage());
        }

       
    }
}
