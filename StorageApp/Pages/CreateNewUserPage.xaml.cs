using StorageApp.Model;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace StorageApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateNewUserPage.xaml
    /// </summary>
    public partial class CreateNewUserPage : Page
    {
        public CreateNewUserPage()
        {
            InitializeComponent();

            foreach (var s in Context.getStorages())
                storageId.Items.Add(s.storageId + " " + s.storageAddr);
            storageId.SelectedIndex = 0;


            roleID.Items.Add(1 + " manager");
            roleID.Items.Add(2 + " storekeeper");
            roleID.SelectedIndex = 0;

        }

        private async void AddBtnClick(object sender, System.Windows.RoutedEventArgs e)
        {

            if (Context.ContainsNullOrWhiteSpace(new string[] { LoginTbox.Text,
                PassTbox.Text,
                Fname.Text,
                Lname.Text,
                phoneNumb.Text,
                roleID.SelectedValue as string,
                storageId.SelectedValue as string }))
            { 
                MessageBox.Show("заполните поля");
                return;
            }

            HttpResponseMessage responseContent = await ValidateUserDataAsync();
            var responseBody = await responseContent.Content.ReadAsStringAsync();

            if ((int)responseContent.StatusCode == 200)
            {
                var result = MessageBox.Show("Добавить пользователя?", responseBody,
                MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    HttpResponseMessage responseContent2 = await AddUserAsync();
                    var responseBody2 = await responseContent2.Content.ReadAsStringAsync();
                    if ((int)responseContent2.StatusCode == 200)
                        MessageBox.Show("Пользователь успешно добавлен");
                    else
                        MessageBox.Show("Ошибка");

                }
            }
            else
                MessageBox.Show((int)responseContent.StatusCode + responseBody);
        }


        //дальше идут конструкторы для формирования запросов

        public async Task<HttpResponseMessage> ValidateUserDataAsync()
        {
            try { 
            HttpResponseMessage responseContent = await Context.checkUserDataValid(
                LoginTbox.Text,
                PassTbox.Text,
                Fname.Text,
                Lname.Text,
                phoneNumb.Text,
                NumbBeforeSpace(roleID.SelectedValue as string),
                NumbBeforeSpace(storageId.SelectedValue as string));

                return responseContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка ID");
                return default(HttpResponseMessage);
            }
        }

        public async Task<HttpResponseMessage> AddUserAsync()
        {
            try
            {
                HttpResponseMessage responseContent = await Context.postNewUserAsync(LoginTbox.Text,
                    PassTbox.Text,
                    Fname.Text,
                    Lname.Text,
                    phoneNumb.Text,
                    NumbBeforeSpace(roleID.SelectedValue as string),
                    NumbBeforeSpace(storageId.SelectedValue as string));

                return responseContent;
            }
            catch (Exception ex)
            {
                return default(HttpResponseMessage);
            }
        }


        static int NumbBeforeSpace(string text)
        {
            return int.Parse(text.Split(' ', 2)[0]);
        }
    }
}
