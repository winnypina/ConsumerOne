﻿using System;

namespace ConsumerOne.Mobile.Services.Responses
{

    public enum UserType
    {
        Admin = 1,
        Provider = 2,
        Consumer = 3 
    }

    public class AccountResponse
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public UserType Type { get; set; }
        public int Code { get; set; }
        public string Email { get; set; }
        public string AddressNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string MobilePhone { get; set; }
        public long? FacebookId { get; set; }
        public string DesiredLanguage { get; set; }
        public string About { get; set; }
        public string Website { get; set; }

        public string BusinessPhone { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }
        public string AddressAddon { get; set; }
        public string Country { get; set; }

        public int Followers { get; set; }
        public bool IsFollowing { get; set; }

        public double Rating { get; set; }

        public double? MyRating { get; set; }

        public string UserPicture => ApiService.MediaBaseAddress + $"/{Id}.jpg";

       
    }
}