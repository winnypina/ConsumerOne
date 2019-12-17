using System;
using System.Collections.Generic;

namespace ConsumerOne.Api.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            Images = new List<Guid>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public double Value { get; set; }
        public string Currency { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string StoreLink { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public bool IsLikedByUser { get; set; }
        public int LikeCount { get; set; }
        public DateTime PublishDate { get; set; }

        public List<Guid> Images { get; set; }

        public string Video { get; set; }
    }
}