using System.ComponentModel.DataAnnotations.Schema;

namespace simple_crud.Data.Entities
{
    public class File
    {
        public int ID { get; set; }

        public int NoveltyID { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public object Data { get; set; }
    }
}