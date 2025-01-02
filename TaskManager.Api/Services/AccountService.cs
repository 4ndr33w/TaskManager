using TaskManager.Api.DbContexts;
using TaskManager.Api.Services.SecurityOptions;

namespace TaskManager.Api.Services
{
    public class AccountService
    {
        private readonly NpgDbContext _npgDbContext;
        private readonly IConfiguration _configuration;
        private readonly SecurityService _securityService;

        public AccountService(NpgDbContext npgDbContext, IConfiguration configuration, SecurityService securityService)
        {
            _npgDbContext = npgDbContext;
            _configuration = configuration;
            _securityService = securityService;
        }
    }
}
