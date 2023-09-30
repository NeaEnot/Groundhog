using Core.Models.Storage;
using System.Windows;

namespace GroundhogDesktop.Views
{
    public partial class CommentWindow : Window
    {
        private CommentedElemet element;
        private string comment;

        public bool IsChanged => element.Comment != comment;

        public CommentWindow(CommentedElemet element)
        {
            InitializeComponent();

            this.element = element;
            comment = element.Comment;

            DataContext = element;
        }
    }
}
