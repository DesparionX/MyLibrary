namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface IReadable
    {
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int Pages { get; set; }
        public bool IsAvailable { get; set; }
    }
}
