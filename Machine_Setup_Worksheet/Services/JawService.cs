using AutoMapper;
using Machine_Setup_Worksheet.CustomExceptions;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Machine_Setup_Worksheet.Services.IServices;

namespace Machine_Setup_Worksheet.Services
{
    public class JawService : IJawService
    {
        private readonly IJawRepository _jawRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public JawService(IJawRepository jawRepository, IMapper mapper, ICacheService cacheService) { 
            _jawRepository = jawRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        


        /// <summary>
        /// Returns all jaws
        /// </summary>
        /// <returns>All JawsDTO</returns>
        public async Task<IEnumerable<JawsDTO>> GetAllJaws()
        {
            try
            {
                var cacheJaws = await _cacheService.Get<IEnumerable<JawsDTO>>("jaws");
                if (cacheJaws != null && cacheJaws.Count() > 0)
                {
                    return cacheJaws;
                }
                IEnumerable<Jaw> allJaws = await _jawRepository.GetAll();
                IEnumerable<JawsDTO> allJawsDTO = _mapper.Map<IEnumerable<JawsDTO>>(allJaws);

                // adding data to cache
                DateTimeOffset expirytime = DateTimeOffset.Now.AddMinutes(5);
                _cacheService.Save("jaws", allJawsDTO, expirytime);
                return allJawsDTO;

            }catch (Exception ex)
            {
                throw new ServiceException("An Error occured while getting all jaws in service layer", ex);
            }
        }





        /// <summary>
        /// Returns Jaw based on given id
        /// </summary>
        /// <param name="jawId">Jaw ID</param>
        /// <returns>JawsDTO</returns>
        public async Task<JawsDTO> GetJawById(Guid? jawId)
        {
            try
            {
                if (jawId != null)
                {
                    var cacheJaw = await _cacheService.Get<JawsDTO>($"jaw{jawId}");
                    if (cacheJaw != null)
                    {
                        return cacheJaw;
                    }
                    Jaw? jaw = await _jawRepository.GetById(jawId.Value);
                    JawsDTO jawDTO = _mapper.Map<JawsDTO>(jaw);

                    // adding cache
                    DateTimeOffset expirytime = DateTimeOffset.Now.AddMinutes(5);
                    _cacheService.Save($"jaw{jawId}", jawDTO, expirytime);
                    return jawDTO;
                }
                throw new ArgumentNullException(nameof(jawId), "Jaw id cannot be null");
            }
            catch (Exception ex)
            {
                throw new ServiceException($"An Error occured while getting a jaws with id {jawId} in service layer", ex);
            }
        }


        /// <summary>
        /// Creates jaw and returns the same jaws
        /// </summary>
        /// <param name="jawsDTO">JawsDTO</param>
        /// <returns>JawsDTO</returns>
        public async Task<JawsDTO> SaveJaw(JawsDTO jawsDTO)
        {
            try
            {
                Jaw jaw = _mapper.Map<Jaw>(jawsDTO);
                Jaw createdJaw = await _jawRepository.Update(jaw);
                JawsDTO createdJawsDTO = _mapper.Map<JawsDTO>(createdJaw);


                // invalidate old cache
                await _cacheService.Delete("jaws");

                // add to cache
                DateTimeOffset expirytime = DateTimeOffset.Now.AddMinutes(5);
                _cacheService.Save($"jaw{createdJawsDTO.JawId}", createdJawsDTO, expirytime);
                return createdJawsDTO;
            }
            catch (Exception ex)
            {
                throw new ServiceException("An Error occured while saving a jaw in service layer", ex);
            }
        }


        /// <summary>
        /// Delete Jaw
        /// </summary>
        /// <param name="jawId">Jaw ID</param>
        /// <returns>number of affected data</returns>
        public async Task<int> DeleteJaw(Guid jawId)
        {
            try
            {
                await _cacheService.Delete($"jaw{jawId}"); // deleting cache

                // invalidate old cache
                await _cacheService.Delete("jaws");

                return await _jawRepository.Delete(jawId);
            }
            catch (Exception ex)
            {
                throw new ServiceException("An Error occured while deleting a jaw in service layer", ex);
            }
        }
    }
}
