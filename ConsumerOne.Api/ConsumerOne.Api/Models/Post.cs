using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GeoAPI.Geometries;

namespace ConsumerOne.Api.Models
{
    public class Post
    {
        [Key] public Guid Id { get; set; }
        public AppUser AppUser { get; set; }
        public int LikeCount { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }

        public virtual ICollection<PostLike> PostLikes { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public IPoint Location { get; set; }

        public string Tags { get; set; }

        public virtual List<PostMedia> PostMedias { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime PublishDate { get; set; }

        public bool  IsPaused { get; set; }
        public double? Price { get; set; }        
        public virtual List<PostReport> PostReports { get; set; }
        public string Address { get; set; }
        public string Currency { get; set; }
        public string StoreLink { get; set; }
        public string AppUserId { get; set; }
    }


    public class PostLike
    {
        [Key] public Guid Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }

    public class PostComment
    {
        [Key] public Guid Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public string Description { get; set; }

        public DateTime PublishDate { get; set; }
    }

    public class PostReport
    {
        [Key] public Guid Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public string Description { get; set; }
    }

    public enum MediaType
    {
        Image,
        Video
    }

    public class PostMedia
    {
        [Key] public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public MediaType MediaType { get; set; }
    }
}