using SportShop.Model;
using SportShop.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SportShop.ViewModel
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Товар> _products;
        private readonly string _connectionString = @"Data Source=EUGENE; DataBase=Var2Golkovaa; Integrated Security=True; Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true; encrypt=false;";
        private int _userId;
        private bool _isAdmin;

        public ShopViewModel(int userId)
        {
            _userId = userId;
            _products = new ObservableCollection<Товар>();
            LoadProducts();
            CheckUserRole();

            AddProductCommand = new RelayCommand(OpenAddProductWindow, CanExecuteAdminCommand);
            EditProductCommand = new RelayCommand(OpenEditProductWindow, CanExecuteAdminCommand);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanExecuteAdminCommand);
            GoHomeNavigateCommand = new RelayCommand(GoHome);
        }

        public ObservableCollection<Товар> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand GoHomeNavigateCommand { get; }

        private void LoadProducts()
        {
            // Загрузка данных из базы данных
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        id_Товар, 
                        Артикул, 
                        Наименование, 
                        Ед_измерения, 
                        Стоимость, 
                        Размер_макс_скидки, 
                        Производитель, 
                        Поставщик, 
                        Категория, 
                        Действущая_скидка, 
                        Колличество_на_складе, 
                        Описание, 
                        Изображение
                    FROM 
                        Товар";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ObservableCollection<Товар> products = new ObservableCollection<Товар>();

                        while (reader.Read())
                        {
                            Товар product = new Товар
                            {
                                id_Товар = reader.GetInt32(0),
                                Артикул = reader.GetString(1),
                                Наименование = reader.GetString(2),
                                Ед_измерения = reader.GetString(3),
                                Стоимость = reader.IsDBNull(4) ? (decimal?)null : reader.GetDecimal(4),
                                Размер_макс_скидки = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                                Производитель = reader.GetString(6),
                                Поставщик = reader.GetString(7),
                                Категория = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8),
                                Действущая_скидка = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                                Колличество_на_складе = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10),
                                Описание = reader.GetString(11),
                                
                            };

                            products.Add(product);
                        }

                        Products = products;
                    }
                }
            }
        }

        private void CheckUserRole()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT 
                        Роль
                    FROM 
                        Пользователи
                    WHERE 
                        id_Пользователи = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", _userId);
                    var role = command.ExecuteScalar();
                    _isAdmin = role != null && (int)role == 1;
                }
            }
        }

        private bool CanExecuteAdminCommand(object parameter)
        {
            return _isAdmin;
        }

        private void OpenAddProductWindow(object parameter)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new AddProductWindow();
            }
        }

        private void OpenEditProductWindow(object parameter)
        {
            var product = parameter as Товар;
            if (product != null)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    var editWindow = new EditProductWindow(product);
                    mainWindow.MainContent.Content = editWindow;
                }
            }
        }

        private void DeleteProduct(object parameter)
        {
            var product = parameter as Товар;
            if (product != null)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                        DELETE FROM Товар
                        WHERE id_Товар = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", product.id_Товар);
                        command.ExecuteNonQuery();
                    }
                }

                Products.Remove(product);
            }
        }

        private void GoHome(object parameter)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new LoginWindow();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
