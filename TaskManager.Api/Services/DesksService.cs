
using TaskManager.Api.DbContexts;
using TaskManager.Api.Services.Abstractions;
using TaskManager.Api.Services.Interfaces;
using TaskManager.Models.Dtos;

namespace TaskManager.Api.Services
{
    public class DesksService : IBaseService<DeskDto>
    {
        private readonly NpgDbContext _npgDbContext;
        private readonly IConfiguration _configuration;
        private readonly AccountService _accountService;
        private readonly UsersService _usersService;

        public DesksService(NpgDbContext npgDbContext, IConfiguration configuration, AccountService accountService, UsersService usersService)
        {
            _npgDbContext = npgDbContext;
            _configuration = configuration;
            _accountService = accountService;
            _usersService = usersService;
        }


        public async Task<bool> CreateAsync(DeskDto deskDto)
        {
            try
            {
                _npgDbContext.Desks.Add(new Models.Desk(deskDto));
                var result = await _npgDbContext.SaveChangesAsync();

                if (result == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
           
            return false;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DeskDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(DeskDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
