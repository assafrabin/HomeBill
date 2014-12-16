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

namespace BillsProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PayBillWindow : Window
    {
        private List<CheckedListItem> m_billsSources = new List<CheckedListItem>();

        /// <summary>
        /// Refreshes the bills data list.
        /// </summary>
        private void RefreshBillsData()
        {
            lbFilesSelect.ItemsSource = null;

            foreach (Bill bill in User.Bills)
            {
                if (bill.IsPaid == false)
                {
                    m_billsSources.Add(new CheckedListItem(bill.Name, bill));
                }
            }
            lbFilesSelect.ItemsSource = m_billsSources;
            CalculateAmount();
        }

        private void CalculateAmount()
        {
            int totalAmount = 0;
            foreach (CheckedListItem bill in m_billsSources)
            {
                if (bill.IsChecked == true)
                {
                    totalAmount += bill.BillToPay.amount;
                }
                tb_amount.Text = totalAmount.ToString();
            }
        }
        public PayBillWindow()
        {
            InitializeComponent();
            RefreshBillsData();
        }

        private void b_payNow_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckedListItem bill in m_billsSources)
            {
                if (bill.IsChecked)
                {
                    if (GetApprovalFromBank() && GetApprovalFromSecurity())
                    {
                        //
                    }
                }
            }
        }
        private bool GetApprovalFromSecurity()
        {
            return true;
        }
        private bool GetApprovalFromBank()
        {
            return true;
        }

        private void b_selectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckedListItem bill in m_billsSources)
            {
                bill.IsChecked = true;
            }
            RefreshBillsData();
        }

        private void b_clearAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckedListItem bill in m_billsSources)
            {
                bill.IsChecked = false;
            }
            RefreshBillsData();
        }

        private void cb_listItem_Changed(object sender, RoutedEventArgs e)
        {
            CalculateAmount();
        }
    }

    public class CheckedListItem
    {
        public CheckedListItem(string name, Bill billToPay) { Name = name; IsChecked = true; BillToPay = billToPay; }
        public string Name { get; set; }
        public Bill BillToPay { get; set; }
        public bool IsChecked { get; set; }
    }
}
