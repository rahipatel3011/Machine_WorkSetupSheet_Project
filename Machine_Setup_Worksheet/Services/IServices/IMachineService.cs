using Machine_Setup_Worksheet.Models.DTOs;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface IMachineService
    {
        Task<IEnumerable<MachineDTO>> GetAllMachines();
        Task<MachineDTO> GetMachineById(Guid? machineId);
        Task<MachineDTO> SaveMachine(MachineDTO machineDTO);
        Task<int> DeleteMachine(Guid jawId);
    }
}
