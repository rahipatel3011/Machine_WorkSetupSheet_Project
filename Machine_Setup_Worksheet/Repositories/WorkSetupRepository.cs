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
            return await _db.WorkSetups.ToListAsync();
        }

        public async Task<WorkSetup> GetByIdAsync(Guid id)
        {
            return await _db.WorkSetups.Include(temp => temp.Setups).ThenInclude(temp => temp.Jaw).FirstOrDefaultAsync(workSetup => workSetup.WorkSetupId == id);
        }

        public async Task<WorkSetup> Save(WorkSetup workSetup)
        {
            _db.WorkSetups.Update(workSetup);
            await _db.SaveChangesAsync();
            return workSetup;
        }

        public async Task<int> Delete(Guid id)
        {
            WorkSetup? workSetup = await _db.WorkSetups.FirstOrDefaultAsync(workSetup => workSetup.WorkSetupId == id);
            if (workSetup != null)
            {
                _db.WorkSetups.Remove(workSetup);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        

    }
}
