using StorageApp.Model;
using StorageApp.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Shapes;

namespace StorageApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public User User = default;
        public UserWindow(User thisUser)
        {
            InitializeComponent();
            this.Closed += ThisWindow_Closed;
            User = thisUser;
            this.Title = $"{thisUser.FirstName} {thisUser.LastName} роль: {Context.roleTranslate[thisUser.Role]}";
            
            if (thisUser.RoleId == 1)
                StorekeeperStackPanel.Visibility = Visibility.Hidden;
            else
                ManagerStackPanel.Visibility = Visibility.Hidden;

        }

        private void ShowUserProfilePage(object sender, EventArgs e)
        {
            mainFrame.Content = new ProfilePage(User);
        }


        private void ShowManagerProfilePage(object sender, EventArgs e)
        {
            mainFrame.Content = new ProfilePage(User);
        }

        private void ThisWindow_Closed(object sender, EventArgs e)
        {
            var window = new AuthWindow();
            window.Show();
        }
        private void ShowViewDatagridPage_Storages(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new ViewDatagridPage(Context.getStorages());
        }
        private void ShowViewDatagridPage_Pkg(object sender, MouseButtonEventArgs e)
        {
            PKGSHOW();
        }

        public void PKGSHOW()
        {
            mainFrame.Content = new ViewUserControlsPage(Context.getPackagesFromStorage(User.StorageId).Select(x => { x.Status = Context.statusTranslate[x.Status.ToLower()]; return x; }).ToArray());
        }
        private void ShowViewDatagridPage_Operation(object sender, MouseButtonEventArgs e)
        {
            //если админ
            if (User.RoleId == 1)
            mainFrame.Content = new ViewDatagridPage(
            Context.getOperations().Select(x =>
            {
                if (Context.statusTranslate != null && Context.statusTranslate.TryGetValue(x.Type.ToLower(), out var translatedStatus))
                {
                    x.Type = translatedStatus;
                }
                return x;
            }));

            else
            //если оператор пвз
            mainFrame.Content = new ViewDatagridPage(
            Context.getOperations().Where(x => x.ActionstorageId == User.StorageId || x.CommandingstorageId == User.StorageId).Select(x =>
            {
                if (Context.statusTranslate != null && Context.statusTranslate.TryGetValue(x.Type.ToLower(), out var translatedStatus))
                {
                    x.Type = translatedStatus;
                }
                return x;
            })
);
        }
        private void ShowViewDatagridPage_Users(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new ViewDatagridPage(Context.getUsers().Select(x => { x.Role = Context.roleTranslate[x.Role.ToLower()]; x.Password = "****"; return x; }));
        }

        private void ShowCreateNewStoragePage(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new CreateNewStoragePage();
        }
        private void ShowCreateNewUserPage(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new CreateNewUserPage();
        }
        private void ShowCreateNewPackagePage(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new CreateNewPackagePage(User.UserId , this);
        }
        private void ShowCreateNewPkgOperatioPage(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new CreateNewPkgOperation(User.UserId , User.StorageId);
        }

        //============================================= 

        private void ShowViewDatagridPage_StoragesFromMyStorage(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new ViewDatagridPage(Context.getPackagesFromStorage(User.StorageId).Select(x => { x.Status = Context.statusTranslate[x.Status.ToLower()]; return x; }).ToArray());
        }

        private void ShowViewUserControlsPage_StoragesFromMyStorage(object sender, MouseButtonEventArgs e)
        {

            mainFrame.Content = new ViewUserControlsPage(Context.getPackagesFromStorage(User.StorageId).Select(x => { x.Status = Context.statusTranslate[x.Status.ToLower()]; return x; }).ToArray());
            
        }

        private void ShowViewUserControlsPage_Storages(object sender, MouseButtonEventArgs e)//админ
        {
                mainFrame.Content = new ViewUserControlsPage(Context.getPackages().ToArray());
        }




        
        private void ShowViewDatagridPage_OperationFromMyStorage(object sender, MouseButtonEventArgs e)//по складу
        {
            mainFrame.Content = new ViewDatagridPage(Context.getOperations().Select(x => { x.Type = Context.statusTranslate[x.Type.ToLower()]; return x; }).ToArray());
        }
        private void ShowСonfirmReceiptPage(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new СonfirmReceiptPage(User);
        }
        private void ShowConfirmIssuePage(object sender, MouseButtonEventArgs e)
        {
            mainFrame.Content = new ConfirmIssuePage(User);
        }
    }
}
