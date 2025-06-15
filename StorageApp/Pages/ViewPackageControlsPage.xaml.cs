using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StorageApp.UserControls;
using StorageApp.Model;
using MahApps.Metro.Controls;
using StorageApp.Windows;

namespace StorageApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewPackageControlsPage.xaml
    /// </summary>
    public partial class ViewPackageControlsPage : Page
    {
        private List<Model.Package> _allPackages;
        private List<Model.Package> _filteredAndSortedPackages; // Added for filtered and sorted packages
        public Model.User thisUser;
        public UserWindow thisWindow;

        public ViewPackageControlsPage(System.Collections.IEnumerable objects, Model.User _user, UserWindow _thisWindow)
        {
            InitializeComponent();
            thisUser = _user;
            thisWindow = _thisWindow;

            filterBox.SelectedIndex = 4;
            sortBox.SelectedIndex = 1; // Set a default sort order, e.g., "по дате ↓"

            filterBox.Visibility = Visibility.Collapsed;

            if (objects is IEnumerable<Model.Package> packages)
            {
                filterBox.Visibility = Visibility.Visible;
                _allPackages = packages.ToList(); // Store as a list directly
                ApplyFiltersAndSorting(); // Apply initial filters and sorting
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFiltersAndSorting();
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAndSorting();
        }

        // New event handler for sorting
        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAndSorting();
        }

        private void ApplyFiltersAndSorting() // Renamed from ApplyFilters
        {
            if (_allPackages == null)
                return;

            IEnumerable<Model.Package> tempPackages = _allPackages;

            string searchText = SearchBox.Text.ToLower().Trim();
            string selectedStatus = (filterBox.SelectedItem as ComboBoxItem)?.Content.ToString().ToLower();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchText))
            {
                tempPackages = tempPackages.Where(package =>
                {
                    var currentStorage = Context.getStorages().FirstOrDefault(x => x.storageId == package.ActionstorageId);

                    return package.PackageId.ToString().Contains(searchText) ||
                           (package.senderFullName()?.ToLower().Contains(searchText) ?? false) ||
                           (package.recipientFullName()?.ToLower().Contains(searchText) ?? false) ||
                           (package.SenderMail?.ToLower().Contains(searchText) ?? false) ||
                           (package.RecipientMail?.ToLower().Contains(searchText) ?? false) ||
                           (package.SenderNumber?.ToLower().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(searchText) ?? false) ||
                           (package.RecipientNumber?.ToLower().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(searchText) ?? false) ||
                           (package.Status?.ToLower().Contains(searchText) ?? false) ||
                           (package.DimensionTitle?.ToLower().Contains(searchText) ?? false) ||
                           package.Weight.ToString().Contains(searchText) ||
                           (currentStorage?.storageAddr?.ToLower().Contains(searchText) ?? false);
                });
            }

            // Apply status filter
            if (selectedStatus != "все статусы" && !string.IsNullOrEmpty(selectedStatus))
            {
                tempPackages = tempPackages.Where(package =>
                {
                    string status = package.Status?.ToLower();
                    string translatedStatus = Context.statusTranslate.ContainsKey(status) ? Context.statusTranslate[status] : status;
                    return translatedStatus == selectedStatus;
                });
            }

            // Apply sorting
            string selectedSort = (sortBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (selectedSort)
            {
                case "по дате ↑":
                    tempPackages = tempPackages.OrderBy(pack => pack.StatusDate);
                    break;
                case "по дате ↓":
                    tempPackages = tempPackages.OrderByDescending(pack => pack.StatusDate); // Assuming PackageDate exists
                    break;
                case "по фио ↑":
                    tempPackages = tempPackages.OrderBy(pack => pack.senderFullName()); // Sort by sender's full name
                    break;
                case "по фио ↓":
                    tempPackages = tempPackages.OrderByDescending(pack => pack.senderFullName()); // Sort by sender's full name
                    break;
                default:
                    tempPackages = tempPackages.OrderByDescending(pack => pack.StatusDate); // Default sort
                    break;
            }

            _filteredAndSortedPackages = tempPackages.ToList();

            // Clear and repopulate the ListView
            MainListView.Items.Clear();
            foreach (Model.Package pack in _filteredAndSortedPackages)
            {
                MainListView.Items.Add(new PackageControl(pack, thisUser, thisWindow));
            }
        }
    }
}