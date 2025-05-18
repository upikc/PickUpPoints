using StorageApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.DataContracts;
using System.Text;
using System.Text.RegularExpressions;
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
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing.Imaging;
using QRCoder;
using System.Drawing;
using StorageApp.Windows;

namespace StorageApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateNewPackagePage.xaml
    /// </summary>
    public partial class CreateNewPackagePage : Page
    {
        public int userId = -1;
        public UserWindow win;
        public CreateNewPackagePage(int userID , UserWindow window)
        {
            InitializeComponent();
            win = window;
            userId=userID;
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
            }) || unitOfWeightComboBox.SelectedItem == null || dimensionComboBox.SelectedItem == null)
            {
                MessageBox.Show("заполните поля");
                return;
            }
            if (!decimal.TryParse(weightTbox.Text, out _) && weightTbox.Text.Trim() == "0")
            {

                MessageBox.Show("Заполните вес верно");
                return;
            }
            //MAIL
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(recipientMailTbox.Text);
            if (!match.Success)
            {

                MessageBox.Show("не верный формат Email получателя");
                return;
            }
            match = regex.Match(senderMailTbox.Text);
            if (!match.Success)
            {

                MessageBox.Show("не верный формат Email отправителя");
                return;
            }

            //PHONE
            Regex phoneRegex = new Regex(@"^\s*(\+7|8)[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d[\s\-()]*\d\s*$");
            if (!phoneRegex.IsMatch(senderNumberTbox.Text.Trim()) && senderNumberTbox.Text.Trim() != "")
            {
                MessageBox.Show("Неверный формат номера телефона отправителя");
                return;
            }

            if (!phoneRegex.IsMatch(recipientNumberTbox.Text.Trim()) && recipientNumberTbox.Text.Trim() != "")
            {
                MessageBox.Show("Неверный формат номера телефона получателя");
                return;
            }





            HttpResponseMessage responseContent = await AddPackageAsync();
            var responseBody = await responseContent.Content.ReadAsStringAsync();
            MessageBox.Show(/*(int)responseContent.StatusCode*/ responseBody);

            win.PKGSHOW();

        }


        public async Task<HttpResponseMessage> AddPackageAsync()
        {
            try
            { 
                
                int unitOfWeightId = unitOfWeightComboBox.SelectedIndex;
                string dimensionId = "pack";
                switch (dimensionComboBox.SelectedIndex)
                {
                    case 0:
                        dimensionId = "L_box";
                        break;
                    case 1:
                        dimensionId = "m_box";
                        break;
                    case 2:
                        dimensionId = "s_box";
                        break;
                    case 3:
                        dimensionId = "pack";
                        break;
                    default:
                        dimensionId = "pack";
                        break;
                }

                decimal weight = decimal.Parse(weightTbox.Text);
                string senderFname = senderFnameTbox.Text;
                string senderSname = senderSnameTbox.Text;
                string senderLname = senderLnameTbox.Text;
                string senderMail = senderMailTbox.Text;
                string senderNumber = senderNumberTbox.Text;

                string recipientFname = recipientFnameTbox.Text;
                string recipientSname = recipientSnameTbox.Text;
                string recipientLname = recipientLnameTbox.Text;
                string recipientMail = recipientMailTbox.Text;
                string recipientNumber = recipientNumberTbox.Text;

                string dimensionName = "Пакет"; // По умолчанию
                switch (dimensionComboBox.SelectedIndex)
                {
                    case 0:
                        dimensionId = "L_box";
                        dimensionName = "Большая коробка";
                        break;
                    case 1:
                        dimensionId = "m_box";
                        dimensionName = "Средняя коробка";
                        break;
                    case 2:
                        dimensionId = "s_box";
                        dimensionName = "Маленькая коробка";
                        break;
                    case 3:
                        dimensionId = "pack";
                        dimensionName = "Пакет";
                        break;
                }

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
                    userId);


                if (senderNumber == " ")
                {
                    senderNumber = "не указан";
                }
                if (recipientNumber == " ")
                {
                    recipientNumber = "не указан";
                }



                return responseContent;
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine("Error: " + ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

    }
}
