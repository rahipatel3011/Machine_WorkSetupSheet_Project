using AutoMapper;
using Machine_Setup_Worksheet.Data;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
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
        public async Task<IEnumerable<Jaw>> GetAll()
        {
            return await _db.Jaws.ToListAsync();
        }


        /// <summary>
        /// Returns Jaw by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Jaw or null</returns>
        public async Task<Jaw> GetById(Guid id)
        {
            Jaw? foundJaw = await _db.Jaws.FirstOrDefaultAsync(jaw=>jaw.JawId == id);
            return foundJaw;
                
        }


        /// <summary>
        /// Create a new Jaw with given name
        /// </summary>
        /// <param name="jaw">Jaw</param>
        /// <returns>Created Jaws</returns>
        public async Task<Jaw> Create(Jaw jaw)
        {
            _db.Jaws.Add(jaw);
            await _db.SaveChangesAsync();
            return jaw;
        }


        /// <summary>
        /// Update Jaws
        /// </summary>
        /// <param name="jaw">Jaw</param>
        /// <returns>Jaw</returns>
        public async Task<Jaw> Update(Jaw jaw)
        {
            _db.Jaws.Update(jaw);
            await _db.SaveChangesAsync(true);
            return jaw;
        }


        /// <summary>
        /// Delete Jaws from the record
        /// </summary>
        /// <param name="id">Jaws Id</param>
        /// <returns>number of affected data</returns>
        public async Task<int> Delete(Guid id)
        {
            Jaw? foundJaw = await _db.Jaws.FirstOrDefaultAsync(jaw => jaw.JawId == id);
            if(foundJaw != null)
            {
                _db.Jaws.Remove(foundJaw);
                return await _db.SaveChangesAsync();
            }

            return 0;
        }
    }
}
