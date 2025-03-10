using StorageApp.Model;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
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

namespace StorageApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для ViewDatagridPage.xaml
    /// </summary>
    public partial class ViewDatagridPage : Page
    {
        public ViewDatagridPage(System.Collections.IEnumerable objects)
        {
            InitializeComponent();

            dataGrid.ItemsSource = objects;

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (dataGrid.ItemsSource.GetType() == typeof(StorageApp.Model.Package[]))
            {
                Model.Package package = dataGrid.SelectedItem as Model.Package;
                int id = package.PackageId;
                Operation[] operations = Context.getOperations();
                string message = $"заказчик: {package.ClientFullname}, вес: {package.Weight}\n";
                foreach (Operation operation in operations.Where(x => x.PackageId == id))
                {
                    message += $"Тип: {Context.statusTranslate[operation.Type]}\tDate: {operation.OperationDate} \n";
                }



                MessageBox.Show(message);

            }
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "storageId":
                    e.Column.Header = "ID склада";
                    break;
                case "storageAddr":
                    e.Column.Header = "Адресс склада";
                    break;
                //
                case "PackageId":
                    e.Column.Header = "ID посылки";
                    break;
                case "Weight":
                    e.Column.Header = "Вес";
                    break;
                case "ClientFullname":
                    e.Column.Header = "Ф.И.О. Клиента";
                    break;
                case "ClientMail":
                    e.Column.Header = "Эл. почта Клиента";
                    break;
                case "ClientNumber":
                    e.Column.Header = "Т.номер Клиента";
                    break;
                case "Status":
                    e.Column.Header = "Статус";
                    break;
                case "StatusDate":
                    e.Column.Header = "Дата изминения статуса";
                    break;
                case "ActionstorageId":
                    e.Column.Header = "ID склада исполнителя";
                    break;
                //
                case "OperationId":
                    e.Column.Header = "Id операции";
                    break;
                case "UserId":
                    e.Column.Header = "Id пользователя";
                    break;
                case "Type":
                    e.Column.Header = "Тип операции";
                    break;
                case "OperationDate":
                    e.Column.Header = "Дата проведения операции";
                    break;
                case "TypeId":
                    e.Column.Header = "Id типа";
                    break;
                case "CommandingstorageId":
                    e.Column.Header = "id склада исполнителя";
                    break;
                //
                case "StorageId":
                    e.Column.Header = "Id склада";
                    break;
                case "RoleId":
                    e.Column.Header = "id роли";
                    break;
                case "Role":
                    e.Column.Header = "Роль";
                    break;
                case "Login":
                    e.Column.Header = "Логин";
                    break;
                case "Password":
                    e.Column.Header = "Пароль";
                    break;
                case "FirstName":
                    e.Column.Header = "Имя";
                    break;
                case "LastName":
                    e.Column.Header = "Фамилия";
                    break;
                case "PhoneNum":
                    e.Column.Header = "Номер Телефона";
                    break;
            }

        }
    }
}
