using StorageApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Xml.Linq;

namespace StorageApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateNewPackagePage.xaml
    /// </summary>
    public partial class CreateNewPackagePage : Page
    {
        public int userId = -1;
        public CreateNewPackagePage(int userID)
        {
            InitializeComponent();
            userId=userID;
        }

        private async void AddBtnClick(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage responseContent = await AddPackageAsync();

            var responseBody = await responseContent.Content.ReadAsStringAsync();
            MessageBox.Show((int)responseContent.StatusCode + responseBody);
        }


        public async Task<HttpResponseMessage> AddPackageAsync()
        {
            try
            {
                HttpResponseMessage responseContent = await App.Context.postNewPackageAsync(decimal.Parse(weightTbox.Text),
                    fullNameTbox.Text,
                    mailTbox.Text,
                    numbTbox.Text,
                    userId);

                return responseContent;
            }
            catch (Exception ex)
            {
                return default(HttpResponseMessage);
            }
        }
    }
}
