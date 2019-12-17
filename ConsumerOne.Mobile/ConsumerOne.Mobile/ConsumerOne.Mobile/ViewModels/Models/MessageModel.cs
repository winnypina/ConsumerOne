using System;
using ConsumerOne.Mobile.Services;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.ViewModels.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }

        public string ToId { get; set; }
        public string FromId { get; set; }
        public string ToName { get; set; }
        public string FromName { get; set; }
        public DateTime SentDate { get; set; }
        public string Message { get; set; }

        public ImageSource UserPicture => ApiService.MediaBaseAddress + $"/{FromId}.jpg";

        public string Since
        {
            get
            {
                var span = (DateTime.UtcNow - SentDate.ToUniversalTime());

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
