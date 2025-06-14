﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using StorageApp.Model;
using ControlzEx.Standard;
using System.Windows.Media;
using Org.BouncyCastle.Bcpg;
using System.Net.Http;
using System.Drawing;
using StorageApp.Windows;

namespace StorageApp.UserControls
{
    public partial class PackageControl : UserControl
    {
        public Model.Package ThisPackage { get; }
        public Model.User thisUser { get; }

        public UserWindow thisWindow { get; }

        public PackageControl(Model.Package package , User _user , UserWindow _thisWindow)
        {
            ThisPackage = package;
            thisUser = _user;
            thisWindow = _thisWindow;

            InitializeComponent();
            PopulatePackageData(package);
            PopulateOperationsHistory(package.PackageId);

            UpdateTextBlockColor(ThisPackage.Status);
        }

        private void PopulatePackageData(Model.Package package)
        {
            txtPackageId.Text = $"📦 Посылка #{package.PackageId}";
            txtWeight.Text = $"{package.Weight} {package.WeightUnit ?? ""}".Trim();
            txtSender.Text = FormatPersonInfo(
                package.senderFullName(),
                package.SenderMail,
                package.SenderNumber);
            txtRecipient.Text = FormatPersonInfo(
                package.recipientFullName(),
                package.RecipientMail,
                package.RecipientNumber);
            txtDimensions.Text = package.DimensionTitle ?? "Не указаны";

            string translatedStatus = TranslateStatus(package.Status);
            txtStatus.Text = $"{translatedStatus} (обновлен: {package.StatusDate:dd.MM.yyyy HH:mm})";

            var currentStorage = Context.getStorages().FirstOrDefault(x => x.storageId == package.ActionstorageId);
            txtStorage.Text = currentStorage != null ? currentStorage.storageAddr : "Не указан";

            var destinationStorage = Context.getStorages().FirstOrDefault(x => x.storageId == package.DestinationStorageId);
            txtDestinationStorage.Text = destinationStorage != null ? destinationStorage.storageAddr : "Не указан";

            //Кнопка действия с посылкой
            if (translatedStatus == "выдана" || thisUser.RoleId == 1) // кнопка не нужна
            {
                ActionBtn.Visibility = Visibility.Collapsed;
                return;
            }

            

        }

        private void PopulateOperationsHistory(int packageId)
        {
            OperationsHistoryPanel.Children.Clear();

            var operations = Context.getOperations().Where(x => x.PackageId == packageId).ToList();

            if (!operations.Any())
            {
                OperationsHistoryPanel.Children.Add(new TextBlock
                {
                    Text = "История операций отсутствует.",
                    FontSize = 24, 
                    Margin = new Thickness(0, 5, 0, 0),
                    FontStyle = FontStyles.Italic
                });
                return;
            }

            var sortedOperations = operations.OrderBy(op => op.OperationDate);

            foreach (var op in sortedOperations)
            {
                string operationTypeName = TranslateStatus(op.Type);
                if (string.IsNullOrEmpty(operationTypeName))
                {
                    operationTypeName = op.Type ?? "Неизвестный тип операции";
                }

                string storageAddress = "Неизвестный склад";
                var storage = Context.getStorages().FirstOrDefault(s => s.storageId == op.ActionstorageId);
                if (storage != null)
                {
                    storageAddress = storage.storageAddr;
                }

                var operationText = $"{op.OperationDate:dd.MM.yyyy HH:mm} - {operationTypeName} на складе: {storageAddress}";


                var textBlock = new TextBlock
                {
                    Text = operationText,
                    FontSize = 24,
                    Margin = new Thickness(0, 5, 0, 0),
                    TextWrapping = TextWrapping.Wrap 
                };
                OperationsHistoryPanel.Children.Add(textBlock);
            }
        }

        private string TranslateStatus(string? statusKey)
        {
            if (string.IsNullOrEmpty(statusKey))
            {
                return "Не указан";
            }
            if (Context.statusTranslate.TryGetValue(statusKey.ToLower(), out string? translatedValue))
            {
                return translatedValue;
            }
            return statusKey;
        }

        private string FormatPersonInfo(string fullName, string email, string phone)
        {
            var result = $"👤 {fullName}";

            if (!string.IsNullOrEmpty(email))
                result += $"\n📧 {email}";

            if (!string.IsNullOrEmpty(phone))
                result += $"\n📞 {FormatPhoneNumber(phone)}";
            else
                result += "\n📞 Не указан";

            return result;
        }

