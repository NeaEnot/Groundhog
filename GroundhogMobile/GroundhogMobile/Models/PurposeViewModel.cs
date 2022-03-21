using Core.Models;
using Xamarin.Forms;

namespace GroundhogMobile.Models
{
    internal class PurposeViewModel
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string Text { get; set; }
        public bool Completed { get; set; }

        public string TextColor => Completed ? "Gray" : "Black";
        public TextDecorations TextDecorations => Completed ? TextDecorations.Strikethrough : TextDecorations.None;

        internal PurposeViewModel(Purpose purpose = null)
        {
            if (purpose != null)
            {
                Id = purpose.Id;
                GroupId = purpose.GroupId;
                Text = purpose.Text;
                Completed = purpose.Completed;
            }
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
