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

namespace Lab_DB_IndEx_MIA
{
    /// <summary>
    /// Логика взаимодействия для PageTitles.xaml
    /// </summary>
    public partial class PageTitles : Page
    {
        MIAContext db = new MIAContext();
        public PageTitles()
        {
            InitializeComponent();
            DataGridWithTitles.ItemsSource = db.Titles.ToList();
        }

        // ДОБАВЛЕНИЕ ДАННЫХ В БД
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Text2.Text != "" && Text3.Text != "" && Text4.Text != "" && Text5.Text != "" && Text6.Text != "")
            {
                var dataBaseTMP = db.Titles.ToList();
                try
                {
                    var tmp = Convert.ToInt64(Text2.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Вы неверно ввели ID!", "Зря!");
                    return;
                }

                foreach (var item in dataBaseTMP)
                {
                    if (item.IdTitle == Math.Abs(Convert.ToInt64(Text2.Text)))
                    {
                        MessageBox.Show("Возможно вы ввели уже существующий IdTitle!", "Злой");
                        return;
                    }
                }
                try
                {
                    var salary = Convert.ToDouble(Text4.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Вы ввели неверную надбавку!", "Упс");
                    return;
                }
                if (Convert.ToDouble(Text4.Text) < 0)
                {
                    MessageBox.Show("Вы неверно ввели надбавку!", "Зря!");
                    return;
                }
                
                try
                {
                    db.Titles.Add(new Title
                    {
                        IdTitle = Math.Abs(Convert.ToInt64(Text2.Text)),
                        NameTitle = Text3.Text,
                        Surcharge = Convert.ToDouble(Text4.Text),
                        Responsibilities = Text5.Text,
                        Requirements = Text6.Text

                    });

                    db.SaveChanges(); // Сохраняем изменения в БД
                    MessageBox.Show("Данные успешно внесены в БД!", "Опа!", MessageBoxButton.OK, MessageBoxImage.Information);
                    DataGridWithTitles.ItemsSource = db.Titles.ToList();
                    DataGridWithTitles.Items.Refresh();
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
                var selectedItems = DataGridWithTitles.SelectedItems;

                var itemsToRemove = new List<object>(selectedItems.Cast<object>());

                foreach (var item in itemsToRemove)
                {
                    if (DataGridWithTitles.ItemsSource is IList sourceList)
                    {
                        sourceList.Remove(item);
                        // Работа с БД
                        var selectedItem = DataGridWithTitles.SelectedItem;
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
                           
                            DataGridWithTitles.UpdateLayout();
                        }
                        catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex) { MessageBox.Show(ex.Message); }
                        // Работа с БД окончена
                        DataGridWithTitles.Items.Refresh();
                    }
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

            var selectedTitle = (Title)DataGridWithTitles.SelectedItem;
            if (selectedTitle != null)
            {
                // Выведем данные в форму
                Text2.Text = Convert.ToString(selectedTitle.IdTitle);
                Text3.Text = Convert.ToString(selectedTitle.NameTitle);
                Text4.Text = Convert.ToString(selectedTitle.Surcharge);
                Text5.Text = Convert.ToString(selectedTitle.Responsibilities);
                Text6.Text = Convert.ToString(selectedTitle.Requirements);
            }
        }

        // Отрабатываем нажатие на кнопку Обновить (обновление данных в БД)
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите обновить данные?", "Проверка на уверенность",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (Text2.Text != "" && Text3.Text != "" && Text4.Text != "" && Text5.Text != "" && Text6.Text != "")
                {
                    try
                    {
                        var tmp = Convert.ToInt64(Text2.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Вы неверно ввели ID!", "Зря!");
                        return;
                    }
                    try
                    {
                        var salary = Convert.ToDouble(Text4.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Вы ввели неверную надбавку!", "Упс");
                        return;
                    }
                    if (Convert.ToDouble(Text4.Text) < 0)
                    {
                        MessageBox.Show("Вы ввели неверную надбавка!", "Зря!");
                        return;
                    }
                   
                    var employeeToUpdate = db.Titles.FirstOrDefault(c => c.IdTitle == Convert.ToInt32(Text2.Text));
                    if (employeeToUpdate != null)
                    {
                        // Обновление данных 
                        employeeToUpdate.IdTitle = Convert.ToInt64(Text2.Text);
                        employeeToUpdate.NameTitle = Text3.Text;
                        employeeToUpdate.Surcharge = Convert.ToDouble(Text4.Text);
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
                        DataGridWithTitles.Items.Refresh();
                        // Отчищаем поля ввода
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