        private string FormatPhoneNumber(string phone)
        {
            return phone.Length == 11
                ? $"+{phone[0]} ({phone.Substring(1, 3)}) {phone.Substring(4, 3)}-{phone.Substring(7, 2)}-{phone.Substring(9)}"
                : phone; 
        }

        
        private void UpdateTextBlockColor(string status)
        {
            SolidColorBrush brush = null;

            switch (status)
            {
                case "создана":
                    brush = (SolidColorBrush)Application.Current.Resources["DeclareStatusColor"];
                    ActionBtn.Content = "Отправить";
                    break;
                case "в пути на пвз":
                    brush = (SolidColorBrush)Application.Current.Resources["TransferStatusColor"];
                    ActionBtn.Content = "Получить";
                    break;
                case "получена на пвз":
                    brush = (SolidColorBrush)Application.Current.Resources["ReceivedStatusColor"];
                    ActionBtn.Content = "Выдать";
                    break;
                case "выдана":
                    brush = (SolidColorBrush)Application.Current.Resources["IssueStatusColor"];
                    break;
            }

            if (brush != null)
            {
                txtPackageId.Background = brush;
            }
        }

        private void MakeBarcode(object sender, RoutedEventArgs e)
        {
            string message =
                $@"=== ИНФОРМАЦИЯ О ПОСЫЛКЕ ===

                Вес: {ThisPackage.Weight} {ThisPackage.WeightUnit}
                Тип упаковки: {ThisPackage.DimensionTitle}

                === ОТПРАВИТЕЛЬ ===
                ФИО: {ThisPackage.senderFullName()}
                Телефон: {(ThisPackage.SenderNumber == null ? "неуказан" : ThisPackage.SenderNumber)}
                Email: {ThisPackage.SenderMail}

                === ПОЛУЧАТЕЛЬ ===
                ФИО: {ThisPackage.recipientFullName()}
                Телефон: {(ThisPackage.RecipientNumber == null ? "неуказан" : ThisPackage.RecipientNumber)}
                Email: {ThisPackage.RecipientMail}";


            Context.GenerateAndSaveBarcodeAsPdf(ThisPackage.PackageId.ToString(), Context.MakeDockFilePath(@$"Barcode_{ThisPackage.PackageId}.pdf"), message);
            MessageBox.Show("Штрихкод создан!");

        }

        private void MakeReceipt(object sender, RoutedEventArgs e)
        {
            Context.GenerateAndSaveReceiptAsPdf(ThisPackage, Context.MakeDockFilePath(@$"Квинтация_{ThisPackage.PackageId}.pdf"));
            MessageBox.Show("Квитанция создана!");
        }

        public async Task MakeOperation(int operationType , int actionStorageId , int userId , int packId)
        {
            if (operationType == 1)
            {
                MessageBoxResult result = MessageBox.Show($"Посылка будет отправлена на {ThisPackage.DestinationStorageAddres}", "Внимание", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                    return;
            }
            if (operationType == 3)
            {
                MessageBoxResult result = MessageBox.Show($"Убедитесь что сверили данные получателя", "Внимание", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                    return;
            }

            try
            {
                HttpResponseMessage responseContent = default;
                responseContent = await Context.postNewPkgOperationAsync(
                packId,
                userId,
                operationType, // 1 подтвердить отправление в доставку . 2 подтвердить получение . 3 подтвердить выдачу
                actionStorageId); 
                
                if ((int)responseContent.StatusCode == 200)
                {
                    MessageBox.Show("Операция совершена успешно");
                    thisWindow.ShowViewPackageControlsPage_Storages(default, default);

                }
                else
                {
                    MessageBox.Show("Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка");
            }
        }

        private async void ActionBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (ThisPackage.Status)
            {
                case "создана":
                    await MakeOperation(1 , ThisPackage.DestinationStorageId , thisUser.UserId , ThisPackage.PackageId); //отправить
                    break;
                case "в пути на пвз":
                    await MakeOperation(2 , thisUser.StorageId , thisUser.UserId , ThisPackage.PackageId); //получить
                    break;
                case "получена на пвз":
                    await MakeOperation(3 , thisUser.StorageId , thisUser.UserId , ThisPackage.PackageId); //выдать
                    break;
                case "выдана":
                    break;
            }
        }
    }
}