namespace CatMatch.Domain.Models
{
    public class Cat : RepositoryCollection
    {
        public string Url { get; set; }

        public string OriginalId { get; set; }

        public int Vote { get; set; }
    }
}
