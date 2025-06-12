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

            //если тип будет отправлен, нужно вывести адресс куда
            if(operation.Type.ToString() == "в пути на пвз")
            {
                TypeText.Text = "отправлена";
                transferAdress.Text = operation.StorageAddress;

            }
            else
            {
                transferAdressLABEL.Visibility = Visibility.Hidden;
                transferAdress.Visibility = Visibility.Hidden;
            }


            OperationDateText.Text = operation.OperationDate.ToString();

            CommandingStorageAddress.Text = operation.CommandingStorageAddress.ToString();
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
        private void CopyTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                Clipboard.SetText(textBlock.Text);
                MessageBox.Show("Текст скопирован в буфер обмена.");
            }
        }

        private void CopyTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                textBlock.Cursor = Cursors.Hand;
            }
        }

        private void CopyTextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                textBlock.Cursor = Cursors.Arrow;
            }
        }
    }
}
