using System;

namespace ConsumerOne.Api.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string NameEnUs { get; set; }

        public string NameEs { get; set; }
    }
}
