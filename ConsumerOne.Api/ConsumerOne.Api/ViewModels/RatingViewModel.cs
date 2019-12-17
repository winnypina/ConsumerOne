using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerOne.Api.ViewModels
{
    public class RatingViewModel
    {
        public Guid Id { get; set; }

        public string ToId { get; set; }
        public string FromId { get; set; }
        public string ToName { get; set; }
        public string FromName { get; set; }
        public DateTime SentDate { get; set; }
        public int Score { get; set; }
        public string Message { get; set; }
    }
}
