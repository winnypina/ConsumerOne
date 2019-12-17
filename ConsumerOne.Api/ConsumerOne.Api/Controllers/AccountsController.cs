using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConsumerOne.Api.Data;
using ConsumerOne.Api.Helpers;
using ConsumerOne.Api.Models;
using ConsumerOne.Api.Services;
using ConsumerOne.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsumerOne.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly ISmsService _smsService;
        private readonly UserManager<AppUser> _userManager;

        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor, IMediaService mediaService, ISmsService smsService,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _mediaService = mediaService;
            _smsService = smsService;
            _emailSender = emailSender;
        }

        // POST api/accounts
        [HttpGet]
        [Route("admin")]
        public async Task<IActionResult> GetAdmin()
        {
            var users = await _userManager.Users.Where(n => n.Type == UserType.Admin && n.Email != "admin@consumer.one")
                .ToListAsync();


            return new OkObjectResult(users.Select(n => new AdminAccountViewModel
            {
                Email = n.Email,
                Id = n.Id
            }));
        }

        [HttpGet]
        [Route("admin/{id}")]
        public async Task<IActionResult> GetAdminById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return new NotFoundResult();

            return new OkObjectResult(new AdminAccountViewModel
            {
                Email = user.Email,
                Id = user.Id
            });
        }

        // POST api/accounts
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst(n => n.Type == "id").Value;
            var user = await _userManager.Users.Include(n=>n.Ratings).SingleAsync(n => n.Id == id);

            if (user == null) return new NotFoundResult();

            var ratings = user.Ratings;

            var rating = 0d;
            if (ratings != null && ratings.Count > 0)
            {
                rating = ratings.Sum(n => n.Score) / ratings.Count;
            }

            var userVm = new AccountViewModel
            {
                Id = user.Id,
                BirthDate = user.BirthDate,
                DesiredLanguage = user.DesiredLanguage,
                Type = user.Type,
                FacebookId = user.FacebookId,
                MobilePhone = user.MobilePhone,
                Name = user.Name,
                About = user.About,
                Address = user.Address,
                AddressAddon = user.AddressAddon,
                BusinessPhone = user.BusinessPhone,
                Cep = user.Cep,
                City = user.City,
                AddressNumber = user.AddressNumber,
                Country = user.Country,
                State = user.State,
                Website = user.Website,
                Email = user.Email,
                Rating = 0,
                Followers = _appDbContext.Relations.Count(n=>n.FollowedId == user.Id)
            };

            return new OkObjectResult(userVm);
        }

        [HttpGet]
        [Authorize]
        [Route("profile/{id:guid}")]
        public async Task<IActionResult> GetProfile(string id)
        {
            var currentId = _httpContextAccessor.HttpContext.User.FindFirst(n => n.Type == "id").Value;
            var currentUser = await _userManager.Users.SingleAsync(n => n.Id == currentId);
            var user = await _userManager.Users.Include(n => n.Ratings).SingleOrDefaultAsync(n=>n.Id == id);

            if (user == null) return new NotFoundResult();

            double userRating = 0;

            if (user.Ratings != null && user.Ratings.Count > 0)
            {
                userRating = user.Ratings.Average(n=>n.Score);
            }

            var userVm = new AccountViewModel
            {
                Id = user.Id,
                BirthDate = user.BirthDate,
                DesiredLanguage = user.DesiredLanguage,
                Type = user.Type,
                FacebookId = user.FacebookId,
                MobilePhone = user.MobilePhone,
                Name = user.Name,
                About = user.About,
                Address = user.Address,
                AddressAddon = user.AddressAddon,
                BusinessPhone = user.BusinessPhone,
                Cep = user.Cep,
                City = user.City,
                AddressNumber = user.AddressNumber,
                Country = user.Country,
                State = user.State,
                Website = user.Website,
                Email = user.Email,
                Rating = user.Ratings?.Count ?? 0,
                IsFollowing = _appDbContext.Relations.Any(n=>n.FollowerId==currentId && n.FollowedId == user.Id),
                MyRating = userRating
            };

            return new OkObjectResult(userVm);
        }

        [HttpPost]
        [Route("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] CodeValidationViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName.Replace("(", "").Replace(")", "")
                .Replace("+", "").Replace("-", ""));
            if (user == null) return NotFound();
            if (user.Code != model.Code) return BadRequest();
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            return Ok();
        }


        [HttpGet]
        [Authorize]
        [Route("rating")]
        public async Task<IActionResult> GetRatings()
        {
            var currentId = _httpContextAccessor.HttpContext.User.FindFirst(n => n.Type == "id").Value;
            var currentUser = await _userManager.Users.SingleAsync(n => n.Id == currentId);

            if (currentUser == null) return new NotFoundResult();

            return new OkObjectResult(_appDbContext.Ratings.Include(n => n.To).Include(n => n.From)
                .Where(n => n.FromId == currentUser.Id).Select(message => new RatingViewModel
                {
                    Id = message.Id,
                    ToId = message.ToId,
                    FromId = message.FromId,
                    Message = message.Message,
                    SentDate = message.SentDate,
                    FromName = message.From.Name,
                    ToName = message.To.Name,
                    Score = message.Score
                }));
        }

        [HttpGet]
        [Authorize]
        [Route("rating/{id}")]
        public async Task<IActionResult> GetRatingsForUser(string id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(n => n.Id == id);

            if (user == null) return new NotFoundResult();  

            return new OkObjectResult(_appDbContext.Ratings.Include(n => n.To).Include(n => n.From)
                .Where(n => n.ToId == user.Id).Select(message => new RatingViewModel
                {
                    Id = message.Id,
                    ToId = message.ToId,
                    FromId = message.FromId,
                    Message = message.Message,
                    SentDate = message.SentDate,
                    FromName = message.From.Name,
                    ToName = message.To.Name,
                    Score = message.Score
                }));
        }

        // POST api/accounts
        [HttpPost]
        [Route("passwordReset")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst(n => n.Type == "id").Value;
            var user = await _userManager.Users.SingleAsync(n => n.Id == id);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            return new OkObjectResult("Account created");
        }

        // POST api/accounts
        [HttpPost]
        [Route("newPassword")]
        public async Task<IActionResult> NewPassword([FromBody] ResetPasswordViewModel model)
        {

            var user = await _userManager.FindByNameAsync(model.User.Replace("(", "").Replace(")", "")
                .Replace("+", "").Replace("-", ""));
            if(user == null)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            if (user.Email == "no@email.com")
                await _smsService.Send(
                    user.UserName.Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", ""),
                    $"Sua nova senha para o ConsumerOne: {model.NewPassword}");
            else
                await _emailSender.Send(user.Email, "Confirmação de cadastro - Consumer One",
                    $"Sua nova senha para o ConsumerOne: {model.NewPassword}");

            return new OkObjectResult("Account created");
        }

        [HttpPost]
        [Authorize]
        [Route("rating")]
        public async Task<IActionResult> AddRating([FromBody] RatingViewModel model)
        {
            var from = await _userManager.FindByIdAsync(model.FromId);
            var to = await _userManager.FindByIdAsync(model.ToId);
            if (from == null || to == null) return NotFound();

            var message = new Rating
            {
                FromId = from.Id,
                ToId = to.Id,
                Message = model.Message,
                SentDate = DateTime.UtcNow,
                Score = model.Score
            };

            await _appDbContext.Ratings.AddAsync(message);

            await _appDbContext.SaveChangesAsync();

            return new OkResult();
        }

        [HttpGet]
        [Authorize]
        [Route("followers")]
        public async Task<IActionResult> GetFollowers()
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst(n => n.Type == "id").Value;
            var user = await _userManager.Users.SingleAsync(n => n.Id == id);
            var followerIds = _appDbContext.Relations.Where(n=> n.FollowedId == id).Select(n => n.FollowerId).Distinct();

            return new OkObjectResult(_userManager.Users.Where(n => followerIds.Contains(n.Id)).Select(us =>
                new AccountViewModel
                {
                    Id = us.Id,
                    Name = us.Name
                }
            ));
        }

        [HttpGet]
        [Authorize]
        [Route("follows")]
        public async Task<IActionResult> GetFollows()
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst(n => n.Type == "id").Value;
            var user = await _userManager.Users.SingleAsync(n => n.Id == id);
            var followedIds = _appDbContext.Relations.Where(n => n.FollowerId == id).Select(n => n.FollowedId).Distinct();

            return new OkObjectResult(_userManager.Users.Where(n => followedIds.Contains(n.Id)).Select(us =>
                new AccountViewModel
                {
                    Id = us.Id,
                    Name = us.Name
                }
            ));
        }

        [HttpPost]
        [Authorize]
        [Route("follow/{userid}")]
        public async Task<IActionResult> Follow(string userId)
        {
            var id = _httpContextAccessor.HttpContext.User.FindFirst(n => n.Type == "id").Value;
            var user = await _userManager.Users.SingleAsync(n => n.Id == id);
            var userToFollow = await _userManager.Users.SingleAsync(n => n.Id == userId);
            if (userToFollow == null) return NotFound();


            var isFollowing = await _appDbContext.Relations.SingleOrDefaultAsync(n => n.FollowerId == id && n.FollowedId == userToFollow.Id);
            if (isFollowing == null)
            {
                await _appDbContext.Relations.AddAsync(new Relation {FollowerId = id, FollowedId = userToFollow.Id});
            }
            else
            {
                _appDbContext.Relations.Remove(isFollowing);
            }

            await _appDbContext.SaveChangesAsync();

            return new OkResult();
        }

        [HttpPost]
        [Route("resend")]
        public async Task<IActionResult> ResendCode([FromBody] CodeValidationViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName.Replace("(", "").Replace(")", "")
                .Replace("+", "").Replace("-", ""));
            if (user == null) return NotFound();
            if (user.Email == "no@email.com")
                await _smsService.Send(
                    user.UserName.Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", ""),
                    $"Seu código de verificação para o ConsumerOne: {user.Code.ToString().PadLeft(4, '0')}");
            else
                await _emailSender.Send(user.Email, "Confirmação de cadastro - Consumer One",
                    $"Seu código de verificação para o ConsumerOne: {user.Code.ToString().PadLeft(4, '0')}");
            return Ok();
        }

        [HttpPost]
        [Route("admin")]
        public async Task<IActionResult> PostAdmin([FromBody] AdminAccountViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userIdentity = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Email,
                Type = UserType.Admin
            };


            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            var userVm = new AccountViewModel
            {
                Id = userIdentity.Id,
                BirthDate = userIdentity.BirthDate,
                DesiredLanguage = userIdentity.DesiredLanguage,
                Type = userIdentity.Type,
                FacebookId = userIdentity.FacebookId,
                MobilePhone = userIdentity.MobilePhone,
                Name = userIdentity.Name,
                About = userIdentity.About,
                Address = userIdentity.Address,
                AddressAddon = userIdentity.AddressAddon,
                AddressNumber = userIdentity.AddressNumber,
                BusinessPhone = userIdentity.BusinessPhone,
                Cep = userIdentity.Cep,
                City = userIdentity.City,
                Country = userIdentity.Country,
                State = userIdentity.State,
                Website = userIdentity.Website,
                Email = userIdentity.Email
            };

            return new OkObjectResult(userVm);
        }


        [HttpPut]
        [Route("admin")]
        public async Task<IActionResult> PutAdmin([FromBody] AdminAccountViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userIdentity =  await _userManager.FindByEmailAsync(model.Email);

            if(userIdentity != null)
            {
                userIdentity.Email = model.Email;
                userIdentity.UserName = model.Email;
                userIdentity.Name = model.Email;
                userIdentity.Type = UserType.Admin;
            }

            var result = await _userManager.UpdateAsync(userIdentity);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            var userVm = new AccountViewModel
            {
                Id = userIdentity.Id,
                BirthDate = userIdentity.BirthDate,
                DesiredLanguage = userIdentity.DesiredLanguage,
                Type = userIdentity.Type,
                FacebookId = userIdentity.FacebookId,
                MobilePhone = userIdentity.MobilePhone,
                Name = userIdentity.Name,
                About = userIdentity.About,
                Address = userIdentity.Address,
                AddressAddon = userIdentity.AddressAddon,
                AddressNumber = userIdentity.AddressNumber,
                BusinessPhone = userIdentity.BusinessPhone,
                Cep = userIdentity.Cep,
                City = userIdentity.City,
                Country = userIdentity.Country,
                State = userIdentity.State,
                Website = userIdentity.Website,
                Email = userIdentity.Email
            };

            return new OkObjectResult(userVm);
        }

        [HttpDelete]
        [Route("admin/{id}")]
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            return Ok();
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (string.IsNullOrEmpty(model.Email) || model.Email == "no@email.com")
            {
                var currentUser = await _userManager.FindByNameAsync(model.MobilePhone.Replace("(", "").Replace(")", "")
                    .Replace("+", "").Replace("-", ""));
                if (currentUser != null) return BadRequest();
            }
            else
            {
                var currentUser = await _userManager.FindByNameAsync(model.Email.Replace("(", "").Replace(")", "")
                    .Replace("+", "").Replace("-", ""));
                if (currentUser != null) return BadRequest();
            }

            var random = new Random();
            var code = random.Next(0, 9999);

            var userIdentity = _mapper.Map<AppUser>(model);
            userIdentity.Code = code;
            if (string.IsNullOrEmpty(model.Email) || model.Email == "no@email.com")
            {
                userIdentity.UserName = model.MobilePhone.Replace("(", "").Replace(")", "").Replace("+", "")
                    .Replace("-", "");
                await _smsService.Send(userIdentity.UserName,
                    $"Seu código de verificação para o ConsumerOne: {code.ToString().PadLeft(4, '0')}");
            }
            else
            {
                userIdentity.UserName = model.Email;
                await _emailSender.Send(userIdentity.Email, "Confirmação de cadastro - Consumer One",
                    $"Seu código de verificação para o ConsumerOne: {code.ToString().PadLeft(4, '0')}");
            }

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            var userVm = new AccountViewModel
            {
                Id = userIdentity.Id,
                BirthDate = userIdentity.BirthDate,
                DesiredLanguage = userIdentity.DesiredLanguage,
                Type = userIdentity.Type,
                FacebookId = userIdentity.FacebookId,
                MobilePhone = userIdentity.MobilePhone,
                Name = userIdentity.Name,
                About = userIdentity.About,
                Address = userIdentity.Address,
                AddressAddon = userIdentity.AddressAddon,
                AddressNumber = userIdentity.AddressNumber,
                BusinessPhone = userIdentity.BusinessPhone,
                Cep = userIdentity.Cep,
                City = userIdentity.City,
                Country = userIdentity.Country,
                State = userIdentity.State,
                Website = userIdentity.Website,
                Email = userIdentity.Email
            };

            return new OkObjectResult(userVm);
        }


        // PUT api/accounts
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] AccountViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            user.BirthDate = model.BirthDate;
            user.MobilePhone = model.MobilePhone;
            user.AddressNumber = model.AddressNumber;
            user.BusinessPhone = model.BusinessPhone;
            user.Cep = model.Cep;
            user.City = model.City;
            user.Country = model.Country;
            user.State = model.State;
            user.Website = model.Website;
            user.About = model.About;
            user.Address = model.Address;
            user.AddressAddon = model.AddressAddon;
            user.DesiredLanguage = model.DesiredLanguage;
            user.Name = model.Name;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            return new OkObjectResult(user);
        }
    }
}