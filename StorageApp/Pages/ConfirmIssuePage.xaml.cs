using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для ConfirmIssuePage.xaml
    /// </summary>
    public partial class ConfirmIssuePage : Page
    {
        public Model.User User = default;
        public ConfirmIssuePage(Model.User TUser)
        {
            InitializeComponent();
            User = TUser;

            foreach (var p in Context.getPackagesFromStorage(TUser.StorageId).Where(p => p.Status == "received"))
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
            if (string.IsNullOrEmpty(CodeTTbox.Text))
            {
                MessageBox.Show("Введите код");
                return;
            }

            if (CodeTTbox.Text != GetLast4CharHashFromString(Context.NumbBeforeSpace(Pkg_Id.SelectedItem as string)))
            {
                string s = Pkg_Id.SelectedItem as string;
                MessageBox.Show("Код не верный " /*+ Context.NumbBeforeSpace(Pkg_Id.SelectedItem as string)*/);
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
                    3,
                    User.StorageId);

                return responseContent;
            }
            catch (Exception ex)
            {
                return default(HttpResponseMessage);
            }
        }

        public string GetLast4CharHashFromString(int numb)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(numb.ToString()));
                return (BitConverter.ToString(hashBytes).Replace("-", "").ToLower())[^4..];
            }
        }
    }
}
