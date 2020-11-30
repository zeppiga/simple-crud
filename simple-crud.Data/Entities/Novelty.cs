using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_crud.Data.Entities
{
    public interface INovelty
    {
        int ID { get; }
        string Name { get; }
        string Description { get; }
        int Version { get; }
        DateTime Created { get; }
        DateTime LastChanged { get; }
        ICollection<File> Files { get; }
    }

    public class Novelty : INovelty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Version { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastChanged { get; set; }

        //TODO

        public ICollection<File> Files { get; set; }
    }
}