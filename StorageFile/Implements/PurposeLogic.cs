﻿using Core;
using Core.Interfaces.Storage;
using Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageFile.Implements
{
    public class PurposeLogic : IPurposeLogic
    {
        private Context context = Context.Instanse;

        public void Create(Purpose model)
        {
            model.Id = IdHelper.GetId("p_");
            context.Purposes
                .Add(new Purpose
                {
                    Id = model.Id,
                    GroupId = model.GroupId,
                    Text = model.Text,
                    Completed = model.Completed,
                    Comment= model.Comment
                });

            context.Save();
        }

        public void Create(List<Purpose> models)
        {
            foreach (Purpose model in models)
            {
                if (string.IsNullOrEmpty(model.Id))
                    model.Id = IdHelper.GetId("p_");

                context.Purposes
                    .Add(new Purpose
                    {
                        Id = model.Id,
                        GroupId = model.GroupId,
                        Text = model.Text,
                        Completed = model.Completed,
                        Comment= model.Comment
                    });
            }

            context.Save();
        }

        public List<Purpose> Read(string groupId)
        {
            return context.Purposes
                .Where(req => req.GroupId == groupId)
                .Select(req => new Purpose
                {
                    Id = req.Id,
                    GroupId = req.GroupId,
                    Text = req.Text,
                    Completed = req.Completed,
                    Comment = req.Comment
                })
                .ToList();
        }

        public void Update(Purpose model)
        {
            Purpose purpose = context.Purposes.FirstOrDefault(req => req.Id == model.Id);

            if (purpose == null)
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {model.Id}.");

            purpose.GroupId = model.GroupId;
            purpose.Text = model.Text;
            purpose.Completed = model.Completed;
            purpose.Comment = model.Comment;

            context.Save();
        }

        public void Delete()
        {
            context.Purposes.Clear();
            context.Save();
        }

        public void Delete(string id)
        {
            Purpose purpose = context.Purposes.FirstOrDefault(req => req.Id == id);

            if (purpose == null)
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {id}.");

            context.Purposes.Remove(purpose);

            context.Save();
        }

        public void Delete(List<string> ids)
        {
            IEnumerable<Purpose> purposes = context.Purposes.Where(req => ids.Contains(req.Id)).ToList();

            foreach (Purpose purpose in purposes)
                context.Purposes.Remove(purpose);

            context.Save();
        }
    }
}
