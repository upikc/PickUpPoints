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

namespace StorageApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage(User user)
        {
            InitializeComponent();
            DataContext = user;


            Context.roleTranslate.TryGetValue(user.Role.ToLower(), out var transrole);

            RoleTbox.Text = transrole;


            if (user.RoleId == 2)
            {
                IEnumerable<Operation> userOperations = Context.getOperations().Where(x => x.UserId == user.UserId);

                DateTime now = DateTime.Now;

                TotalOperationsCountTbox.Text = userOperations.Count().ToString();

                DateTime monthStart = new DateTime(now.Year, now.Month, 1);
                MonthlyOperationsCountTbox.Text = userOperations.Count(o => o.OperationDate >= monthStart).ToString();

                DateTime weekStart = now.Date.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
                WeeklyOperationsCountTbox.Text = userOperations.Count(o => o.OperationDate >= weekStart).ToString();

                TotalOperationsCountTbox.Text = $"Всего операций: {userOperations.Count()}";
                MonthlyOperationsCountTbox.Text = $"За месяц: {userOperations.Count(o => o.OperationDate >= monthStart)}";
                WeeklyOperationsCountTbox.Text = $"За неделю: {userOperations.Count(o => o.OperationDate >= weekStart)}";
            }
            else
            {
                managerDataBox.Visibility = Visibility.Collapsed;
            }



        }
    }
}
