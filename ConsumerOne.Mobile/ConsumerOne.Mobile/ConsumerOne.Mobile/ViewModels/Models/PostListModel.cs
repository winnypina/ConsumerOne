using System;
using System.Collections.Generic;
using System.Text;
using ConsumerOne.Mobile.Controls.VideoPlayerControl;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.ViewModels.Models
{
    public class PostListModel : MvxNotifyPropertyChanged
    {

        public PostListModel()
        {
            Images = new MvxObservableCollection<ImageSource>();
        }

        private double _likeCount;
        private bool _isLikedByUser;

        public Guid Id { get; set; }
        public ImageSource UserPicture { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string PostId { get; set; }
        public bool HasImage { get; set; }
        public bool HasVideo { get; set; }
        public MvxObservableCollection<ImageSource> Images { get; }
        public VideoSource VideoSource { get; set; }
        public string Price { get; set; }
        public string Distance { get; set; }
        public string Title { get; set; }
        public string StoreLink { get; set; }
        public string Description { get; set; }
        public double LikeCount
        {
            get => _likeCount;
            set => SetProperty(ref _likeCount,value);
        }

        public bool IsLikedByUser
        {
            get => _isLikedByUser;
            set => SetProperty(ref _isLikedByUser,value);
        }

        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string UserId { get; set; }
        public ImageSource Image { get; set; }
        public ImageSource QrCodeImageSource { get; set; }
    }
}
