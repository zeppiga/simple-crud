using System;
using simple_crud.Data.Entities;

namespace simple_crud.Data
{
    public sealed class NoveltyInfo
    {
        public string Name { get; }

        public int Id { get; }

        public DateTime LastChanged { get; }

        public NoveltyInfo(string name, int id, DateTime lastChanged)
        {
            Name = name;
            Id = id;
            LastChanged = lastChanged;
        }

        public NoveltyInfo(Novelty novelty) : this(novelty.Name, novelty.ID, novelty.LastChanged)
        { }
    }
}
