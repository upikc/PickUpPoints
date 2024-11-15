using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

namespace StorageApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateNewStoragePage.xaml
    /// </summary>
    public partial class CreateNewStoragePage : Page
    {
        public CreateNewStoragePage()
        {
            InitializeComponent();
        }

        private async void AddBtnClick(object sender, RoutedEventArgs e)
        {
            string adress = adressTbox.Text;
            string responseContent = Task.Run(async () => await App.Context.postNewStorageAsync(adress)).Result;
            MessageBox.Show(responseContent);

        }

    }
}
