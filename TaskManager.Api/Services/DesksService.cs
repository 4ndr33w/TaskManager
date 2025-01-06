
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

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

        public async Task<DeskDto> GetAsync(Guid deskId)
        {
            try
            {
                var result = await _npgDbContext.Desks.FirstOrDefaultAsync(x => x.Id == deskId);
                return result.ToDto();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<bool> UpdateAsync(DeskDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DeskDto>> GetByProjectIdAsync(Guid projectId, Guid userId)
        {
            var desksColection = new List<DeskDto>();
            try
            {
                var result = await _npgDbContext.Desks.Where(d => d.ProjectId == projectId).Select(d => d.ToDto()).ToListAsync();

                foreach (var desk in result)
                {

                    if (desk.IsPrivate == true && desk.AdminId != userId)
                    {
                    }
                    else
                    {
                        desksColection.Add(desk);
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<IEnumerable<DeskDto>> GetAllUserDesksAsync(IEnumerable<Guid> deskIds)
        {
            List<DeskDto> desksCollection = new List<DeskDto>();
            try
            {
                foreach (var id in deskIds)
                {
                    var desk = await _npgDbContext.Desks.FirstOrDefaultAsync(d => d.Id == id);
                    var deskDto = desk.ToDto();
                    if (desk != null && !desksCollection.Contains(deskDto))
                    {
                        desksCollection.Add(deskDto);
                    }
                }
                return desksCollection;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<IEnumerable<DeskDto>> GetAllUserDesksAsync(Guid userId)
        {
            List<DeskDto> desksCollection = new List<DeskDto>();
            try
            {
                var user = await _usersService.GetAsync(userId);

                foreach (var deskId in user.DesksIds)
                {
                    var desk = await _npgDbContext.Desks.FirstOrDefaultAsync(d => d.Id == deskId);
                    var deskDto = desk.ToDto();
                    if (desk != null && !desksCollection.Contains(deskDto))
                    {
                        desksCollection.Add(deskDto);
                    }
                }
                return desksCollection;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> IsUserDeskAdmin(Guid userId, Guid deskId)
        {
            try
            {
                var desk = await _npgDbContext.Desks.FirstOrDefaultAsync(d => d.Id == deskId);
                if (desk != null)
                {
                    bool result = desk.AdminId == userId;
                    if (result == true)
                        return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
