﻿using StorageApp.Model;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using BarcodeLib;
using System.Windows.Controls;
using System.Net.Http;


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
            filterBox.SelectedIndex = filterBox.Items.Count;


            filterBox.Visibility = Visibility.Collapsed;


            if (dataGrid.ItemsSource.GetType() == typeof(StorageApp.Model.Package[]))
            { 
                filterBox.Visibility = Visibility.Visible;
            }


            _collectionView = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
            _collectionView.Filter = FilterData;
        }

        private async void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dataGrid.ItemsSource.GetType() == typeof(StorageApp.Model.Package[]))
            {
                Model.Package package = dataGrid.SelectedItem as Model.Package;
                if (package == null)
                    return;

                int id = package.PackageId;
                Operation[] operations = Context.getOperations();
                string message = " ";
                foreach (Operation operation in operations.Where(x => x.PackageId == id))
                {
                    message += $"Тип: {Context.statusTranslate[operation.Type]}\tДата: {operation.OperationDate} {Context.getStorages().FirstOrDefault(x => x.storageId == operation.ActionstorageId).storageAddr} \n";
                }


                string SenderNumber = package.SenderNumber;
                string RecipientNumber = package.RecipientNumber;


                if (package.SenderNumber == " ")
                {
                    SenderNumber = "не указан";
                }
                if (package.RecipientNumber == " ")
                {
                    RecipientNumber = "не указан";
                }



                // Формируем текст для QR-кода
                string qrText = $@"=== ИНФОРМАЦИЯ О ПОСЫЛКЕ ===

                Вес: {package.Weight} {package.WeightUnit}
                Тип упаковки: {package.DimensionTitle}

                === ОТПРАВИТЕЛЬ ===
                ФИО: {package.senderFullName()}
                Телефон: {package.SenderNumber}
                Email: {package.SenderMail}

                === ПОЛУЧАТЕЛЬ ===
                ФИО: {package.recipientFullName()}
                Телефон: {package.RecipientNumber}
                Email: {package.RecipientMail}


                {message}

                ";







                ShowPackageInfo(qrText, package.PackageId.ToString());



                //string qrText = $@"=== ИНФОРМАЦИЯ О ПОСЫЛКЕ ===

                //Вес: {weight} {unitOfWeightComboBox.Text.Split(" ")[2]}
                //Тип упаковки: {dimensionName}

                //=== ОТПРАВИТЕЛЬ ===
                //ФИО: {senderLname} {senderFname} {senderSname}
                //Телефон: {senderNumber}
                //Email: {senderMail}

                //=== ПОЛУЧАТЕЛЬ ===
                //ФИО: {recipientLname} {recipientFname} {recipientSname}
                //Телефон: {recipientNumber}
                //Email: {recipientMail}
                //";


                //Context.GenerateAndSaveQRCodeAsPdf(qrText, @$"C:\qrcode_{}.pdf");

            }// ДАТАГРИД нажали на посылку

            else if (dataGrid.ItemsSource is IEnumerable<User> users) // Админ нажал на пользователя
            {
                Model.User user = dataGrid.SelectedItem as Model.User;

                var passwordDialog = new PasswordInputDialog();
                if (passwordDialog.ShowDialog() == true)
                {
                    string newPassword = passwordDialog.Password;

                    HttpResponseMessage responseContent = await Context.ChangeUserPasswordAsync(user.UserId, newPassword);
                    if ((int)responseContent.StatusCode == 200)
                    {
                        MessageBox.Show("Новый пароль сохранен");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка");
                    }
                }
            }

        }

        public class PasswordInputDialog : Window
        {
            public PasswordInputDialog()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.Title = "Изминение пароля";
                this.Width = 300;
                this.Height = 150;
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                var stackPanel = new StackPanel();
                var label = new Label { Content = "Введите пароль:" };
                var passwordBox = new PasswordBox { Margin = new Thickness(0, 10, 0, 10) };
                var button = new Button { Content = "OK", Width = 100, Margin = new Thickness(0, 10, 0, 0) };

                button.Click += (sender, e) =>
                {
                    this.DialogResult = true;
                    this.Close();
                };

                stackPanel.Children.Add(label);
                stackPanel.Children.Add(passwordBox);
                stackPanel.Children.Add(button);

                this.Content = stackPanel;

                passwordBox.Focus();
            }

            public string Password
            {
                get { return ((StackPanel)this.Content).Children.OfType<PasswordBox>().First().Password; }
            }
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            var columnsToHide = new List<string>
                {
                    "storageid",
                    "userid",
                    "actionstorageid",
                    "commandingstorageid",
                    "roleid",
                    "actionstorageId",
                    "commandingstorageId",
                    "typeid",
                    "enable",
                    "firstname",
                    "lastname",
                    "password"
                };

            if (columnsToHide.Contains(e.PropertyName.ToLower()))
            {
                e.Cancel = true; // отменяяяем
            }

            if (Context.propertiesTranslate.TryGetValue(e.PropertyName.ToLower(), out var header))
            {
                e.Column.Header = header;
            }
            else
            {
                e.Column.Header = "ERROR: " + e.PropertyName;
            }

        }

        private void SearchBox_MouseLeave(object sender, MouseEventArgs e)
        {
            _collectionView.Refresh();
        }

        private bool FilterData(object item)
        {
            // Фильтрация по статусу посылок 
            if (filterBox.SelectedItem != null && filterBox.SelectedItem is ComboBoxItem selectedStatus)
            {
                string status = selectedStatus.Content.ToString();

                if (status != "все" && item is Model.Package package && package.Status != status)
                    return false;
            }

            if (string.IsNullOrWhiteSpace(SearchBox.Text))
                return true;

            var searchText = SearchBox.Text.ToLower();
            var itemType = item.GetType();

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
                if (value.Contains(searchText))
                    return true;
            }

            return false;
        }

        public void ShowPackageInfo(string message , string packId)
        {
            // 1. Создаём окно
            var window = new Window
            {
                Title = "Информация о посылке",
                Width = 600,
                Height = 800,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow // Делаем модальным
            };

            // 2. Добавляем TextBlock с прокруткой
            var textBlock = new TextBlock
            {
                Text = message,
                FontSize = 18,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10)
            };

            var scrollViewer = new ScrollViewer
            {
                Content = textBlock,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            // 3. Добавляем кнопку для QR-кода
            var qrButton = new Button
            {
                Content = "Создать QR-код",
                Margin = new Thickness(10),
                Height = 30
            };


            //ВАЖНО убрал связь с message
            //ВАЖНО вернул на место


            qrButton.Click += (sender, e) =>
            {
                Context.GenerateAndSaveBarcodeAsPdf(packId, Context.MakeDockFilePath(@$"Barcode_{packId}.pdf") , message);
                MessageBox.Show("QR-код создан!");
            };

            // 4. Компоновка элементов
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(scrollViewer);
            stackPanel.Children.Add(qrButton);

            window.Content = stackPanel;
            window.ShowDialog();
        }

        private void filterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _collectionView.Refresh();
            }
            catch (Exception ex) { }

        }
    }

}
