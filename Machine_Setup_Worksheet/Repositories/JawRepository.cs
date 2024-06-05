using Machine_Setup_Worksheet.Data;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Machine_Setup_Worksheet.Repositories
{
    public class JawRepository : IJawRepository
    {
        private readonly ApplicationDbContext _db;
        public JawRepository(ApplicationDbContext db) {
            _db = db;
        }


        /// <summary>
        /// Returns all jaws
        /// </summary>
        /// <returns>All Jaws</returns>
        public async Task<IEnumerable<Jaw>> getAll()
        {
            return await _db.Jaws.ToListAsync();
        }


        /// <summary>
        /// Returns Jaw by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Jaw or null</returns>
        public async Task<Jaw> getById(Guid? id)
        {
            if(id != null)
            {
                Jaw foundJaw = await _db.Jaws.FirstOrDefaultAsync(jaw=>jaw.JawId == id);
                if(foundJaw != null)
                {
                    return foundJaw;
                }
            }
            return new Jaw();
        }
    }
}
