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
    /// Interaction logic for TrackWindow.xaml
    /// </summary>
    public partial class TrackWindow : Window
    {
        public TrackWindow()
        {
            InitializeComponent();
            RefreshBillsData();
        }
        private List<CheckedListItem> m_billsSources = new List<CheckedListItem>();

        /// <summary>
        /// Refreshes the bills data list.
        /// </summary>
        private void RefreshBillsData()
        {
            lbFilesSelect.ItemsSource = null;

            foreach (Bill bill in User.Bills)
            {
                if (bill.IsPaid == true)
                {
                    m_billsSources.Add(new CheckedListItem(bill.Name, bill));
                }
            }
            lbFilesSelect.ItemsSource = m_billsSources;
        }

        private void b_track_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
