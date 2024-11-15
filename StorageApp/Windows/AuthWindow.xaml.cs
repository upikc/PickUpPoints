using StorageApp.Windows;
using System.Diagnostics.Metrics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace StorageApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTbox.Text;
            string password = PassTbox.Text;


            var user = App.Context.UserEnterCheck(login , password);
            if (user == null )
            {
                MessageBox.Show("Данные не верны, повторите ввод");
            }
            else
            {
                new UserWindow(user).Show();
                this.Close();
            }
        }

        

    }
}