using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ConsumerOne.Api.Models
{

    public enum UserType
    {
        Admin = 1,
        Provider = 2,
        Consumer = 3
    }

    public class AppUser : IdentityUser
    {
        // Extended Properties        
        public string Name { get; set; }
        public UserType Type { get; set; }
        public int Code { get; set; }
        public DateTime? BirthDate { get; set; }
        public string MobilePhone { get; set; }
        public long? FacebookId { get; set; }        
        public string DesiredLanguage { get; set; }
        public string About { get; set; }
        public string Website { get; set; }
        public string AddressNumber { get; set; }
        public string BusinessPhone { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }
        public string AddressAddon { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public ICollection<Post> Posts { get; set; }
    }

    public class Rating
    {
        [Key]
        public Guid Id { get; set; }

        public string ToId { get; set; }
        public string FromId { get; set; }
        public AppUser To { get; set; }
        public AppUser From { get; set; }
        public DateTime SentDate { get; set; }
        public int Score { get; set; }
        public string Message { get; set; }
    }

    public class UserMessage
    {
        [Key]
        public Guid Id { get; set; }

        public string ToId { get; set; }
        public string FromId { get; set; }
        public AppUser To { get; set; }
        public AppUser From { get; set; }
        public DateTime SentDate { get; set; }
        public string Message { get; set; }
    }

    public class Relation
    {

        [Key]
        public Guid Id { get; set; }
        
        public string FollowedId { get; set; }

        
        public string FollowerId { get; set; }
    }

   

}