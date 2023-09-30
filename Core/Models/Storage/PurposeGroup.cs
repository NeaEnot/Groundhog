﻿namespace Core.Models.Storage
{
    public class PurposeGroup : CommentedElemet
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return (Id + Name).GetHashCode();
        }
    }
}
