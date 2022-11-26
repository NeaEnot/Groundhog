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

        internal bool CanRedo => undoedStates.Count > 0;
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

        internal int? Redo()
        {
            string stored = currentText;

            states.Push(currentText);
            currentText = undoedStates.Pop();

            return FirstOccurence(stored, currentText);
        }

        internal int? Undo()
        {
            string stored = currentText;

            undoedStates.Push(currentText);
            currentText = states.Pop();

            return FirstOccurence(stored, currentText);
        }

        internal void Save()
        {
            savedIndex = states.Count;
        }

        private static int? FirstOccurence(string str1, string str2)
        {
            if (ReferenceEquals(str1, str2))
                return null;

            int? startIndex = null;
            int? endIndex = null;

            for (int i = 0; i < str1.Length && i < str2.Length; i++)
            {
                if (str1[i] != str2[i])
                {
                    startIndex = i;
                    break;
                }
            }

            for (int i = 1; str1.Length - i >= 0 && str2.Length - i >= 0; i++)
            {
                if (str1[str1.Length - i] != str2[str2.Length - i])
                {
                    endIndex = str1.Length - i;
                    break;
                }
            }

            if (startIndex == null && endIndex == null)
                return null;
            if (startIndex == null && endIndex != null)
                return str2.Length;
            if (startIndex != null && endIndex == null)
            {
                if (str1.Length > str2.Length)
                    return startIndex;
                else
                    return str2.Length - str1.Length;
            }
            if (startIndex != null && endIndex != null)
            {
                if (str1.Length > str2.Length)
                    return startIndex;
                else if (str1.Length < str2.Length)
                    return str2.Length - str1.Length + startIndex;
                else
                    return startIndex + 1;
            }

            return null;
        }
    }
}
