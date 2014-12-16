using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillsProject.Enums;

namespace BillsProject
{
    public static class User
    {
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static List<Bill> Bills { get; set; }
        public static string EmailAddress { get; set; }
        public static MailNotificationTime MailNotification { get; set; }
        public static List<CompaniesToPay> Companies { get; set; }
    }
}
