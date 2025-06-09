using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StorageApp.UserControls; 
using StorageApp.Model;
using MahApps.Metro.Controls;

namespace StorageApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewPackageControlsPage.xaml
    /// </summary>
    public partial class ViewPackageControlsPage : Page
    {
        private List<Model.Package> _allPackages;

        public ViewPackageControlsPage(System.Collections.IEnumerable objects)
        {
            InitializeComponent();

            filterBox.SelectedIndex = 4;

            filterBox.Visibility = Visibility.Collapsed;

            if (objects is IEnumerable<Model.Package> packages)
            {
                filterBox.Visibility = Visibility.Visible;
                _allPackages = packages.ToList(); 

                foreach (Model.Package pack in _allPackages)
                {
                    MainListView.Items.Add(new PackageControl(pack));
                }
            }

            ApplyFilters();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string searchText = SearchBox.Text.ToLower().Trim();
            string selectedStatus = (filterBox.SelectedItem as ComboBoxItem)?.Content.ToString().ToLower();

            foreach (UserControls.PackageControl packageControl in MainListView.Items)
            {
                var package = packageControl.ThisPackage;

                var currentStorage = Context.getStorages().FirstOrDefault(x => x.storageId == package.ActionstorageId);
                var destinationStorage = Context.getStorages().FirstOrDefault(x => x.storageId == package.DestinationStorageId);

                bool isTextMatch =
                    string.IsNullOrEmpty(searchText) ||
                    package.PackageId.ToString().Contains(searchText) ||
                    (package.senderFullName()?.ToLower().Contains(searchText) ?? false) ||
                    (package.recipientFullName()?.ToLower().Contains(searchText) ?? false) ||
                    (package.SenderMail?.ToLower().Contains(searchText) ?? false) ||
                    (package.RecipientMail?.ToLower().Contains(searchText) ?? false) ||
                    (package.SenderNumber?.ToLower().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(searchText) ?? false) ||
                    (package.RecipientNumber?.ToLower().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(searchText) ?? false) ||
                    (package.Status?.ToLower().Contains(searchText) ?? false) ||
                    (package.DimensionTitle?.ToLower().Contains(searchText) ?? false) ||
                    package.Weight.ToString().Contains(searchText) ||
                    (currentStorage?.storageAddr?.ToLower().Contains(searchText) ?? false) ||
                    (destinationStorage?.storageAddr?.ToLower().Contains(searchText) ?? false);


                // фильтрация по статусу
                bool isStatusMatch = false;
                if (selectedStatus == "все статусы" || string.IsNullOrEmpty(selectedStatus))
                {
                    isStatusMatch = true;
                }
                else
                {
                    string status = package.Status?.ToLower();
                    string translatedStatus = Context.statusTranslate.ContainsKey(status) ? Context.statusTranslate[status] : status;

                    isStatusMatch = translatedStatus == selectedStatus;
                }
                packageControl.Visibility = (isTextMatch && isStatusMatch) ? Visibility.Visible : Visibility.Collapsed;


                ///скрыть элемент содержащий packageControl
                ListViewItem container = (ListViewItem)MainListView.ItemContainerGenerator.ContainerFromItem(packageControl);
                if (container != null)
                    container.Visibility = (isTextMatch && isStatusMatch) ? Visibility.Visible : Visibility.Collapsed;


            }






        }
    }
}