﻿using System;
using Newtonsoft.Json;

namespace ConsumerOne.Mobile.Services.Responses
{
    public class LoginResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("auth_token")]
        public string AccessToken { get; set; }
    }
}