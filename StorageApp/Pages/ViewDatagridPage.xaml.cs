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
        public ViewDatagridPage(bool type)
        {
            InitializeComponent();

            if (type)
            {
                var data = App.Context.getPackages();
                dataGrid.ItemsSource = data;
            }
            else
            {
                var data = App.Context.getOperations();
                dataGrid.ItemsSource = data;
            }





        }
    }
}
