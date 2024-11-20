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

namespace StorageApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для СonfirmReceiptPage.xaml
    /// </summary>
    public partial class СonfirmReceiptPage : Page
    {
        public Model.User User = default;
        public СonfirmReceiptPage(Model.User TUser)
        {
            InitializeComponent();
            User = TUser;

            foreach (var p in Context.getPackagesFromStorage(TUser.StorageId).Where(p => p.Status == "transfer"))
                Pkg_Id.Items.Add(p.PackageId + " " + p.ClientFullname + " " + p.ClientNumber);
            Pkg_Id.SelectedIndex = 0;
        }

        private async void ConfirmPkg(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Pkg_Id.SelectedValue as string))
            {
                MessageBox.Show("Выберите Посылку");
                return;
            }

            HttpResponseMessage responseContent2 = await ConfirmReceiptPkg();
            var responseBody2 = await responseContent2.Content.ReadAsStringAsync();
            if ((int)responseContent2.StatusCode == 200)
                MessageBox.Show("Операция совершена успешно");
            else
                MessageBox.Show("Ошибка");

            string Content = await responseContent2.Content.ReadAsStringAsync();
            MessageBox.Show(Content);
        }

        public async Task<HttpResponseMessage> ConfirmReceiptPkg()
        {
            try
            {
                HttpResponseMessage responseContent = await Context.postNewPkgOperationAsync(
                    Context.NumbBeforeSpace(Pkg_Id.SelectedItem as string),
                    User.UserId,
                    2,
                    User.StorageId);

                return responseContent;
            }
            catch (Exception ex)
            {
                return default(HttpResponseMessage);
            }
        }
    }
}
