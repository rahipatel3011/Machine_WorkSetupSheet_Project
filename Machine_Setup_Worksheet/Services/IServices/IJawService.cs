using Machine_Setup_Worksheet.Models.DTOs;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface IJawService
    {
        /// <summary>
        /// Returns all jaws
        /// </summary>
        /// <returns>All JawsDTO</returns>
        Task<IEnumerable<JawsDTO>> GetAllJaws();

        /// <summary>
        /// Returns Jaw based on given id
        /// </summary>
        /// <param name="jawId">Jaw ID</param>
        /// <returns>JawsDTO</returns>
        Task<JawsDTO> GetJawById(Guid? jawId);


        /// <summary>
        /// Creates jaw and returns the same jaws
        /// </summary>
        /// <param name="jawsDTO">JawsDTO</param>
        /// <returns>JawsDTO</returns>
        Task<JawsDTO> SaveJaw(JawsDTO jawsDTO);


        /// <summary>
        /// Delete Jaw
        /// </summary>
        /// <param name="jawId">Jaw ID</param>
        /// <returns>number of affected data</returns>
        Task<int> DeleteJaw(Guid jawId);
    }
}
