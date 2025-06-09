using StorageApp.Model;
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

namespace StorageApp.UserControls
{
    /// <summary>
    /// Логика взаимодействия для OperationControl.xaml
    /// </summary>
    public partial class OperationControl : UserControl
    {
        Model.Operation operation;
        public OperationControl(Model.Operation _operation)
        {
            operation = _operation;
            InitializeComponent();
            initialization();



        }

        public void initialization()
        {
            OperationIdText.Text = operation.OperationId.ToString();

            PackageIdText.Text = operation.PackageId.ToString();

            var user = Context.getUsers().First(x => x.UserId.ToString() == operation.UserId.ToString());

            UserIdText.Text = user.FirstName + " " + user.LastName;

            TypeText.Text = operation.Type.ToString();

            OperationDateText.Text = operation.OperationDate.ToString();

            CommandingStorageIdText.Text = operation.CommandingstorageId.ToString();
        }

        public Model.Operation GetOperation()
        {
            return operation;
        }
        public string GetAllName()
        {
            return UserIdText.Text;
        }
        public string GetDate()
        {
            return OperationDateText.Text;
        }
    }
}
