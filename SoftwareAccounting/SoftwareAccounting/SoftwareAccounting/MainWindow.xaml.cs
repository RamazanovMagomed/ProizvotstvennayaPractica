using System;
using System.Windows;
using System.Windows.Controls;

namespace SoftwareAccounting
{
    public partial class MainWindow : Window
    {
        private SoftwareManager _softwareManager;
        private SoftwareItem _selectedItem;

        public MainWindow()
        {
            InitializeComponent();
            _softwareManager = new SoftwareManager();
            LoadData();
        }

        private void LoadData()
        {
            dgSoftware.ItemsSource = _softwareManager.GetAllItems();
            UpdateStatus($"Загружено записей: {_softwareManager.GetAllItems().Count}");
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtVersion.Text = "";
            cmbLicenseType.SelectedIndex = -1;
            txtManufacturer.Text = "";
            dpPurchaseDate.SelectedDate = DateTime.Now;
            txtCost.Text = "0";
            txtResponsible.Text = "";
            txtUserCount.Text = "1";
            dpExpirationDate.SelectedDate = DateTime.Now.AddYears(1);
            txtNotes.Text = "";
            _selectedItem = null;
        }

        private SoftwareItem GetFormData()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название ПО!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            if (!decimal.TryParse(txtCost.Text, out decimal cost))
            {
                MessageBox.Show("Некорректная стоимость!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            if (!int.TryParse(txtUserCount.Text, out int userCount))
            {
                MessageBox.Show("Некорректное количество пользователей!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            return new SoftwareItem
            {
                Id = _selectedItem?.Id ?? 0,
                Name = txtName.Text.Trim(),
                Version = txtVersion.Text.Trim(),
                LicenseType = (cmbLicenseType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "",
                Manufacturer = txtManufacturer.Text.Trim(),
                PurchaseDate = dpPurchaseDate.SelectedDate ?? DateTime.Now,
                Cost = cost,
                ResponsiblePerson = txtResponsible.Text.Trim(),
                UserCount = userCount,
                ExpirationDate = dpExpirationDate.SelectedDate ?? DateTime.Now.AddYears(1),
                Notes = txtNotes.Text.Trim()
            };
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var item = GetFormData();
            if (item != null)
            {
                _softwareManager.AddItem(item);
                LoadData();
                ClearForm();
                UpdateStatus("Запись добавлена");
                MessageBox.Show("ПО успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem == null)
            {
                MessageBox.Show("Выберите запись для обновления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var item = GetFormData();
            if (item != null)
            {
                item.Id = _selectedItem.Id;
                _softwareManager.UpdateItem(item);
                LoadData();
                ClearForm();
                UpdateStatus("Запись обновлена");
                MessageBox.Show("Запись успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить ПО '{_selectedItem.Name}' версии {_selectedItem.Version}?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _softwareManager.DeleteItem(_selectedItem.Id);
                LoadData();
                ClearForm();
                UpdateStatus("Запись удалена");
                MessageBox.Show("Запись успешно удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            UpdateStatus("Форма очищена");
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            UpdateStatus("Список обновлен");
        }

        private void DgSoftware_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItem = dgSoftware.SelectedItem as SoftwareItem;

            if (_selectedItem != null)
            {
                txtName.Text = _selectedItem.Name;
                txtVersion.Text = _selectedItem.Version;

                for (int i = 0; i < cmbLicenseType.Items.Count; i++)
                {
                    var item = cmbLicenseType.Items[i] as ComboBoxItem;
                    if (item != null && item.Content.ToString() == _selectedItem.LicenseType)
                    {
                        cmbLicenseType.SelectedIndex = i;
                        break;
                    }
                }

                txtManufacturer.Text = _selectedItem.Manufacturer;
                dpPurchaseDate.SelectedDate = _selectedItem.PurchaseDate;
                txtCost.Text = _selectedItem.Cost.ToString();
                txtResponsible.Text = _selectedItem.ResponsiblePerson;
                txtUserCount.Text = _selectedItem.UserCount.ToString();
                dpExpirationDate.SelectedDate = _selectedItem.ExpirationDate;
                txtNotes.Text = _selectedItem.Notes;
                UpdateStatus($"Выбрана запись ID: {_selectedItem.Id} - {_selectedItem.Name}");
            }
        }

        private void UpdateStatus(string message)
        {
            txtStatus.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
        }
    }
}