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
using BillsProject.Enums;

namespace BillsProject
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void b_update_Click(object sender, RoutedEventArgs e)
        {
            if (tb_password.Password != tb_cofirmPassword.Password)
            {
                tb_cofirmPassword.Password = "";
                tb_password.Password = "";
                tbl_passwordMatch.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            // Update user details
            User.Password = tb_password.Password;
            User.EmailAddress = tb_emailAddress.Text;
            User.MailNotification = ConvertStringToMailNotification(cmb_notification.SelectedItem.ToString());
            // TODO: User.UpdateDBFromUser() //update DB  
            this.Close();
        }
        /// <summary>
        /// Converts string to mail notification enum value.
        /// </summary>
        /// <param name="selection">The combobox selection.</param>
        /// <returns></returns>
        public static MailNotificationTime ConvertStringToMailNotification(string selection)
        {
            switch (selection)
            {
                case "One Day":
                    return MailNotificationTime.Day;
                case "One Week":
                    return MailNotificationTime.Week;
            }
            return MailNotificationTime.TwoWeeks;
        }
    }
}
