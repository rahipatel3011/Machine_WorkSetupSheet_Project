using AutoMapper;
using Machine_Setup_Worksheet.CustomExceptions;
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
        private readonly ICacheService _cacheService;

        public MachineService(IMachineRepository machineRepository, IMapper mapper, ICacheService cacheService)
        {
            _machineRepository = machineRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }




        /// <summary>
        /// Returns all machines
        /// </summary>
        /// <returns>All MachineDTO</returns>
        public async Task<IEnumerable<MachineDTO>> GetAllMachines()
        {
            try
            {
                var cacheMachines = await _cacheService.Get<IEnumerable<MachineDTO>>("machines");
                if (cacheMachines != null && cacheMachines.Count() > 0)
                {
                    return cacheMachines;
                }
                IEnumerable<Machine> allMachines = await _machineRepository.GetAll();
                IEnumerable<MachineDTO> allMachinesDTO = _mapper.Map<IEnumerable<MachineDTO>>(allMachines);


                // adding data to cache
                DateTimeOffset expirytime = DateTimeOffset.Now.AddMinutes(5);
                _cacheService.Save("machines", allMachinesDTO, expirytime);

                return allMachinesDTO;
            }
            catch (Exception ex)
            {
                throw new ServiceException("An Error occured while getting all machines in service layer", ex);
            }
        }





        /// <summary>
        /// Returns Machine based on given id
        /// </summary>
        /// <param name="machineId">Machine ID</param>
        /// <returns>MachineDTO</returns>
        public async Task<MachineDTO> GetMachineById(Guid? machineId)
        {
            try
            {
                if (machineId != null)
                {
                    var cacheMachine = await _cacheService.Get<MachineDTO>($"machine{machineId}");
                    if (cacheMachine != null)
                    {
                        return cacheMachine;
                    }
                    Machine? machine = await _machineRepository.GetById(machineId.Value);
                    if (machine != null)
                    {
                        MachineDTO foundMachineDTO = _mapper.Map<MachineDTO>(machine);
                        // adding cache
                        DateTimeOffset expirytime = DateTimeOffset.Now.AddMinutes(5);
                        _cacheService.Save($"machine{machineId}", foundMachineDTO, expirytime);
                        return foundMachineDTO;
                    }
                }

                return new MachineDTO();
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while getting a machine in service layer with id:{machineId}", ex);
            }
        }


        /// <summary>
        /// Creates Machine and returns the same Machine
        /// </summary>
        /// <param name="machineDTO">MachineDTO</param>
        /// <returns>MachineDTO</returns>
        public async Task<MachineDTO> SaveMachine(MachineDTO machineDTO)
        {
            try
            {
                Machine machine = _mapper.Map<Machine>(machineDTO);
                Machine createdMachine = await _machineRepository.Update(machine);
                MachineDTO createdMachineDTO = _mapper.Map<MachineDTO>(createdMachine);


                // invalidate old cache
                await _cacheService.Delete("machines");

                // add to cache
                DateTimeOffset expirytime = DateTimeOffset.Now.AddMinutes(5);
                _cacheService.Save($"jaw{createdMachineDTO.MachineId}", createdMachineDTO, expirytime);

                return createdMachineDTO;
            }
            catch (Exception ex)
            {
                throw new ServiceException("An Error occured while saving a machine in service layer", ex);
            }
        }



        /// <summary>
        /// Delete Machine
        /// </summary>
        /// <param name="machineId">Machine ID</param>
        /// <returns>number of affected data</returns>
        public async Task<int> DeleteMachine(Guid machineId)
        {
            try
            {
                int numberOfDeletedMachines = await _machineRepository.Delete(machineId);

                // invalidate old cache
                await _cacheService.Delete("machines");

                await _cacheService.Delete($"jaw{machineId}"); // deleting cache
                return numberOfDeletedMachines;
            }
            catch (Exception ex)
            {
                throw new ServiceException("An Error occured while deleting a machine in service layer", ex);
            }

        }
    }
}
