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
using System.Windows.Shapes;
using System.Threading.Tasks;
using Microsoft.Win32;  
using BillsProject.Enums;

namespace BillsProject
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool m_isUserLoggedIn = false;

        public MainWindow()
        {
            InitializeComponent();
            RegistryCheck();
        }
        private bool ValidUserLogin()
        {
            string userName = tb_userName.Text;
            string password = DB_API.HashPassword(tb_password.Password);

            if (DB_API.IsUserExist(userName) && password == DB_API.GetPassword(userName))
            {
                m_isUserLoggedIn = true;
                RegistryUpdate();
                return true;
            }
            MessageBox.Show("Could not log in. wrong usename or password.", "Wrong details", MessageBoxButton.OK);
            return false;
        }

        private void b_payBills_Click(object sender, RoutedEventArgs e)
        {
            if (m_isUserLoggedIn)
            {
                PayBillWindow m_payBillWindow = new PayBillWindow();
                m_payBillWindow.Show();
            }
            else
            {
                MessageBox.Show("First login!", "Error", MessageBoxButton.OK);
            }
        }

        private void b_login_Click(object sender, RoutedEventArgs e)
        {
            if (ValidUserLogin())
            {
                ChangeDisplay(); // From login boxes to buttons
                InitializeUser();
            }

        }

        private void b_settings_Click(object sender, RoutedEventArgs e)
        {
            if (m_isUserLoggedIn)
            {
                SettingsWindow settingsWindow = new SettingsWindow();
                settingsWindow.ShowDialog();
            }
        }
        /// <summary>
        /// Initializes the static user details.
        /// </summary>
        private void InitializeUser()
        {
            User.UserName = "Assaf";
            User.Password = "123";
            User.MailNotification = MailNotificationTime.Week;
            User.EmailAddress = "assafrabin@gmail.com";
            User.Companies = new List<CompaniesToPay> { CompaniesToPay.Hot, CompaniesToPay.Electricity };
            User.Bills = new List<Bill> {new Bill {Name="Electricity 11.14", IsPaid = false, amount = 50},
                                        new Bill {Name="HOT 10.14", IsPaid = false, amount = 200},
                                        new Bill {Name="b", IsPaid  = true, amount = 100}};
        }

        private void b_trackBills_Click(object sender, RoutedEventArgs e)
        {
            if (m_isUserLoggedIn)
            {
                TrackWindow m_TrackWindow = new TrackWindow();
                m_TrackWindow.Show();
            }
            else
            {
                MessageBox.Show("First login!", "Error", MessageBoxButton.OK);
            }
        }

        public void CreateMyPasswordTextBox()
        {
            // Set the maximum length of text in the control to eight.
            tb_password.MaxLength = 8;
            // Assign the asterisk to be the password character.
            //            tb_password.PasswordChar = '*';
        }

        private void b_register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
        }

        /// <summary>
        /// Changes display from login details to the functionallity.
        /// </summary>
        private void ChangeDisplay()
        {
            g_login.Visibility = System.Windows.Visibility.Hidden;
            g_buttons.Visibility = System.Windows.Visibility.Visible;
            tbl_login.Visibility = System.Windows.Visibility.Hidden;
            tbl_choose.Visibility = System.Windows.Visibility.Visible;
        }

        private void RegistryUpdate()
        {
            string subKeyPath = "Software\\HomeBill";
            RegistryKey subKey;
            subKey = Registry.CurrentUser.CreateSubKey(subKeyPath);
            subKey.SetValue("UserName", tb_userName.Text);
            subKey.SetValue("Password", tb_password.Password);
        }

        private void RegistryCheck()
        {
            string subKeyPath = "Software\\HomeBill";
            RegistryKey subKey = Registry.CurrentUser.OpenSubKey(subKeyPath);
            if (subKey != null)
            { 
                tb_userName.Text = subKey.GetValue("UserName").ToString();
                tb_password.Password = subKey.GetValue("Password").ToString();
            }
        }
    }
}
