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

                if (undoedStates.Count > 0)
                    undoedStates.Clear();

                if (savedIndex > states.Count)
                    savedIndex = -1;

                states.Push(currentText);
                currentText = value;
            }
        }

        internal bool CanDo => undoedStates.Count > 0;
        internal bool CanUndo => states.Count > 0;
        internal bool IsSaved => states.Count == savedIndex;

        private string currentText;
        private int savedIndex = 0;

        private Stack<string> states;
        private Stack<string> undoedStates;

        internal NoteCell(NoteViewModel note)
        {
            Note = note;
            currentText = note.Text;

            states = new Stack<string>();
            undoedStates = new Stack<string>();
        }

        internal void Do()
        {
            states.Push(currentText);
            currentText = undoedStates.Pop();
        }

        internal void Redo()
        {
            undoedStates.Push(currentText);
            currentText = states.Pop();
        }

        internal void Save()
        {
            savedIndex = states.Count;
        }
    }
}
