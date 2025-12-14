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

namespace Do_An_LTTQ.View.UserPage
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private void FriendList_MouseEnter(object sender, MouseEventArgs e)
        {
            FriendList.Width = 200;
        }

        private void FriendList_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!FriendList.IsMouseOver) { FriendList.Width = 100; }
        }
    }
}
