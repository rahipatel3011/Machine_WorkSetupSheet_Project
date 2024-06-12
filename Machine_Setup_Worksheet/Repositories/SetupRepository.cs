using Machine_Setup_Worksheet.Data;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Machine_Setup_Worksheet.Repositories
{
    public class SetupRepository : ISetupRepository
    {
        private readonly ApplicationDbContext _db;

        public SetupRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<int> Delete(Guid id)
        {
            Setup? setup = _db.Setups.FirstOrDefault(setup => setup.SetupId == id);
            if(setup != null)
            {
                _db.Remove(setup);
                await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<Setup>> GetAllAsync(Guid WorkSetupId)
        {
            return await _db.Setups.Where(setup => setup.WorkSetupId == WorkSetupId).ToListAsync();
        }

        public async Task<Setup> GetByIdAsync(Guid id)
        {
            Setup? setup = await _db.Setups.FirstOrDefaultAsync(setup => setup.SetupId == id);
            return setup;
        }

        public async Task<Setup> Save(Setup setup)
        {
            _db.Setups.Update(setup);
            await _db.SaveChangesAsync();
            return setup;
        }
    }
}
