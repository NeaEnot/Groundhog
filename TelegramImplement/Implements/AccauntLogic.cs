using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TelegramImplement.Implements
{
    public class AccauntLogic : IAccauntLogic
    {
        private Context context = Context.Instanse;

        public void Create(Accaunt model)
        {
            try
            {
                if (context.Accaunts.Count(req => req.Name == model.Name) > 0)
                {
                    throw new Exception("Аккаунт с таким именем уже существует.");
                }

                model.Id = Guid.NewGuid().ToString();
                context.Accaunts.Add(model);

                context.Save();
            }
            catch
            {
                throw;
            }
        }

        public List<Accaunt> Read()
        {
            return
                context.Accaunts
                .Select(req => new Accaunt
                {
                    Id = req.Id,
                    Name = req.Name,
                    ConnetionString = req.ConnetionString
                })
                .ToList();
        }

        public void Update(Accaunt model)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
