﻿using Core;
using Core.Interfaces.Storage;
using Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorageFile.Implements
{
    public class PurposeGroupLogic : IPurposeGroupLogic
    {
        private Context context = Context.Instanse;

        public void Create(PurposeGroup model)
        {
            model.Id = IdHelper.GetId("pg_");
            context.PurposeGroups
                .Add(new PurposeGroup
                {
                    Id = model.Id,
                    Name = model.Name,
                    Comment = model.Comment
                });

            context.Save();
        }

        public void Create(List<PurposeGroup> models)
        {
            foreach (PurposeGroup model in models)
            {
                if (string.IsNullOrEmpty(model.Id))
                    model.Id = IdHelper.GetId("pg_");

                context.PurposeGroups
                    .Add(new PurposeGroup
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Comment = model.Comment
                    });
            }

            context.Save();
        }

        public List<PurposeGroup> Read()
        {
            return context.PurposeGroups
                .Select(req => new PurposeGroup
                {
                    Id = req.Id,
                    Name = req.Name,
                    Comment = req.Comment
                })
                .ToList();
        }

        public void Update(PurposeGroup model)
        {
            PurposeGroup group = context.PurposeGroups.FirstOrDefault(req => req.Id == model.Id);

            if (group == null)
                throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {model.Id}.");

            group.Name = model.Name;
            group.Comment = model.Comment;

            context.Save();
        }

        public void Delete(string id)
        {
            if (id == null)
            {
                context.PurposeGroups.Clear();
            }
            else
            {
                PurposeGroup group = context.PurposeGroups.FirstOrDefault(req => req.Id == id);

                if (group == null)
                    throw new Exception($"{GroundhogContext.Language.ErrorsMessages.EntityWithSameIdDontExist}: {id}.");

                context.PurposeGroups.Remove(group);
            }

            context.Save();
        }
    }
}
