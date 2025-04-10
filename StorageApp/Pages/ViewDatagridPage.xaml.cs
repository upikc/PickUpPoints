using StorageApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private ICollectionView _collectionView;
        public ViewDatagridPage(System.Collections.IEnumerable objects)
        {
            InitializeComponent();

            dataGrid.ItemsSource = objects;

            _collectionView = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
            _collectionView.Filter = FilterData;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (dataGrid.ItemsSource.GetType() == typeof(StorageApp.Model.Package[]))
            {
                Model.Package package = dataGrid.SelectedItem as Model.Package;
                if (package == null)
                    return;


                int id = package.PackageId;
                Operation[] operations = Context.getOperations();
                string message = $"заказчик: {package.ClientFullname}, вес в центнерах: {package.Weight}\n";
                foreach (Operation operation in operations.Where(x => x.PackageId == id))
                {
                    message += $"Тип: {Context.statusTranslate[operation.Type]}\tDate: {operation.OperationDate} {Context.getStorages().FirstOrDefault(x => x.storageId == operation.ActionstorageId)} \n";
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
                    e.Column.Header = "Адрес склада";
                    break;
                //
                case "PackageId":
                    e.Column.Header = "ID посылки";
                    break;
                case "Weight":
                    e.Column.Header = "вес в центнерах";
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
                    e.Column.Header = "Дата изменения статуса";
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

        private void SearchBox_MouseLeave(object sender, MouseEventArgs e)
        {
            _collectionView.Refresh();
        }

        private bool FilterData(object item)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
                return true;

            var searchText = SearchBox.Text.ToLower();
            var itemType = item.GetType();

            // Проверяем все видимые столбцы
            foreach (DataGridColumn column in dataGrid.Columns)
            {
                if (column.Visibility != Visibility.Visible)
                    continue;

                var binding = (column as DataGridBoundColumn)?.Binding as Binding;
                var propName = binding?.Path.Path;
                if (string.IsNullOrEmpty(propName))
                    continue;

                var prop = itemType.GetProperty(propName);
                if (prop == null)
                    continue;

                var value = prop.GetValue(item)?.ToString()?.ToLower() ?? "";
                if (value.Contains(searchText.ToLower()))
                    return true;
            }

            return false;
        }
    }
}
