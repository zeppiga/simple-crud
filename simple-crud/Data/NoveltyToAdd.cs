namespace simple_crud.Data
{
    public class NoveltyToAdd
    {
        public int? ID { get; }
        public string Name { get; }
        public string Description { get; }

        public NoveltyToAdd(string name, string description, int? id = null)
        {
            Name = name;
            Description = description;
            ID = id;
        }
    }
}
