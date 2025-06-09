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
        public ViewOperationControlPage(List<Model.Operation> allOperations)
        {
            InitializeComponent();

            filterBox.SelectedIndex = 4;

            if (allOperations is IEnumerable<Model.Operation> operations)
            {
                filterBox.Visibility = Visibility.Visible;
                _allOperations = operations.ToList();

                foreach (Model.Operation Opp in _allOperations)
                {
                    MainListView.Items.Add(new OperationControl(Opp));
                }
            }

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

            foreach (UserControls.OperationControl operationControl in MainListView.Items)
            {
                var operation = operationControl.GetOperation();

                var currentStorage = Context.getStorages().FirstOrDefault(x => x.storageId == operation.ActionstorageId);

                bool isTextMatch =
                    string.IsNullOrEmpty(searchText) ||
                    operationControl.GetAllName().ToString().Contains(searchText) ||
                    (operationControl.GetDate().ToLower().Contains(searchText));


                // фильтрация по статусу
                bool isStatusMatch = false;
                if (selectedStatus == "все статусы" || string.IsNullOrEmpty(selectedStatus))
                {
                    isStatusMatch = true;
                }
                else
                {
                    string status = operation.Type.ToLower();
                    string translatedStatus = Context.statusTranslate.ContainsKey(status) ? Context.statusTranslate[status] : status;

                    isStatusMatch = translatedStatus == selectedStatus;
                }
                operationControl.Visibility = (/*isTextMatch &&*/ isStatusMatch) ? Visibility.Visible : Visibility.Collapsed;


                ///скрыть элемент содержащий packageControl
                ListViewItem container = (ListViewItem)MainListView.ItemContainerGenerator.ContainerFromItem(operationControl);
                if (container != null)
                    container.Visibility = (/*isTextMatch &&*/ isStatusMatch) ? Visibility.Visible : Visibility.Collapsed;


            }
        }

    }
}
