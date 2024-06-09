using AutoMapper;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Repositories;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Machine_Setup_Worksheet.Services.IServices;

namespace Machine_Setup_Worksheet.Services
{
    public class MachineService : IMachineService
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IMapper _mapper;

        public MachineService(IMachineRepository machineRepository, IMapper mapper) {
            _machineRepository = machineRepository;
            _mapper = mapper;
        }

        


        /// <summary>
        /// Returns all machines
        /// </summary>
        /// <returns>All MachineDTO</returns>
        public async Task<IEnumerable<MachineDTO>> GetAllMachines()
        {
            IEnumerable<Machine> allMachines = await _machineRepository.GetAll();
            IEnumerable<MachineDTO> allMachinesDTO = _mapper.Map<IEnumerable<MachineDTO>>(allMachines);

            return allMachinesDTO;
        }





        /// <summary>
        /// Returns Machine based on given id
        /// </summary>
        /// <param name="machineId">Machine ID</param>
        /// <returns>MachineDTO</returns>
        public async Task<MachineDTO> GetMachineById(Guid? machineId)
        {

            if (machineId != null)
            {
                Machine? machine = await _machineRepository.GetById(machineId.Value);
                if(machine != null)
                {
                    return _mapper.Map<MachineDTO>(machine);
                }
            }

            return new MachineDTO();
        }


        /// <summary>
        /// Creates Machine and returns the same Machine
        /// </summary>
        /// <param name="machineDTO">MachineDTO</param>
        /// <returns>MachineDTO</returns>
        public async Task<MachineDTO> SaveMachine(MachineDTO machineDTO)
        {          
            Machine machine = _mapper.Map<Machine>(machineDTO);
            Machine createdMachine = await _machineRepository.Update(machine);
            return _mapper.Map<MachineDTO>(createdMachine);
        }



        /// <summary>
        /// Delete Machine
        /// </summary>
        /// <param name="machineId">Machine ID</param>
        /// <returns>number of affected data</returns>
        public async Task<int> DeleteMachine(Guid machineId)
        {
            return await _machineRepository.Delete(machineId);
            
        }
    }
}
