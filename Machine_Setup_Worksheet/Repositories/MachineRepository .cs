using AutoMapper;
using Machine_Setup_Worksheet.CustomExceptions;
using Machine_Setup_Worksheet.Data;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Machine_Setup_Worksheet.Repositories
{

    public class MachineRepository : IMachineRepository
    {
        private readonly ApplicationDbContext _db;

        public MachineRepository(ApplicationDbContext db) {
            _db = db;
        }




        /// <summary>
        /// Returns all Machines
        /// </summary>
        /// <returns>All Machines</returns>
        public async Task<IEnumerable<Machine>> GetAll()
        {
            try
            {
                return await _db.Machines.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to fetch all Machines from the database", nameof(Jaw), ex);
            }
        }


        /// <summary>
        /// Returns Machine by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Machine or null</returns>
        public async Task<Machine> GetById(Guid id)
        {
            try
            {
                Machine foundMachine = await _db.Machines.FirstAsync(machine => machine.MachineId == id);
                return foundMachine;
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Failed to fetch a machine with id: {id} from the database", nameof(Jaw), ex);
            }

        }


        /// <summary>
        /// Create a new Machine with given name
        /// </summary>
        /// <param name="machine">Machine</param>
        /// <returns>Created Machines</returns>
        public async Task<Machine> Create(Machine machine)
        {
            try
            {
                _db.Machines.Add(machine);
                await _db.SaveChangesAsync();
                return machine;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to create machine in the database", nameof(Jaw), ex);
            }
        }


        /// <summary>
        /// Update Machine
        /// </summary>
        /// <param name="machine">Machines</param>
        /// <returns>Machines</returns>
        public async Task<Machine> Update(Machine machine)
        {
            try
            {
                _db.Machines.Update(machine);
                await _db.SaveChangesAsync(true);
                return machine;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to update a machine in the database", nameof(Jaw), ex);
            }
        }




        /// <summary>
        /// Delete machine from the record
        /// </summary>
        /// <param name="id">MachineID</param>
        /// <returns>number of affected data</returns>
        public async Task<int> Delete(Guid id)
        {
            try
            {
                Machine foundMachine = await _db.Machines.FirstAsync(machine => machine.MachineId == id);
                if (foundMachine != null)
                {
                    _db.Machines.Remove(foundMachine);
                    return await _db.SaveChangesAsync();
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Failed to delete machine from the database with id: {id}", nameof(Jaw), ex);
            }
        }

    }
}
