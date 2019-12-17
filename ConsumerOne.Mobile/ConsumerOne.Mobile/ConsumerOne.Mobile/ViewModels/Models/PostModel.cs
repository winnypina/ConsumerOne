using System;
using System.Collections.Generic;

namespace ConsumerOne.Mobile.ViewModels.Models
{
    public class PostModel
    {
        public PostModel()
        {
            Images = new List<byte[]>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public double Value { get; set; }
        public string Currency { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string StoreLink { get; set; }

        public string Address { get; set; }

        public List<byte[]> Images { get; set; }

        public byte[] Video { get; set; }

    }
}
