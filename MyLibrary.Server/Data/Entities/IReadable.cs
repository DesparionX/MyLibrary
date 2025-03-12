namespace MyLibrary.Server.Data.Entities
{
    public interface IReadable
    {
        public string ISBN { get; }
        public string Genre { get; }
        public string Title { get; }
        public string Description { get; }
        public string Author { get; }
        public string Publisher { get; }
        public int Pages { get; }
    }
}
