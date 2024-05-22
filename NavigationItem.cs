namespace Navigation_System.Models
{
    public class NavigationItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public string Url { get; set; }

        public NavigationItem Parent { get; set; }
        public ICollection<NavigationItem> Children { get; set; }
    }
}
