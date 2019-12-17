using System;
using System.Collections.Generic;
using System.Text;
using ConsumerOne.Mobile.Services.Requests;

namespace ConsumerOne.Mobile.ViewModels.Models
{
    public class CommentModel : CommentRequest
    {
        public string Since { get; set; }
        public string UserPicture { get; set; }
    }
}
