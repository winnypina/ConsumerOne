﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ConsumerOne.Api
{
    public class DefaultDataProtectorTokenProviderOptions : DataProtectionTokenProviderOptions
    {

    }

    public class DefaultDataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public DefaultDataProtectorTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DefaultDataProtectorTokenProviderOptions> options) : base(dataProtectionProvider, options)
        {
        }
    }
}
