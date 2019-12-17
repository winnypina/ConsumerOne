using System;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Api.Data;
using ConsumerOne.Api.Models;
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
    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(UserManager<AppUser> userManager, ApplicationDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null) return new NotFoundResult();

            return new OkObjectResult(_appDbContext.UserMessages.Include(n => n.To).Include(n => n.From)
                .Where(n => n.FromId == user.Id || n.ToId == user.Id).Select(message => new MessageViewModel
                {
                    Id = message.Id,
                    ToId = message.ToId,
                    FromId = message.FromId,
                    Message = message.Message,
                    SentDate = message.SentDate,
                    FromName = message.From.Name,
                    ToName = message.To.Name
                }));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] MessageViewModel model)
        {
            var from = await _userManager.FindByIdAsync(model.FromId);
            var to = await _userManager.FindByIdAsync(model.ToId);
            if (from == null || to == null) return NotFound();

            var message = new UserMessage
            {
                FromId = from.Id,
                ToId = from.Id,
                Message = model.Message,
                SentDate = DateTime.Now
            };

            await _appDbContext.UserMessages.AddAsync(message);

            await _appDbContext.SaveChangesAsync();

            return new OkResult();
        }
    }
}