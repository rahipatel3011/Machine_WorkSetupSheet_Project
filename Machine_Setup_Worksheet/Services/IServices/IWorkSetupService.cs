using Machine_Setup_Worksheet.Models.DTOs;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface IWorkSetupService
    {
        /// <summary>
        /// Find all WorkSetups
        /// </summary>
        /// <returns>return IEnumerable of WorkSetupDTO</returns>
        Task<IEnumerable<WorkSetupDTO>> GetAllWorkSetupAsync();


        /// <summary>
        /// Find Work Setup using provided ID
        /// </summary>
        /// <param name="id">WorkSetupId</param>
        /// <returns>WorkSetupDTO</returns>
        Task<WorkSetupDTO> GetWorkSetupByIdAsync(Guid id);



        /// <summary>
        /// Filter WorkSetup using searchTerm
        /// </summary>
        /// <param name="searchTerm">search keywork</param>
        /// <returns><IEnumerable<WorkSetupDTO>></returns>
        Task<IEnumerable<WorkSetupDTO>> GetBySearchAsync(string searchTerm);


        /// <summary>
        /// Insert or Update WorkSetup
        /// </summary>
        /// <param name="workSetupDTO">WorkSetupDTO</param>
        /// <returns>WorkSetupDTO</returns>
        Task<WorkSetupDTO> SaveWorkSetup(WorkSetupDTO workSetup);


        /// <summary>
        /// Delete Work Setup
        /// </summary>
        /// <param name="id">WorkSetup Id</param>
        /// <returns>int(affeceted records)</returns>
        Task<int> DeleteWorkSetup(Guid id);

    }
}
