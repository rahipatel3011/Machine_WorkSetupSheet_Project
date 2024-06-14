using Machine_Setup_Worksheet.CustomExceptions;
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
            try
            {
                Setup setup = await _db.Setups.FirstAsync(setup => setup.SetupId == id);
                if (setup != null)
                {
                    _db.Remove(setup);
                    return await _db.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Failed to delete setup from the database with id: {id}", nameof(Jaw), ex);
            }
        }

        public async Task<IEnumerable<Setup>> GetAllAsync(Guid WorkSetupId)
        {
            try
            {
                return await _db.Setups.Where(setup => setup.WorkSetupId == WorkSetupId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to fetch all setups associated with worksetup from the database", nameof(Jaw), ex);
            }
        }

        public async Task<Setup> GetByIdAsync(Guid id)
        {
            try
            {
                Setup setup = await _db.Setups.FirstAsync(setup => setup.SetupId == id);
                return setup;
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Failed to fetch a setup from the database with id: {id}", nameof(Jaw), ex);
            }
        }

        public async Task<Setup> Save(Setup setup)
        {
            try
            {
                _db.Setups.Update(setup);
                await _db.SaveChangesAsync();
                return setup;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to save machine in the database", nameof(Jaw), ex);
            }
        }
    }
}
