using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_crud.Data.Entities
{
    public class Novelty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Version { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastChanged { get; set; }

        public ICollection<File> Files { get; set; }
    }
}