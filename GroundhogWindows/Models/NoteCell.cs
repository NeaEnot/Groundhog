using System.Collections.Generic;

namespace GroundhogWindows.Models
{
    internal class NoteCell
    {
        internal NoteViewModel Note { get; set; }
        internal double Position { get; set; }
        internal string CurrentText
        {
            get => currentText;
            set
            {
                if (value == currentText)
                    return;

                if (redoedStates.Count > 0)
                    redoedStates.Clear();

                currentText = value;
                states.Push(currentText);
            }
        }

        internal bool CanDo => redoedStates.Count > 0;
        internal bool CanRedo => states.Count > 1;

        private string currentText;

        private Stack<string> states;
        private Stack<string> redoedStates;

        internal NoteCell(NoteViewModel note)
        {
            Note = note;
            currentText = note.Text;

            states = new Stack<string>();
            redoedStates = new Stack<string>();

            states.Push(currentText);
        }

        internal void Do()
        {
            currentText = redoedStates.Pop();
            states.Push(currentText);
        }

        internal void Redo()
        {
            redoedStates.Push(currentText);
            currentText = states.Pop();
        }
    }
}
