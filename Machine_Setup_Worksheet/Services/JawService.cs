using AutoMapper;
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

        public JawService(IJawRepository jawRepository, IMapper mapper) { 
            _jawRepository = jawRepository;
            _mapper = mapper;
        }

        


        /// <summary>
        /// Returns all jaws
        /// </summary>
        /// <returns>All JawsDTO</returns>
        public async Task<IEnumerable<JawsDTO>> GetAllJaws()
        {
            IEnumerable<Jaw> allJaws = await _jawRepository.GetAll();
            IEnumerable<JawsDTO> allJawsDTO = _mapper.Map<IEnumerable<JawsDTO>>(allJaws);

            return allJawsDTO;
        }





        /// <summary>
        /// Returns Jaw based on given id
        /// </summary>
        /// <param name="jawId">Jaw ID</param>
        /// <returns>JawsDTO</returns>
        public async Task<JawsDTO> GetJawById(Guid? jawId)
        {

            if (jawId != null)
            {
                Jaw? jaw = await _jawRepository.GetById(jawId.Value);
                if(jaw != null)
                {
                    return _mapper.Map<JawsDTO>(jaw);
                }
            }

            return new JawsDTO();
        }


        /// <summary>
        /// Creates jaw and returns the same jaws
        /// </summary>
        /// <param name="jawsDTO">JawsDTO</param>
        /// <returns>JawsDTO</returns>
        public async Task<JawsDTO> SaveJaw(JawsDTO jawsDTO)
        {          
            Jaw jaw = _mapper.Map<Jaw>(jawsDTO);
            Jaw createdJaw = await _jawRepository.Update(jaw);
            return _mapper.Map<JawsDTO>(createdJaw);
        }


        /// <summary>
        /// Delete Jaw
        /// </summary>
        /// <param name="jawId">Jaw ID</param>
        /// <returns>number of affected data</returns>
        public async Task<int> DeleteJaw(Guid jawId)
        {
            return await _jawRepository.Delete(jawId);
            
        }
    }
}
