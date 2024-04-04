using Microsoft.EntityFrameworkCore;
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
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Lab_DB_IndEx_MIA
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

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            NameQuery.Text = "";
            FrameForTableAndForm.Content = new PageEmployees();
        }

        private void PostsButton_Click(object sender, RoutedEventArgs e)
        {
            NameQuery.Text = "";
            FrameForTableAndForm.Content = new PagePosts();
        }

        private void TitlesButton_Click(object sender, RoutedEventArgs e)
        {
            NameQuery.Text = "";
            FrameForTableAndForm.Content= new PageTitles();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите закрыть окно?", "Проверка уверенности",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
        // Запросы на выборку
        // Запрос на сотрудников, родившихся в промежутке с 1 января 1989 по 10 апреля 2006
        private void ForSampling_Click(object sender, RoutedEventArgs e)
        {
            var dataGridNew = new DataGrid();
            NameQuery.Text = "Сотрудники, родившиеся с 1 января 1989 по 10 апреля 2006";
            FrameForTableAndForm.Content = dataGridNew;
            using (var context = new MIAContext())
            {
                var result = context.Empolyees
                    .Where(e => e.dateTime >= new DateTime(1964, 1, 1) && e.dateTime <= new DateTime(1964, 12, 10))
                    .ToList();
                if (result.Any())
                {
                    dataGridNew.ItemsSource = result;
                }
                else
                {
                    MessageBox.Show("Данных не найдено!", "Тоска", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }
        }

        //Вывести мужчин, чей возраст меньше 30
        private void ForSampling2_Click(object sender, RoutedEventArgs e)
        {
            var dataGridNew2 = new DataGrid();
            NameQuery.Text = "Мужчины, чей возраст меньше 30";
            FrameForTableAndForm.Content = dataGridNew2;
            using (var context2 = new MIAContext())
            {
                var result = context2.Empolyees
                    .Where(e => e.Age < 30)
                    .Select(g => new
                    {
                        ИД = g.Id,
                        Имя = g.FullName,
                        Лет = g.Age,
                    })
                    .ToList();
                if (result.Any())
                {
                    dataGridNew2.ItemsSource = result;
                }
                else
                {
                    MessageBox.Show("Данных не найдено!", "Тоска", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        // Запрос на средний возраст сотрудников
        private void StaticFunc_Click(object sender, RoutedEventArgs e)
        {

            var dataGridNew = new DataGrid();
            NameQuery.Text = "Средний возраст сотрудников на должностях";
            FrameForTableAndForm.Content = dataGridNew;
            using (var context = new MIAContext())
            {
                var query = from employee in context.Empolyees
                            join post in context.Posts on employee.IdPost equals post.IdPost
                            group employee by post.NamePost into g
                            select new
                            {
                                Должность = g.Key,
                                Возраст = g.Average(e=>e.Age)
                            };
                var result = query.ToList();
                if (result.Any())
                {
                    dataGridNew.ItemsSource = result;
                }
                else
                {
                    MessageBox.Show("Данных не найдено!", "Тоска", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        //Запрос на вывод всех сотрудников и их должностей с званиями
        private void JoinRequest_Click(object sender, RoutedEventArgs e)
        {
            var dataGridNew = new DataGrid();
            NameQuery.Text = "Вывод всех сотрудников с должностями и званиями";
            FrameForTableAndForm.Content = dataGridNew;
            using (var context = new MIAContext())
            {
                var query = from employee in context.Empolyees
                            join post in context.Posts on employee.IdPost equals post.IdPost
                            join title in context.Titles on employee.IdTitle equals title.IdTitle
                            select new
                            {
                                ИД = employee.Id, 
                                Имя = employee.FullName,
                                Должность = post.NamePost,
                                Звание = title.NameTitle,
                            };
                var result = query.ToList();
                if (result.Any())
                {
                    dataGridNew.ItemsSource = result;
                }
                else
                {
                    MessageBox.Show("Данных не найдено!", "Тоска", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }
    }
}
