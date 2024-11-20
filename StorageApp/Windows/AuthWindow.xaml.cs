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
    public partial class AuthWindow : Window
    {
        public Model.User user;
        public AuthWindow()
        {
            InitializeComponent();
            this.Closed += appClose;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTbox.Text;
            string password = PassTbox.Text;


            user = Context.UserEnterCheck(login , password);
            if (user == null )
            {
                MessageBox.Show("Данные не верны, повторите ввод");
            }
            else
            {
                MessageBox.Show($"Добро пожаловать {user.FirstName} {user.LastName} вы зашли как {user.Role}");
                new UserWindow(user).Show();
                this.Close();
            }
        }
        private void appClose(object sender, EventArgs e)
        {
            if (user == null)
                Application.Current.Shutdown();
        }


    }
}