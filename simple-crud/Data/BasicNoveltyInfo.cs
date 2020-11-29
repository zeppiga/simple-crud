using System;
using simple_crud.Data.Entities;

namespace simple_crud.Data
{
    public sealed class BasicNoveltyInfo
    {
        public string Name { get; set; }

        public int Id { get; }

        public DateTime LastChanged { get; }

        public BasicNoveltyInfo(string name, int id, DateTime lastChanged)
        {
            Name = name;
            Id = id;
            LastChanged = lastChanged;
        }

        public BasicNoveltyInfo(Novelty novelty) : this(novelty.Name, novelty.ID, novelty.LastChanged)
        { }
    }
}
