using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ConsumerOne.Api.Auth;
using ConsumerOne.Api.Data;
using ConsumerOne.Api.Helpers;
using ConsumerOne.Api.Models;
using ConsumerOne.Api.Services;
using ConsumerOne.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ConsumerOne.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExternalAuthController : ControllerBase
    {
        private static readonly HttpClient Client = new HttpClient();
        private readonly ApplicationDbContext _appDbContext;
        private readonly FacebookAuthSettings _fbAuthSettings;
        private readonly IJwtFactory _jwtFactory;
        private readonly IMediaService _mediaService;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<AppUser> _userManager;

        public ExternalAuthController(IOptions<FacebookAuthSettings> fbAuthSettingsAccessor,
            UserManager<AppUser> userManager, ApplicationDbContext appDbContext, IJwtFactory jwtFactory, IMediaService mediaService,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            _fbAuthSettings = fbAuthSettingsAccessor.Value;
            _userManager = userManager;
            _appDbContext = appDbContext;
            _jwtFactory = jwtFactory;
            _mediaService = mediaService;
            _jwtOptions = jwtOptions.Value;
        }



        // POST api/externalauth/facebook
        [HttpPost]
        public async Task<IActionResult> Facebook([FromBody] FacebookAuthViewModel model)
        {
            // 1.generate an app access token
            var appAccessTokenResponse = await Client.GetStringAsync(
                $"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            // 2. validate the user access token
            var userAccessTokenValidationResponse = await Client.GetStringAsync(
                $"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation =
                JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid facebook token.", ModelState));

            // 3. we've got a valid token so we can request user data from fb
            var userInfoResponse = await Client.GetStringAsync(
                $"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            // 4. ready to create the local user account (if necessary) and jwt
            var user = await _userManager.FindByEmailAsync(userInfo.Email);

            if (user == null)
            {
                var appUser = new AppUser
                {
                    Name = userInfo.Name,
                    FacebookId = userInfo.Id,
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    Type = model.UserType
                    //PictureUrl = userInfo.Picture.Data.Url
                };

                var result = await _userManager.CreateAsync(appUser,
                    Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

                if (!string.IsNullOrEmpty(userInfo.Picture.Data.Url))
                {
                    using (var httpClient = new HttpClient())
                    using (var contentStream = await httpClient.GetStreamAsync(userInfo.Picture.Data.Url))
                    {
                        var memoryStream = new MemoryStream();
                        await contentStream.CopyToAsync(memoryStream);
                        await _mediaService.UploadAsync(appUser.Id + ".jpg", memoryStream);
                    }
                }

                if (!result.Succeeded)
                    return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            }

            // generate the jwt for the local user...
            var localUser = await _userManager.FindByNameAsync(userInfo.Email);

            if (localUser == null)
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.",
                    ModelState));

            var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id),
                _jwtFactory, localUser.UserName, _jwtOptions,
                new JsonSerializerSettings {Formatting = Formatting.Indented});

            return new OkObjectResult(jwt);
        }
    }
}