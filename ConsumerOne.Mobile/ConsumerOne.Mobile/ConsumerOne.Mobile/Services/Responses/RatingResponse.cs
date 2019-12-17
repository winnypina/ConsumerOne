using System;
using System.Collections.Generic;
using System.Text;

namespace ConsumerOne.Mobile.Services.Responses
{
    public class RatingResponse
    {

        public Guid Id { get; set; }

        public string ToId { get; set; }
        public string FromId { get; set; }
        public string ToName { get; set; }
        public string FromName { get; set; }
        public DateTime SentDate { get; set; }
        public int Score { get; set; }
        public string Message { get; set; }

        public string UserPicture => ApiService.MediaBaseAddress + $"/{FromId}.jpg";
        public string Since
        {
            get
            {
                var span = DateTime.UtcNow - SentDate.ToUniversalTime();

                var since = string.Empty;

                if (span.Days > 0)
                {
                    since += $"{span.Days}d";
                }
                else
                {
                    if (span.Hours > 0)
                    {
                        since += $"{span.Hours}h";
                    }
                    else
                    {
                        if (span.Minutes > 0)
                        {
                            since += $"{span.Minutes}m";
                        }
                        else
                        {
                            since += $"{span.Seconds}s";
                        }
                    }
                }

                return since;
            }
        }
    }
}
