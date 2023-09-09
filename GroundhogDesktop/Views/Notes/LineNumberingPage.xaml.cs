using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WindowsDesktop.Views.Notes
{
    public partial class LineNumberingPage : Page
    {
        private Stack<Label> labels;

        internal LineNumberingPage()
        {
            InitializeComponent();
            labels = new Stack<Label>();
        }

        internal void GenerateNumbering(int count)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    SetWidth(count);

                    while (labels.Count < count)
                    {
                        Label label = CreateLabel();

                        labels.Push(label);
                        spNumbers.Children.Add(label);
                    }

                    while (labels.Count > count)
                    {
                        Label label = labels.Pop();
                        spNumbers.Children.Remove(label);
                    }
                }));
            });
        }

        private void SetWidth(int count)
        {
            int n = 0;
            for (int i = count; i > 0; i /= 10)
                n++;

            Width = n > 1 ? n * 10 : 20;
        }

        private Label CreateLabel()
        {
            return new Label
            {
                Content = labels.Count + 1,
                HorizontalAlignment = HorizontalAlignment.Right,
                Padding = new Thickness(0, 0, 4, 0), //0.66335
                FontSize = 13
            };
        }
    }
}
