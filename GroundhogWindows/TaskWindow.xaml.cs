using Core.Enums;
using Core.Models;
using System;
using System.Windows;

namespace GroundhogWindows
{
    /// <summary>
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
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

                int value = 0;
                if ((RepeatMode)comboBox.SelectedItem != RepeatMode.Нет)
                {
                    if (!int.TryParse(textBoxValue.Text, out value))
                        throw new Exception("Неверное значение.");

                    if ((RepeatMode)comboBox.SelectedItem == RepeatMode.ЧислоМесяца && (value < 1 || value > 31))
                        throw new Exception("Неверное число месяца.");

                    if (value < 1)
                        throw new Exception("Неверное число дней.");
                }

                Task.Text = textBoxText.Text;
                Task.RepeatMode = (RepeatMode)comboBox.SelectedItem;
                Task.RepeatValue = value;
                Task.AccauntId = App.Accaunt.Id;

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
    }
}
