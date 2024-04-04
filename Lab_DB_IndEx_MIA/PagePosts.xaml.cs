using System;
using System.Collections;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Lab_DB_IndEx_MIA
{
    /// <summary>
    /// Логика взаимодействия для PagePosts.xaml
    /// </summary>
    public partial class PagePosts : Page
    {
        public MIAContext db = new MIAContext();
        public PagePosts()
        {
            InitializeComponent();
            DataGridWithPosts.ItemsSource = db.Posts.ToList();
        }

        // ДОБАВЛЕНИЕ ДАННЫХ В БД
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Text2.Text != "" && Text3.Text != "" && Text4.Text != "" && Text5.Text != "" && Text6.Text != "" )
            {
                var dataBaseTMP = db.Posts.ToList();
                try
                {
                    var tmp = Convert.ToInt64(Text2.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Вы неверно ввели Id!", "Зря!");
                    return;
                }
                foreach (var item in dataBaseTMP)
                {
                    if (item.IdPost == Math.Abs(Convert.ToInt64(Text2.Text)))
                    {
                        MessageBox.Show("Возможно вы ввели уже существующий IdPost!", "Злой");
                        return;
                    }
                }
                try
                {
                    var tmp = Convert.ToDouble(Text4.Text);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Вы ввели неверный оклад!", "Зря!");
                    return;
                }
               
                if (Convert.ToDouble(Text4.Text) < 0)
                {
                    MessageBox.Show("Вы ввели неверный оклад!", "Зря!");
                    return;
                }
               
                try
                {
                    db.Posts.Add(new Post
                    {
                        IdPost = Math.Abs(Convert.ToInt64(Text2.Text)),
                        NamePost = Text3.Text,
                        Salary = Convert.ToDouble(Text4.Text),
                        Responsibilities = Text5.Text,
                        Requirements = Text6.Text
                      
                    });
                   
                    db.SaveChanges(); // Сохраняем изменения в БД
                   
                    MessageBox.Show("Данные успешно внесены в БД!", "Опа!", MessageBoxButton.OK, MessageBoxImage.Information);
                    DataGridWithPosts.Items.Refresh();
                    DataGridWithPosts.ItemsSource = db.Posts.ToList();
                   

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка записи данных!", "Грусть");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ой!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // УДАЛЕНИЕ ДАННЫХ ИЗ БД ЧЕРЕЗ DATAGRID
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите удалить данные?", "Проверка на уверенность",
               MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedItems = DataGridWithPosts.SelectedItems;

                var itemsToRemove = new List<object>(selectedItems.Cast<object>());

                foreach (var item in itemsToRemove)
                {
                    if (DataGridWithPosts.ItemsSource is IList sourceList)
                    {
                        sourceList.Remove(item);
                        // Работа с БД
                        var selectedItem = DataGridWithPosts.SelectedItem; 
                        if (selectedItem == null) return;
                        try
                        {
                            db.RemoveRange(selectedItem);
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка изменения данных!", "Всё, конец", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                          
                            DataGridWithPosts.UpdateLayout();
                                                     
                        }
                        catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex) { MessageBox.Show(ex.Message); }
                        // Работа с БД окончена
                        DataGridWithPosts.Items.Refresh();
                    }
                    //DataGridWithPosts.Items.Refresh();
                }
            }
        }
        // ИЗМЕНЕНИЕ ДАННЫХ В БД ЧЕРЕЗ DATAGRID И ФОРМЫ
        private void UpdateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Add.Visibility = Visibility.Collapsed; // Скрываем кнопку добавления данных в БД
            Add.IsEnabled = false; // Отключаем кнопку Добавления, исключая случайное нажатие
            Update.Visibility = Visibility.Visible; // Ура! Теперь кнопка Обновления видимая
            Update.IsEnabled = true; // И работает

            var selectedPost = (Post)DataGridWithPosts.SelectedItem;
            if (selectedPost != null)
            {
                // Выведем данные в форму
                Text2.Text = Convert.ToString(selectedPost.IdPost);
                Text3.Text = Convert.ToString(selectedPost.NamePost);
                Text4.Text = Convert.ToString(selectedPost.Salary);
                Text5.Text = Convert.ToString(selectedPost.Responsibilities);
                Text6.Text = Convert.ToString(selectedPost.Requirements);
            }
        }

        // Отрабатываем нажатие на кнопку Обновить (обновление данных в БД)
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите обновить данные?", "Проверка на уверенность",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (Text2.Text != "" && Text3.Text != "" && Text4.Text != "" && Text5.Text != "" && Text6.Text != "" )
                {
                    try
                    {
                        var tmp = Convert.ToInt64(Text2.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Вы неверно ввели Id!", "Зря!");
                        return;
                    }
                    try
                    {
                        var tmp = Convert.ToDouble(Text4.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Вы  неверно ввели оклад!", "Зря!");
                        return;
                    }
                    if (Convert.ToDouble(Text4.Text) < 0)
                    {
                        MessageBox.Show("Вы ввели неверный оклад!", "Зря!");
                        return;
                    }
                                       
                    var employeeToUpdate = db.Posts.FirstOrDefault(c => c.IdPost == Convert.ToInt64(Text2.Text));
                    if (employeeToUpdate != null)
                    {
                        // Обновление данных 
                       
                        employeeToUpdate.IdPost = Convert.ToInt64(Text2.Text);
                        employeeToUpdate.NamePost = Text3.Text;
                        employeeToUpdate.Salary = Convert.ToDouble(Text4.Text);
                        employeeToUpdate.Responsibilities = Text5.Text;
                        employeeToUpdate.Requirements = Text6.Text;
                        // Сохранение изменений в базе данных
                        db.SaveChanges();

                        MessageBox.Show("Данные успешно внесены в БД!", "Опа!", MessageBoxButton.OK, MessageBoxImage.Information);
                        Add.Visibility = Visibility.Visible; // Вскрываем кнопку добавления данных в БД
                        Add.IsEnabled = true; // Включаем кнопку Добавления, исключая случайное нажатие
                        Update.Visibility = Visibility.Collapsed; // Ура! Теперь кнопка Обновления невидимая
                        Update.IsEnabled = false; // И не работает
                        // Обновляем DataGrid
                        DataGridWithPosts.Items.Refresh();
                        // Отчищаем поля ввода
                        // IEnumerable<TextBox> textBoxes = FindVisualChildren<TextBox>(pageEmployees);
                        Text2.Text = Text3.Text = Text4.Text = Text5.Text = Text6.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Должность не найдена!");
                    }
                }
                else
                {
                    MessageBox.Show("Заполните все поля!", "Ой!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

    }
}
