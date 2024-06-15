using AutoMapper;
using Machine_Setup_Worksheet.CustomExceptions;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Machine_Setup_Worksheet.Services.IServices;
using System.Collections.Generic;

namespace Machine_Setup_Worksheet.Services
{
    public class WorkSetupService : IWorkSetupService
    {
        private readonly IWorkSetupRepository _workRepository;
        private readonly IMapper _mapper;

        public WorkSetupService(IWorkSetupRepository workRepository, IMapper mapper) {
            _workRepository = workRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Insert or Update WorkSetup
        /// </summary>
        /// <param name="workSetupDTO">WorkSetupDTO</param>
        /// <returns>WorkSetupDTO</returns>
        public async Task<WorkSetupDTO> SaveWorkSetup(WorkSetupDTO workSetupDTO)
        {
            try
            {
                WorkSetup workSetup = _mapper.Map<WorkSetup>(workSetupDTO);
                WorkSetup savedWorkSetup = await _workRepository.Save(workSetup);
                return _mapper.Map<WorkSetupDTO>(savedWorkSetup);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while saving a work setup in service layer", ex);
            }
        }



        /// <summary>
        /// Delete Work Setup
        /// </summary>
        /// <param name="id">WorkSetup Id</param>
        /// <returns>int(affeceted records)</returns>
        public async Task<int> DeleteWorkSetup(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    return await _workRepository.Delete(id);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while deleting a work setup in service layer with id:{id}", ex);
            }
        }



        /// <summary>
        /// Find all WorkSetups
        /// </summary>
        /// <returns>return IEnumerable of WorkSetupDTO</returns>
        public async Task<IEnumerable<WorkSetupDTO>> GetAllWorkSetupAsync()
        {
            try
            {
                return _mapper.Map<IEnumerable<WorkSetupDTO>>(await _workRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while getting all work setups in service layer", ex);
            }
        }



        /// <summary>
        /// Find Work Setup using provided ID
        /// </summary>
        /// <param name="id">WorkSetupId</param>
        /// <returns>WorkSetupDTO</returns>
        public async Task<WorkSetupDTO> GetWorkSetupByIdAsync(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    WorkSetup workSetup = await _workRepository.GetByIdAsync(id);
                    workSetup.Setups = workSetup.Setups.OrderBy(s => s.SetupNumber).ToList();
                    return _mapper.Map<WorkSetupDTO>(workSetup);
                }
                else
                {
                    throw new ArgumentNullException(nameof(id), "id cannot be null");
                }
                
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while getting a work setup in service layer with id:{id}", ex);
            }
        }



        /// <summary>
        /// Filter WorkSetup using searchTerm
        /// </summary>
        /// <param name="searchTerm">search keywork</param>
        /// <returns><IEnumerable<WorkSetupDTO>></returns>
        /// <exception cref="ServiceException">Service Exception</exception>
        public async Task<IEnumerable<WorkSetupDTO>> GetBySearchAsync(string searchTerm)
        {
            try
            {
                IEnumerable<WorkSetup> allWorkSetups = await _workRepository.GetAllAsync();
                IEnumerable<WorkSetupDTO> allWorkSetupsDTOs = _mapper.Map< IEnumerable < WorkSetupDTO >>(allWorkSetups);
                return allWorkSetupsDTOs.Where(ws=> ContainsKeyword(ws,searchTerm)).ToList();
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while getting a work setup in service layer with search:{searchTerm}", ex);
            }
        }


        #region private methods
        private bool ContainsKeyword(WorkSetupDTO workSetup, string keyword)
        {
            if (workSetup == null)
                return false;

            if (String.IsNullOrWhiteSpace(keyword))
                return true;

            // Check properties of WorkSetupDTO
            if (RemoveWhiteSpaces(workSetup.WorkSetupName)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                RemoveWhiteSpaces(workSetup.WorkSetupCode)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                RemoveWhiteSpaces(workSetup.CompanyName)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                RemoveWhiteSpaces(workSetup.Note)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            // Check each SetupDTO in the Setups collection
            foreach (var setup in workSetup.Setups ?? Enumerable.Empty<SetupDTO>())
            {
                if (ContainsKeyword(setup, keyword))
                    return true;
            }

            return false;
        }

        private bool ContainsKeyword(SetupDTO setup, string keyword)
        {
            if (setup == null)
                return false;

            // Check properties of SetupDTO
            if (RemoveWhiteSpaces(setup.Toothinfo)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                RemoveWhiteSpaces(Convert.ToString(setup.MaterialSize)).Contains(keyword, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            // Check the associated JawsDTO
            if (setup.Jaw != null && ContainsKeyword(setup.Jaw, keyword))
                return true;

            return false;
        }

        private bool ContainsKeyword(JawsDTO jaw, string keyword)
        {
            if (jaw == null)
                return false;

            // Check properties of JawsDTO
            if (RemoveWhiteSpaces(jaw.JawName)?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            return false;
        }

        private string RemoveWhiteSpaces(string input)
        {
            if (input == null)
                return null;

            return new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
        #endregion
    }
}
