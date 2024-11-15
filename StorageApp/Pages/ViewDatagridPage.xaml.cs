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

namespace StorageApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для ViewDatagridPage.xaml
    /// </summary>
    public partial class ViewDatagridPage : Page
    {
        public ViewDatagridPage(int type)
        {
            InitializeComponent();

            if (type == 0)
            {
                var data = App.Context.getStorages();
                dataGrid.ItemsSource = data;
            }
            else if(type == 1)
            {
                var data = App.Context.getPackages();
                dataGrid.ItemsSource = data;
            }
            else if (type == 2)
            {
                var data = App.Context.getOperations();
                dataGrid.ItemsSource = data;
            }
            else if (type == 3)
            {
                var data = App.Context.getUsers();
                dataGrid.ItemsSource = data;
            }
        }
    }
}
