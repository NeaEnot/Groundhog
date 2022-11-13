﻿using Core;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GroundhogWindows
{
    public partial class NotePage : Page
    {
        private MainWindow windowContext;
        private Stack<Label> labels;

        public NotePage(MainWindow windowContext)
        {
            InitializeComponent();

            this.windowContext = windowContext;

            labels = new Stack<Label>();
        }

        public void LoadText()
        {
            if (windowContext.SelectedNote != null)
            {
                tbNote.Text = windowContext.SelectedNote.Text;
                tbNote.IsEnabled = true;

                btnSave.IsEnabled = true;
            }
            else
            {
                tbNote.Text = "";
                tbNote.IsEnabled = false;

                btnSave.IsEnabled = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            windowContext.SelectedNote.Text = tbNote.Text;
            GroundhogContext.NoteLogic.Update(windowContext.SelectedNote);
        }

        private void tbNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            Task.Run(() =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    int n = 0;
                    for (int i = tbNote.Text.Split('\n').Length; i > 0; i /= 10)
                        n++;

                    numbersColumn.Width = new GridLength(n > 1 ? n * 8 : 16);

                    while (labels.Count < tbNote.Text.Split('\n').Length)
                    {
                        Label label = new Label
                        {
                            Content = labels.Count + 1,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Padding = new Thickness(0, 0.66335, 4, 0),
                            FontSize = 11.5
                        };

                        labels.Push(label);
                        spNumbers.Children.Add(label);
                    }

                    while (labels.Count > tbNote.Text.Split('\n').Length)
                    {
                        Label label = labels.Pop();
                        spNumbers.Children.Remove(label);
                    }
                }));
            });
        }
    }
}
