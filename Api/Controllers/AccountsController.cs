using System.Threading.Tasks;
using AuthTokens.Model;
using AuthTokens.Repository;
using AuthTokens.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthTokens.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistrationViewModel model)
        {
            var userIdentity = _mapper.Map<AppUser>(model);
            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            if (!result.Succeeded)
            {
                return new BadRequestResult();
            }
            await _appDbContext.Customers.AddAsync(new Customer { IdentityId = userIdentity.Id });
            await _appDbContext.SaveChangesAsync();
            return new OkObjectResult("Account created");
        }
    }
}