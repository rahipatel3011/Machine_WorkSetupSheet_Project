using Machine_Setup_Worksheet.Models.DTOs;

namespace Machine_Setup_Worksheet.Services.IServices
{
    public interface ISetupService
    {

        /// <summary>
        /// Returns all Setups associated with provided WorkSetup
        /// </summary>
        /// <param name="WorkSetupId">WorkSetup Id</param>
        /// <returns><IEnumerable<SetupDTO></returns>
        Task<IEnumerable<SetupDTO>> GetAllSetupsAsync(Guid WorkSetupId);


        /// <summary>
        /// Get Setup using id 
        /// </summary>
        /// <param name="id">Setup Id</param>
        /// <returns>SetupDTO</returns>
        Task<SetupDTO> GetBySetupIdAsync(Guid id);


        /// <summary>
        /// Save Setup
        /// </summary>
        /// <param name="setupDTO">SetupDTO</param>
        /// <returns>SetupDTO</returns>
        Task<SetupDTO> SaveSetup(SetupDTO setup);


        /// <summary>
        /// Delete Setup with given id
        /// </summary>
        /// <param name="id">SetupId</param>
        /// <returns>number of affected recored</returns>
        Task<int> DeleteSetup(Guid id, Guid workSetupId);
    }
}
