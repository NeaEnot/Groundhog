﻿using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TelegramImplement.Implements
{
    public class AccauntLogic : IAccauntLogic
    {
        private Context context = Context.Instanse;

        public void Create(Accaunt model)
        {
            if (context.Accaunts.Count(req => req.Name == model.Name) > 0)
            {
                throw new Exception("Аккаунт с таким именем уже существует.");
            }

            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.ConnetionString));
            model.Id = Convert.ToBase64String(hash);

            context.Accaunts
                .Add(new Accaunt
                {
                    Id = model.Id,
                    Name = model.Name,
                    ConnetionString = model.ConnetionString
                });

            context.Save();
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
            Accaunt accaunt = context.Accaunts.FirstOrDefault(req => req.Id == model.Id);

            if (accaunt == null)
            {
                throw new Exception("Аккаунта с данным Id не существует.");
            }

            accaunt.Name = model.Name;
            accaunt.ConnetionString = model.ConnetionString;

            context.Save();
        }

        public void Delete(string id)
        {
            Accaunt accaunt = context.Accaunts.FirstOrDefault(req => req.Id == id);

            if (accaunt == null)
            {
                throw new Exception("Аккаунта с данным Id не существует.");
            }

            context.Accaunts.Remove(accaunt);

            context.Save();
        }
    }
}