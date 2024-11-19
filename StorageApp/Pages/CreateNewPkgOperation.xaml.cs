﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Логика взаимодействия для CreateNewPkgOperation.xaml
    /// </summary>
    public partial class CreateNewPkgOperation : Page
    {
        public int userId = -1;
        public CreateNewPkgOperation(int userID , int storageId)
        {
            InitializeComponent();
            userId = userID;

            foreach (var p in App.Context.getPackagesFromStorage(storageId))
                Pkg_id.Items.Add(p.PackageId + " " + p.ClientFullname + " " + p.StatusDate);
            Pkg_id.SelectedIndex = 0;


            foreach (var s in App.Context.getStorages())
            {
                if (s.storageId != storageId)
                Storage_id.Items.Add(s.storageId + " " + s.storageAddr);
            }
            Storage_id.SelectedIndex = 0;

        }

        private async void AddBtnClick(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage responseContent2 = await AddPkgOperationAsync();
            var responseBody2 = await responseContent2.Content.ReadAsStringAsync();
            if ((int)responseContent2.StatusCode == 200)
                MessageBox.Show("Операция совершена успешно");
            else
                MessageBox.Show("Ошибка");
        }



        public async Task<HttpResponseMessage> AddPkgOperationAsync()
        {
            try
            {
                HttpResponseMessage responseContent = await App.Context.postNewPkgOperationAsync(
                    NumbBeforeSpace(Pkg_id.SelectedValue as string),
                    userId,
                    1,//отправка с склада
                    NumbBeforeSpace(Storage_id.SelectedValue as string));

                return responseContent;
            }
            catch (Exception ex)
            {
                return default(HttpResponseMessage);
            }
        }

        static int NumbBeforeSpace(string text)
        {
            return int.Parse(text.Split(' ', 2)[0]);
        }// перенести в ДАТАконтекст
    }
}