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
        }

        private async void AddBtnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            HttpResponseMessage responseContent = await ValidateUserDataAsync();
            var responseBody = await responseContent.Content.ReadAsStringAsync();

            if ((int)responseContent.StatusCode == 200)
            {
                var result = MessageBox.Show("Добавить пользователя?", responseBody,
                MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {

                    //string text = Task.Run(async () => await App.Context.postNewUserAsync(new User(LoginTbox.Text,
                    //                                    PassTbox.Text,
                    //                                    Fname.Text,
                    //                                    Lname.Text,
                    //                                    phoneNumb.Text,
                    //                                    int.Parse(roleID.Text),
                    //                                    int.Parse(storageId.Text)))).Result;

                    //MessageBox.Show(text);   //проблема потоков

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

        public async Task<HttpResponseMessage> ValidateUserDataAsync()
        {
            try { 
            HttpResponseMessage responseContent = await App.Context.checkUserDataValid(
                LoginTbox.Text,
                PassTbox.Text,
                Fname.Text,
                Lname.Text,
                phoneNumb.Text,
                int.Parse(roleID.Text),
                int.Parse(storageId.Text));

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
                HttpResponseMessage responseContent = await App.Context.postNewUserAsync(LoginTbox.Text,
                    PassTbox.Text,
                    Fname.Text,
                    Lname.Text,
                    phoneNumb.Text,
                    int.Parse(roleID.Text),
                    int.Parse(storageId.Text));

                return responseContent;
            }
            catch (Exception ex)
            {
                return default(HttpResponseMessage);
            }
        }
    }
}
