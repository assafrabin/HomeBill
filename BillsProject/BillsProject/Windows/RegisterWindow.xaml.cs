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

namespace BillsProject
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void b_register_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForMissingFields() || DB_API.IsUserExist(tb_userName.Text))
                return;

            if (tb_password.Password != tb_confirmPassword.Password || tb_password.Password.Length < 6)
            {
                tb_confirmPassword.Password = "";
                tb_password.Password = "";
                return;
            }

            // Update user details
            DB_API.Insert(tb_userName.Text, DB_API.HashPassword(tb_confirmPassword.Password), tb_emailAddress.Text, cmb_notification.SelectedIndex);
            User.Password = tb_password.Password;
            User.EmailAddress = tb_emailAddress.Text;
            User.MailNotification = SettingsWindow.ConvertStringToMailNotification(cmb_notification.SelectedItem.ToString());
            this.Close();
        }

        private void tb_userName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DB_API.IsUserExist(tb_userName.Text))
            {
                tbl_userNameExist.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                tbl_userNameExist.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        /// <summary>
        /// Checks for missing fields and puts a red * next to them.
        /// </summary>
        /// <returns></returns>
        private bool CheckForMissingFields()
        {
            if (tb_userName.Text == "" || tb_password.Password == "" || tb_emailAddress.Text == "" || cmb_notification.SelectedIndex == -1)
            {
                tbl_missingFields.Visibility = System.Windows.Visibility.Visible;
                if (tb_userName.Text == "") { tbl_userEmpty.Visibility = System.Windows.Visibility.Visible; }
                else { tbl_userEmpty.Visibility = System.Windows.Visibility.Hidden; }
                if (tb_password.Password == "") { tbl_passwordEmpty.Visibility = System.Windows.Visibility.Visible; }
                else { tbl_passwordEmpty.Visibility = System.Windows.Visibility.Hidden; }
                if (tb_emailAddress.Text == "") { tbl_emailEmpty.Visibility = System.Windows.Visibility.Visible; }
                else { tbl_emailEmpty.Visibility = System.Windows.Visibility.Hidden; }
                if (cmb_notification.SelectedIndex == -1) { tbl_notificationEmpty.Visibility = System.Windows.Visibility.Visible; }
                else { tbl_notificationEmpty.Visibility = System.Windows.Visibility.Hidden; }
                return true;
            }
            return false;
        }

        private void tb_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (tb_password.Password.Length > 0) // Passwords are empty
            {
                if (tb_password.Password.Length < 6 && tb_confirmPassword.Password.Length < 6) // Passwords are shorter than 6 characters
                {
                    tbl_passwordShort.Visibility = System.Windows.Visibility.Visible;
                    tbl_passwordUnmatch.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    tbl_passwordShort.Visibility = System.Windows.Visibility.Hidden;

                    if (tb_password.Password != tb_confirmPassword.Password) // Passwords are not equal
                    {
                        tbl_passwordUnmatch.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        tbl_passwordUnmatch.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
            else
            {
                tbl_passwordUnmatch.Visibility = System.Windows.Visibility.Hidden;
                tbl_passwordShort.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
