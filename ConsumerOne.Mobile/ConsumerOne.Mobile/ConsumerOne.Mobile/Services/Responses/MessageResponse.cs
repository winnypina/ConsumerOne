using System;
namespace ConsumerOne.Mobile.Services.Responses
{
    public class MessageResponse
    {
		public Guid Id { get; set; }

		public string ToId { get; set; }
		public string FromId { get; set; }
		public string ToName { get; set; }
		public string FromName { get; set; }
		public DateTime SentDate { get; set; }
		public string Message { get; set; }
	}
}
