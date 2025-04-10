using ControlzEx.Standard;
using MahApps.Metro.Controls.Dialogs;
using StorageApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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

            if (string.IsNullOrEmpty(adressTbox.Text))
            {
                MessageBox.Show("заполните поля");
                return;
            }
            else if (Context.getStorages().FirstOrDefault(x => x.storageAddr == adressTbox.Text) != default)
            {
                MessageBox.Show("Адресс уже занят");
                return;
            }

            string NormAdress =  Regex.Replace(adressTbox.Text, @"[^a-zA-Zа-яА-Я0-9\s]", "").ToLower().Trim();
            string[] repetitive_words = ["улица", "ул", "город", "гор"];

            string message = "Подтвердите добавление\nОбнаружены дубли :\n";
            foreach (Storage s in Context.getStorages())
            {
                string cleaned = Regex.Replace(s.storageAddr, @"\d", "");
                string[] result = cleaned.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string res in result)
                {
                    string lovRes = res.ToLower();
                    if (NormAdress.Contains(lovRes) && !repetitive_words.Contains(lovRes))
                    {
                        message += s.storageAddr + "\n";
                        break;
                    }
                }
            }

            if (message != "Подтвердите добавление\nОбнаружены дубли :\n")
            {
                MessageBoxResult result = MessageBox.Show(
                message,
                "Проверьте!",
                MessageBoxButton.OKCancel
                );


                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }

            }






            string responseContent = await Context.postNewStorageAsync(adressTbox.Text as string);
            MessageBox.Show(responseContent);

        }

    }
}
