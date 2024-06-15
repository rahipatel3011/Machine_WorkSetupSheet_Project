using Machine_Setup_Worksheet.CustomExceptions;
using Machine_Setup_Worksheet.Data;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Machine_Setup_Worksheet.Repositories
{
    public class WorkSetupRepository : IWorkSetupRepository
    {
        private readonly ApplicationDbContext _db;

        public WorkSetupRepository(ApplicationDbContext db) {
            _db = db;
        }

        public async Task<IEnumerable<WorkSetup>> GetAllAsync()
        {
            try
            {
                return await _db.WorkSetups.Include(temp => temp.Setups).ThenInclude(temp => temp.Jaw).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to fetch all work setup from the database", nameof(Jaw), ex);
            }
        }

        public async Task<WorkSetup> GetByIdAsync(Guid id)
        {
            try
            {
                return await _db.WorkSetups.Include(temp => temp.Setups).ThenInclude(temp => temp.Jaw).FirstAsync(workSetup => workSetup.WorkSetupId == id);
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Failed to fetch work setup from the database with id: {id}", nameof(Jaw), ex);
            }
        }

        public async Task<WorkSetup> Save(WorkSetup workSetup)
        {
            try
            {
                _db.WorkSetups.Update(workSetup);
                await _db.SaveChangesAsync();
                return workSetup;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to fetch save work setup in the database", nameof(Jaw), ex);
            }
        }

        public async Task<int> Delete(Guid id)
        {
            try
            {
                WorkSetup? workSetup = await _db.WorkSetups.FirstAsync(workSetup => workSetup.WorkSetupId == id);
                if (workSetup != null)
                {
                    _db.WorkSetups.Remove(workSetup);
                    return await _db.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to delete work setup from the database", nameof(Jaw), ex);
            }
        }

        

    }
}
