using Core.Interfaces.Storage;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageFile.Implements
{
    public class NoteLogic : INoteLogic
    {
        private Context context = Context.Instanse;

        public void Create(Note model)
        {
            model.Id = IdHelper.GetId("n_");
            context.Notes
                .Add(new Note
                {
                    Id = model.Id,
                    Name = model.Name,
                    Text = model.Text
                });

            context.Save();
        }

        public void Create(List<Note> models)
        {
            foreach (Note model in models)
            {
                if (string.IsNullOrEmpty(model.Id))
                    model.Id = IdHelper.GetId("n_");

                context.Notes
                    .Add(new Note
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Text = model.Text
                    });
            }

            context.Save();
        }

        public List<Note> Read()
        {
            return context.Notes
                .Select(req => new Note
                {
                    Id = req.Id,
                    Name = req.Name,
                    Text = req.Text
                })
                .ToList();
        }

        public void Update(Note model)
        {
            Note note = context.Notes.FirstOrDefault(req => req.Id == model.Id);

            if (note == null)
                throw new Exception("Заметки с данным Id не существует.");

            note.Name = model.Name;
            note.Text = model.Text;

            context.Save();
        }

        public void Delete(string id)
        {
            if (id == null)
            {
                context.Notes.Clear();
            }
            else
            {
                Note note = context.Notes.FirstOrDefault(req => req.Id == id);

                if (note == null)
                    throw new Exception("Заметки с данным Id не существует.");

                context.Notes.Remove(note);
            }

            context.Save();
        }
    }
}
