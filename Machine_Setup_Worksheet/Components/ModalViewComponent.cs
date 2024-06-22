using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Machine_Setup_Worksheet.Components
{
    /// <summary>
    /// View component for rendering modal dialogs.
    /// </summary>
    public class ModalViewComponent : ViewComponent
    {
        /// <summary>
        /// Invokes the view component asynchronously.
        /// </summary>
        /// <param name="dtoObject">The DTO object to pass to the view.</param>
        /// <param name="isDelete">Flag indicating if the modal is for delete action.</param>
        /// <returns>An asynchronous task that returns an IViewComponentResult.</returns>
        public async Task<IViewComponentResult> InvokeAsync(object dtoObject, bool isDelete)
        {
            if (isDelete)
            {
                return View("Delete", dtoObject);
            }
            else
            {
                return View("Modal", dtoObject);
            }
        }
    }
}
