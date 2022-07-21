using Core.Models;

namespace GroundhogWindows.Models
{
    internal class PurposeViewModel
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string Text { get; set; }
        public bool Completed { get; set; }

        public string TextColor => Completed ? App.Current.Resources["Additional text"].ToString(): App.Current.Resources["Main text"].ToString();
        public string TextDecorations => Completed ? "Strikethrough" : "None";

        internal PurposeViewModel(Purpose purpose)
        {
            Id = purpose.Id;
            GroupId = purpose.GroupId;
            Text = purpose.Text;
            Completed = purpose.Completed;
        }

        internal Purpose Convert()
        {
            return new Purpose
            {
                Id = Id,
                GroupId = GroupId,
                Text = Text,
                Completed = Completed
            };
        }
    }
}
