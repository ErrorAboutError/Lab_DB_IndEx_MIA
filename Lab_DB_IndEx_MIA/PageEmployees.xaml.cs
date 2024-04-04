using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для PageEmployees.xaml
    /// </summary>
    public partial class PageEmployees : Page
    {
        public MIAContext db = new MIAContext();
        public PageEmployees()
        {
            InitializeComponent();
            DataGridWithEmployees.ItemsSource = db.Empolyees.ToList();
        }

        // ДОБАВЛЕНИЕ ДАННЫХ В БД
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Text1.Text != "" && Text2.Text != "" && Text3.Text != "" && Text4.Text != "" && Text5.Text != "" && Text6.Text != "" && Text7.Text != ""
                && Text8.Text != "" && Text9.Text != "" && TextData.Text!="")
            {
                var dataBaseTMP = db.Empolyees.ToList();
                // Проверка ID
                try
                {
                    var tmp = Convert.ToInt64(Text1.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Вы неверно ввели ID!", "Вы совершаете ошибку!");
                    return;
                }
                foreach (var item in dataBaseTMP)
                {
                    if (item.Id == Math.Abs(Convert.ToInt64(Text1.Text)))
                    {
                        MessageBox.Show("Возможно вы ввели уже существующий Id!", "Злой");
                        return;
                    }
                }
                // Проверка возраста
                try
                {
                    var age = Convert.ToInt32(Text3.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Вы некорректно ввели возраст!", "Упс!");
                    return;
                }
                if (Convert.ToInt32(Text3.Text) < 18)
                {
                    MessageBox.Show("Вы ввели возраст меньше 18 лет!", "Зря!");
                    return;
                }
                // Проверка кода Должности
                try
                {
                    var tmp = Convert.ToInt64(Text8.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Вы неверно ввели Код должности!", "Вы совершаете ошибку!");
                    this.InvalidateVisual();
                    return;
                }
                var tmpPost = db.Posts.ToList();
                var tmpCount = 0;
                foreach (var item in tmpPost)
                {
                    if (item.IdPost == Math.Abs(Convert.ToInt64(Text8.Text)))
                    {
                        tmpCount++;
                    }
                    
                }
                if (tmpCount == 0)
                {
                    MessageBox.Show("Возможно вы ввели не существующий код должности!", "Злой");
                    return;
                }
                // Проверка кода звания
                try
                {
                    var tmp = Convert.ToInt64(Text9.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Вы неверно ввели код звания!", "Вы совершаете ошибку!");
                    this.InvalidateVisual();
                    return;
                }
                var tmpTitle = db.Titles.ToList();
                var tmpCount_2 = 0;
                foreach (var item in tmpTitle)
                {
                    if (item.IdTitle == Math.Abs(Convert.ToInt64(Text9.Text)))
                    {
                        tmpCount_2++;
                    }
                    
                }
                if (tmpCount_2 == 0)
                {
                    MessageBox.Show("Возможно вы ввели не существующий код звания!", "Злой");
                    return;
                }
                // Проврека даты
                try
                {
                    var date = Convert.ToDateTime(TextData.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Вы ввели неверную дату рождения сотрудника!", "Вы совершаете ошибку!");
                    return;
                }
               
                if (Convert.ToDateTime(TextData.Text) > DateTime.Now)
                {
                    MessageBox.Show("Вы ввели неверную дату рождения сотрудника!", "Вы совершаете ошибку!");
                    return;
                }              
               
                try
                {
                    db.Empolyees.Add(new Empolyee
                    {
                        Id = Math.Abs(Convert.ToInt64(Text1.Text)),
                        FullName = Text2.Text,
                        Age = Convert.ToInt32(Text3.Text),
                        Male = Text4.Text,
                        Address = Text5.Text,
                        Telephone = Text6.Text,
                        Pasport = Text7.Text,
                        IdPost = Convert.ToInt64(Text8.Text),
                        IdTitle = Convert.ToInt64(Text9.Text),
                        dateTime = Convert.ToDateTime(TextData.Text)

                    }); 

                    db.SaveChanges(); // Сохраняем изменения в БД
                    MessageBox.Show("Данные успешно внесены в БД!", "Опа!", MessageBoxButton.OK, MessageBoxImage.Information);
                    DataGridWithEmployees.Items.Refresh();
                    DataGridWithEmployees.ItemsSource = db.Empolyees.ToList();
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
                var selectedItems = DataGridWithEmployees.SelectedItems;

                var itemsToRemove = new List<object>(selectedItems.Cast<object>());

                foreach (var item in itemsToRemove)
                {
                    if (DataGridWithEmployees.ItemsSource is IList sourceList)
                    {
                        sourceList.Remove(item);
                        // Работа с БД
                        var selectedItem = DataGridWithEmployees.SelectedItem; //as BaseEntity
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
                           
                            DataGridWithEmployees.UpdateLayout();
                        }
                        catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex) { MessageBox.Show(ex.Message); }
                        // Работа с БД окончена
                    }
                    DataGridWithEmployees.Items.Refresh();
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

            var selectedEmployee = (Empolyee)DataGridWithEmployees.SelectedItem;
            if (selectedEmployee != null)
            {
                // Выведем данные в форму
                Text1.Text = Convert.ToString(selectedEmployee.Id);
                Text2.Text = Convert.ToString(selectedEmployee.FullName);
                Text3.Text = Convert.ToString(selectedEmployee.Age);
                Text4.Text = Convert.ToString(selectedEmployee.Male);
                Text5.Text = Convert.ToString(selectedEmployee.Address);
                Text6.Text = Convert.ToString(selectedEmployee.Telephone);
                Text7.Text = Convert.ToString(selectedEmployee.Pasport);
                Text8.Text = Convert.ToString(selectedEmployee.IdPost);
                Text9.Text = Convert.ToString(selectedEmployee.IdTitle);
                TextData.Text = Convert.ToString(selectedEmployee.dateTime);
            }
        }

       // Отрабатываем нажатие на кнопку Обновить (обновление данных в БД)
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите обновить данные?", "Проверка на уверенность",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (Text1.Text != "" && Text2.Text != "" && Text3.Text != "" && Text4.Text != "" && Text5.Text != "" && Text6.Text != "" && Text7.Text != ""
                    && Text8.Text != "" && Text9.Text != "" && TextData.Text !="")
                {
                    try
                    {
                        var tmp = Convert.ToInt64(Text1.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Вы неверно ввели ID!", "Вы совершаете ошибку!");
                        return;
                    }

                    try
                    {
                        var age = Convert.ToInt32(Text3.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Вы ввели неверный возраст!", "Упс");
                        return;
                    }
                    if (Convert.ToInt32(Text3.Text) < 18)
                    {
                        MessageBox.Show("Вы ввели возраст меньше 18 лет!", "Зря!");
                        return;
                    }
                    // Проверка кода Должности
                    try
                    {
                        var tmp = Convert.ToInt64(Text8.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Вы неверно ввели Код должности!", "Вы совершаете ошибку!");
                        this.InvalidateVisual();
                        return;
                    }
                    var tmpPost = db.Posts.ToList();
                    var tmpCount = 0;
                    foreach (var item in tmpPost)
                    {
                        if (item.IdPost == Math.Abs(Convert.ToInt64(Text8.Text)))
                        {
                            tmpCount++;
                        }

                    }
                    if (tmpCount == 0)
                    {
                        MessageBox.Show("Возможно вы ввели не существующий код должности!", "Злой");
                        return;
                    }
                    // Проверка кода звания
                    try
                    {
                        var tmp = Convert.ToInt64(Text9.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Вы неверно ввели код звания!", "Вы совершаете ошибку!");
                        this.InvalidateVisual();
                        return;
                    }
                    var tmpTitle = db.Titles.ToList();
                    var tmpCount_2 = 0;
                    foreach (var item in tmpTitle)
                    {
                        if (item.IdTitle == Math.Abs(Convert.ToInt64(Text9.Text)))
                        {
                            tmpCount_2++;
                        }

                    }
                    if (tmpCount_2 == 0)
                    {
                        MessageBox.Show("Возможно вы ввели не существующий код звания!", "Злой");
                        return;
                    }

                    try
                    {
                        var date = Convert.ToDateTime(TextData.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Вы ввели неверную дату рождения сотрудника!", "Вы совершаете ошибку!");
                        return;
                    }
                    if (Convert.ToDateTime(TextData.Text) > DateTime.Now)
                    {
                        MessageBox.Show("Вы ввели неверную дату рождения сотрудника!", "Вы совершаете ошибку!");
                        return;
                    }               

                    var employeeToUpdate = db.Empolyees.FirstOrDefault(c => c.Id == Convert.ToInt32(Text1.Text));
                    if (employeeToUpdate != null)
                    {
                        // Обновление данных 
                        employeeToUpdate.Id = Convert.ToInt64(Text1.Text);
                        employeeToUpdate.FullName = Text2.Text;
                        employeeToUpdate.Age = Convert.ToInt64(Text3.Text);
                        employeeToUpdate.Male = Text4.Text;
                        employeeToUpdate.Address  = Text5.Text;
                        employeeToUpdate.Telephone = Text6.Text;
                        employeeToUpdate.Pasport = Text7.Text;
                        employeeToUpdate.IdPost = Convert.ToInt64(Text8.Text);
                        employeeToUpdate.IdTitle  = Convert.ToInt64(Text9.Text);
                        employeeToUpdate.dateTime = Convert.ToDateTime(TextData.Text);
                        // Сохранение изменений в базе данных
                        db.SaveChanges();

                        MessageBox.Show("Данные успешно внесены в БД!", "Опа!", MessageBoxButton.OK, MessageBoxImage.Information);
                        Add.Visibility = Visibility.Visible; // Вскрываем кнопку добавления данных в БД
                        Add.IsEnabled = true; // Включаем кнопку Добавления, исключая случайное нажатие
                        Update.Visibility = Visibility.Collapsed; // Ура! Теперь кнопка Обновления невидимая
                        Update.IsEnabled = false; // И не работает
                        // Обновляем DataGrid
                        DataGridWithEmployees.Items.Refresh();
                        // Отчищаем поля ввода
                        // IEnumerable<TextBox> textBoxes = FindVisualChildren<TextBox>(pageEmployees);
                        Text1.Text = Text2.Text = Text3.Text = Text4.Text = Text5.Text = Text6.Text = Text7.Text = Text8.Text= Text9.Text = TextData.Text= "";
                    }
                    else
                    {
                       MessageBox.Show("Сотрудник не найден.");
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
