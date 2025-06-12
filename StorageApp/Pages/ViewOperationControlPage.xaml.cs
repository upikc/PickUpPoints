using Org.BouncyCastle.Utilities;
using StorageApp.UserControls;
using System;
using System.Collections.Generic;
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

namespace StorageApp.Pages
{
    public partial class ViewOperationControlPage : Page
    {
        private List<Model.Operation> _allOperations;
        private List<Model.Operation> _filteredAndSortedOperations;

        public ViewOperationControlPage(List<Model.Operation> allOperations)
        {
            InitializeComponent();

            filterBox.SelectedIndex = 4;
            sortBox.SelectedIndex = 1;

            if (allOperations is IEnumerable<Model.Operation> operations)
            {
                filterBox.Visibility = Visibility.Visible;
                _allOperations = operations.ToList();
                ApplyFiltersAndSorting();
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

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAndSorting();
        }

        private void ApplyFiltersAndSorting()
        {
            if (_allOperations == null)
                return;


            IEnumerable<Model.Operation> tempOperations = _allOperations;

            string searchText = SearchBox.Text.ToLower().Trim();
            string selectedStatus = (filterBox.SelectedItem as ComboBoxItem)?.Content.ToString().ToLower();


            if (!string.IsNullOrEmpty(searchText))
            {
                tempOperations = tempOperations.Where(operation =>
                {
                    string operationType = operation.Type?.ToLower() ?? string.Empty;
                    string operationDateStr = operation.OperationDate.ToString("dd.MM.yyyy").ToLower();
                    string userStr = operation.UserFullname.ToLower() ?? string.Empty;
                    string idString = operation.PackageId.ToString() ?? string.Empty;

                    return operationType.Contains(searchText) ||
                           operationDateStr.Contains(searchText) ||
                           userStr.Contains(searchText) ||
                           idString.Contains(searchText);
                });
            }

            if (selectedStatus != "все статусы" && !string.IsNullOrEmpty(selectedStatus))
            {
                tempOperations = tempOperations.Where(operation =>
                {
                    string status = operation.Type.ToLower();
                    string translatedStatus = Context.statusTranslate.ContainsKey(status) ? Context.statusTranslate[status] : status;
                    return translatedStatus == selectedStatus;
                });
            }

            string selectedSort = (sortBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (_allOperations == null)
                return;

            switch (selectedSort)
            {
                case "по дате ↑":
                    tempOperations = tempOperations.OrderBy(op => op.OperationDate);
                    break;
                case "по дате ↓":
                    tempOperations = tempOperations.OrderByDescending(op => op.OperationDate);
                    break;
                case "по фио ↑":
                    tempOperations = tempOperations.OrderBy(op => op.UserId);
                    break;
                case "по фио ↓":
                    tempOperations = tempOperations.OrderByDescending(op => op.UserId);
                    break;
                default:
                    tempOperations = tempOperations.OrderByDescending(op => op.OperationDate);
                    break;
            }

            _filteredAndSortedOperations = tempOperations.ToList();

            MainListView.Items.Clear();
            foreach (Model.Operation op in _filteredAndSortedOperations)
            {
                MainListView.Items.Add(new OperationControl(op));
            }
        }
    }
}