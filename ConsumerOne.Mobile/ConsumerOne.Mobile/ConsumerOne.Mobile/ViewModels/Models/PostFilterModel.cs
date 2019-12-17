namespace ConsumerOne.Mobile.ViewModels.Models
{

    public enum SearchType
    {
        Posts = 1,
        Providers = 2
    }

    public enum OrderType
    {
        Date = 1,
        Distance = 2
    }

    public class PostFilterModel
    {
        public string Term { get; set; }
        public SearchType SearchType { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int Distance { get; set; }
        public OrderType OrderType { get; set; }
        public string Address { get; set; }
    }
}