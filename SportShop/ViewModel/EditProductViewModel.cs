﻿using SportShop.Model;
using SportShop.View;
using System;
using System.Collections.Generic;
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
    public class EditProductViewModel : INotifyPropertyChanged
    {
        private readonly string _connectionString = @"Data Source=EUGENE; DataBase=Var2Golkovaa; Integrated Security=True; Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true; encrypt=false;";
        private Товар _product;

        private string _артикул;
        private string _наименование;
        private string _ед_измерения;
        private decimal? _стоимость;
        private int? _размер_макс_скидки;
        private string _производитель;
        private string _поставщик;
        private int? _категория;
        private int? _действующая_скидка;
        private int? _колличество_на_складе;
        private string _описание;
        private string _изображение;

        public EditProductViewModel(Товар product)
        {
            _product = product;
            Артикул = product.Артикул;
            Наименование = product.Наименование;
            Ед_измерения = product.Ед_измерения;
            Стоимость = product.Стоимость;
            Размер_макс_скидки = product.Размер_макс_скидки;
            Производитель = product.Производитель;
            Поставщик = product.Поставщик;
            Категория = product.Категория;
            Действующая_скидка = product.Действущая_скидка;
            Колличество_на_складе = product.Колличество_на_складе;
            Описание = product.Описание;
            Изображение = product.Изображение;

            SaveProductCommand = new RelayCommand(SaveProduct);
        }

        public string Артикул
        {
            get => _артикул;
            set
            {
                _артикул = value;
                OnPropertyChanged();
            }
        }

        public string Наименование
        {
            get => _наименование;
            set
            {
                _наименование = value;
                OnPropertyChanged();
            }
        }

        public string Ед_измерения
        {
            get => _ед_измерения;
            set
            {
                _ед_измерения = value;
                OnPropertyChanged();
            }
        }

        public decimal? Стоимость
        {
            get => _стоимость;
            set
            {
                _стоимость = value;
                OnPropertyChanged();
            }
        }

        public int? Размер_макс_скидки
        {
            get => _размер_макс_скидки;
            set
            {
                _размер_макс_скидки = value;
                OnPropertyChanged();
            }
        }

        public string Производитель
        {
            get => _производитель;
            set
            {
                _производитель = value;
                OnPropertyChanged();
            }
        }

        public string Поставщик
        {
            get => _поставщик;
            set
            {
                _поставщик = value;
                OnPropertyChanged();
            }
        }

        public int? Категория
        {
            get => _категория;
            set
            {
                _категория = value;
                OnPropertyChanged();
            }
        }

        public int? Действующая_скидка
        {
            get => _действующая_скидка;
            set
            {
                _действующая_скидка = value;
                OnPropertyChanged();
            }
        }

        public int? Колличество_на_складе
        {
            get => _колличество_на_складе;
            set
            {
                _колличество_на_складе = value;
                OnPropertyChanged();
            }
        }

        public string Описание
        {
            get => _описание;
            set
            {
                _описание = value;
                OnPropertyChanged();
            }
        }

        public string Изображение
        {
            get => _изображение;
            set
            {
                _изображение = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveProductCommand { get; }

        private void SaveProduct(object parameter)
        {
            // Логика обновления товара в базе данных
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Товар
                    SET Артикул = @Артикул, Наименование = @Наименование, Ед_измерения = @Ед_измерения, Стоимость = @Стоимость, Размер_макс_скидки = @Размер_макс_скидки, Производитель = @Производитель, Поставщик = @Поставщик, Категория = @Категория, Действущая_скидка = @Действующая_скидка, Колличество_на_складе = @Колличество_на_складе, Описание = @Описание, Изображение = @Изображение
                    WHERE id_Товар = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Артикул", Артикул);
                    command.Parameters.AddWithValue("@Наименование", Наименование);
                    command.Parameters.AddWithValue("@Ед_измерения", Ед_измерения);
                    command.Parameters.AddWithValue("@Стоимость", Стоимость ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Размер_макс_скидки", Размер_макс_скидки ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Производитель", Производитель);
                    command.Parameters.AddWithValue("@Поставщик", Поставщик);
                    command.Parameters.AddWithValue("@Категория", Категория ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Действующая_скидка", Действующая_скидка ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Колличество_на_складе", Колличество_на_складе ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Описание", Описание);
                    command.Parameters.AddWithValue("@Изображение", Изображение ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Id", _product.id_Товар);
                    command.ExecuteNonQuery();
                }
            }

            // Закрыть окно после сохранения
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var shopWindow = new ShopWindow(UserService.CurrentUserId);
                mainWindow.MainContent.Content = shopWindow;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
