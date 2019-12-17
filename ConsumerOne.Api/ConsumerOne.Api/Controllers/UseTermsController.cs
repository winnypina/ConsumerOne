using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConsumerOne.Api.Data;
using ConsumerOne.Api.ViewModels;

namespace ConsumerOne.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UseTermsController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;

        public UseTermsController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<UseTermsViewModel>> Get()
        {
            var useTerm = await _appDbContext.UseTerms.FirstAsync();
            return new OkObjectResult(new UseTermsViewModel { Text = useTerm.Value, TextEnUs = useTerm.ValueEnUs, TextEs = useTerm.ValueEs});
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody]UseTermsViewModel viewModel)
        {
            var useTerm = await _appDbContext.UseTerms.FirstAsync();
            useTerm.Value = viewModel.Text;
            useTerm.ValueEnUs = viewModel.TextEnUs;
            useTerm.ValueEs = viewModel.TextEs;
            _appDbContext.Update(useTerm);
            await _appDbContext.SaveChangesAsync();
            return new OkObjectResult(true);
        }
    }
}