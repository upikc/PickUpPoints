using StorageApp.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.Xml;
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
    /// Логика взаимодействия для ConfirmPackTransferPage.xaml
    /// </summary>
    public partial class ConfirmPackTransferPage : Page
    {
        Model.User thisUser;
        UserWindow thisWindow;
        public ConfirmPackTransferPage(Model.User _user , UserWindow _window)
        {
            thisUser = _user;
            thisWindow = _window;
            InitializeComponent();
        }

        private void ConfirmBtnClick(object sender, RoutedEventArgs e)
        {
            ConfirmOpAsync();
        }

        private void Pkg_id_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //сканер поссе ввода кода автоматически нажимает enter 
                ConfirmOpAsync();
            }
        }

        private async Task ConfirmOpAsync()
        {
            int packId;

            if (int.TryParse(Pkg_id.Text, out packId) && Context.getPackages().Any(x => x.PackageId == packId)) //id валид
            {
                var pack = Context.getPackages().FirstOrDefault(x => x.PackageId == packId);
                if (pack.Status == "transfer" || pack.Status == "declare") //если создана - готова к отправке, если отправлена то можно получить
                {
                    switch (pack.Status)
                    {
                        case "declare":
                            await MakeOperation(1, pack.DestinationStorageId, thisUser.UserId, pack); //отправить
                            break;
                        case "transfer":
                            await MakeOperation(2, thisUser.StorageId, thisUser.UserId, pack); //получить
                            break;
                    }

                }
                else //уже получена на ПВЗ, или выдана 
                    MessageBox.Show("Посылка уже находиться на ПВЗ, или выдана");
            }
            else
                MessageBox.Show("Посылка не найдена");
        }

        public async Task MakeOperation(int operationType, int actionStorageId, int userId, Model.Package pack)
        {
            if (operationType == 1)
            {
                MessageBoxResult result = MessageBox.Show($"Посылка будет отправлена на {pack.DestinationStorageAddres}", "Внимание", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                    return;
            }
            else //получить можно если склад отправки тот же что и склад юзера
            {
                if (thisUser.StorageId != pack.ActionstorageId)
                    MessageBox.Show("Данная посылка к нам не направлена");
                    return; //попытка принять посылку с чужого склада
            }



            try
            {
                HttpResponseMessage responseContent = default;
                responseContent = await Context.postNewPkgOperationAsync(
                pack.PackageId,
                userId,
                operationType, // 1 подтвердить отправление в доставку . 2 подтвердить получение 
                actionStorageId);

                if ((int)responseContent.StatusCode == 200)
                {
                    if (operationType == 2)
                        MessageBox.Show("Посылка получена");
                    else
                        MessageBox.Show("Операция совершена успешно");

                    thisWindow.ShowViewPackageControlsPage_Storages(default, default);

                }
                else
                {
                    MessageBox.Show("Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка");
            }
        }
    }
}
