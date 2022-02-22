using Core.Enums;
using Core.Models;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace GroundhogWindows
{
    /// <summary>
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private static Regex dayOfYearReg = new Regex(@"^(?<mounth>\d\d).(?<day>\d\d)$");

        public Task Task { get; private set; }

        public TaskWindow(Task task)
        {
            InitializeComponent();

            comboBox.ItemsSource = Enum.GetValues(typeof(RepeatMode));

            if (task != null)
            {
                Task = task;

                textBoxText.Text = task.Text;
                comboBox.SelectedItem = task.RepeatMode;
                textBoxValue.Text = task.RepeatValue.ToString();
                checkBoxToNextDay.IsChecked = task.ToNextDay;
            }
            else
            {
                comboBox.SelectedItem = RepeatMode.Нет;
                Task = new Task();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxText.Text) || (RepeatMode)comboBox.SelectedItem != RepeatMode.Нет && string.IsNullOrWhiteSpace(textBoxValue.Text))
                    throw new Exception("Поля должны быть заполнены.");

                string value = GetRepeatValue();

                Task.Text = textBoxText.Text;
                Task.RepeatMode = (RepeatMode)comboBox.SelectedItem;
                Task.RepeatValue = value;
                Task.ToNextDay = checkBoxToNextDay.IsChecked.Value;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            textBoxValue.IsEnabled = (RepeatMode)comboBox.SelectedItem != RepeatMode.Нет;
        }

        private string GetRepeatValue()
        {
            string text = textBoxValue.Text;

            switch ((RepeatMode)comboBox.SelectedItem)
            {
                case RepeatMode.Дни:
                    int a;

                    if (!int.TryParse(text, out a))
                        throw new Exception("Неверное значение.");

                    if (a < 1)
                        throw new Exception("Неверное число дней.");

                    break;

                case RepeatMode.ЧислоМесяца:
                    int b;

                    if (!int.TryParse(text, out b))
                        throw new Exception("Неверное значение.");

                    if (b < 1)
                        throw new Exception("Неверное число дней.");

                    break;

                case RepeatMode.ДеньГода:
                    if (!dayOfYearReg.IsMatch(text))
                        throw new Exception("Неверный формат дня месяца: 'MM.dd'.");

                    GroupCollection groups = dayOfYearReg.Match(text).Groups;
                    int mounth;
                    int day;

                    if (!int.TryParse(groups["mounth"].Value, out mounth))
                        throw new Exception("Неверный номер месяца.");

                    if (!int.TryParse(groups["day"].Value, out day))
                        throw new Exception("Неверный день.");

                    if (day > DateTime.DaysInMonth(2020, mounth))
                        throw new Exception("В указанном месяце меньше дней.");

                    break;
            }

            return text;
        }
    }
}
