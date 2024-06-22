using Machine_Setup_Worksheet.Models.DTOs;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface IMachineService
    {
        /// <summary>
        /// Returns all machines
        /// </summary>
        /// <returns>All MachineDTO</returns>
        Task<IEnumerable<MachineDTO>> GetAllMachines();

        /// <summary>
        /// Returns Machine based on given id
        /// </summary>
        /// <param name="machineId">Machine ID</param>
        /// <returns>MachineDTO</returns>
        Task<MachineDTO> GetMachineById(Guid? machineId);


        /// <summary>
        /// Creates Machine and returns the same Machine
        /// </summary>
        /// <param name="machineDTO">MachineDTO</param>
        /// <returns>MachineDTO</returns>
        Task<MachineDTO> SaveMachine(MachineDTO machineDTO);


        /// <summary>
        /// Delete Machine
        /// </summary>
        /// <param name="machineId">Machine ID</param>
        /// <returns>number of affected data</returns>
        Task<int> DeleteMachine(Guid jawId);
    }
}
