using StorageApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using StorageApp.Windows; // Ensure this namespace is correct for UserWindow

namespace StorageApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateNewPackagePage.xaml
    /// </summary>
    public partial class CreateNewPackagePage : Page
    {
        public int userId = -1;
        public int thisStorageId = -1;
        public UserWindow win;

        public CreateNewPackagePage(int userID, UserWindow window)
        {
            InitializeComponent();
            win = window;
            thisStorageId = Context.getUsers().FirstOrDefault(x => x.UserId == userID).StorageId;
            userId = userID;
            // Load storages when the page initializes
            LoadStorages();
        }

        private void LoadStorages()
        {
            var storages = Context.getStorages().Where(x => x.storageId != 0 && x.storageId != thisStorageId);
            destinationStorageComboBox.ItemsSource = storages;
            if (storages != null)
            {
                destinationStorageComboBox.SelectedIndex = 0;
            }
        }

        private async void AddBtnClick(object sender, RoutedEventArgs e)
        {
            if (Context.ContainsNullOrWhiteSpace(new string[] {
                weightTbox.Text,
                senderFnameTbox.Text,
                senderSnameTbox.Text,
                senderLnameTbox.Text,
                senderMailTbox.Text,
                recipientFnameTbox.Text,
                recipientSnameTbox.Text,
                recipientLnameTbox.Text,
                recipientMailTbox.Text,
            }) || unitOfWeightComboBox.SelectedItem == null || dimensionComboBox.SelectedItem == null || destinationStorageComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все обязательные поля.");
                return;
            }

            if (!decimal.TryParse(weightTbox.Text, out decimal weight) || weight <= 0) // Changed to check for non-positive weight as well
            {
                MessageBox.Show("Заполните вес верно (должно быть число больше 0).");
                return;
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(recipientMailTbox.Text))
            {
                MessageBox.Show("Неверный формат Email получателя.");
                return;
            }
            if (!regex.IsMatch(senderMailTbox.Text))
            {
                MessageBox.Show("Неверный формат Email отправителя.");
                return;
            }

            Regex phoneRegex = new Regex(@"^\s*(\+7|8)[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d\s*$");
            if (!string.IsNullOrWhiteSpace(senderNumberTbox.Text) && !phoneRegex.IsMatch(senderNumberTbox.Text.Trim()))
            {
                MessageBox.Show("Неверный формат номера телефона отправителя.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(recipientNumberTbox.Text) && !phoneRegex.IsMatch(recipientNumberTbox.Text.Trim()))
            {
                MessageBox.Show("Неверный формат номера телефона получателя.");
                return;
            }

            string dimensionId = "pack"; // Default value
            switch (dimensionComboBox.SelectedIndex)
            {
                case 0: dimensionId = "L_box"; break;
                case 1: dimensionId = "m_box"; break;
                case 2: dimensionId = "s_box"; break;
                case 3: dimensionId = "pack"; break;
            }

            int unitOfWeightId = unitOfWeightComboBox.SelectedIndex;

            if (destinationStorageComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите пункт назначения.");
                return;
            }
            int destinationStorageId = (int)destinationStorageComboBox.SelectedValue;


            HttpResponseMessage responseContent = await AddPackageAsync(
                weight,
                unitOfWeightId,
                dimensionId,
                senderFnameTbox.Text,
                senderSnameTbox.Text,
                senderLnameTbox.Text,
                senderMailTbox.Text,
                senderNumberTbox.Text, 
                recipientFnameTbox.Text,
                recipientSnameTbox.Text,
                recipientLnameTbox.Text,
                recipientMailTbox.Text,
                recipientNumberTbox.Text, 
                userId,
                destinationStorageId 
            );

            var responseBody = await responseContent.Content.ReadAsStringAsync();
            MessageBox.Show(responseBody);

            try
            {
                Package newPack = Context.getPackages().Last(x => x.ActionstorageId == (Context.getUsers().First(x => x.UserId == userId)).StorageId);
                Context.GenerateAndSaveReceiptAsPdf(newPack, Context.MakeDockFilePath(@$"Квинтация_{newPack.PackageId}.pdf"));
            }
            catch { MessageBox.Show("Проблема с генерацией квитанции"); }

            win.PKGSHOW();
        }

        public async Task<HttpResponseMessage> AddPackageAsync(
            decimal weight,
            int unitOfWeightId,
            string dimensionId,
            string senderFname,
            string senderSname,
            string senderLname,
            string senderMail,
            string? senderNumber,
            string recipientFname,
            string recipientSname,
            string recipientLname,
            string recipientMail,
            string? recipientNumber,
            int userId,
            int destinationStorageId) 
        {
            try
            {

                HttpResponseMessage responseContent = await Context.postNewPackageAsync(
                    weight,
                    unitOfWeightId,
                    dimensionId,
                    senderFname,
                    senderSname,
                    senderLname,
                    senderMail,
                    senderNumber,
                    recipientFname,
                    recipientSname,
                    recipientLname,
                    recipientMail,
                    recipientNumber,
                    userId,
                    destinationStorageId
                );

                return responseContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}