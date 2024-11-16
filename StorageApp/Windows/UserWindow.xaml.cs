using StorageApp.Model;
using StorageApp.Pages;
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

namespace StorageApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow(User thisUser)
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new Page1();
        }
        private void ShowViewDatagridPage_Storages(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new ViewDatagridPage(0);
        }
        private void ShowViewDatagridPage_Pkg(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new ViewDatagridPage(1);
        }
        private void ShowViewDatagridPage_Operation(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new ViewDatagridPage(2);
        }
        private void ShowViewDatagridPage_Users(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new ViewDatagridPage(3);
        }

        private void ShowCreateNewStoragePage(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new CreateNewStoragePage();

        }
        private void ShowCreateNewUserPage(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new CreateNewUserPage();

        }
        
    }
}
