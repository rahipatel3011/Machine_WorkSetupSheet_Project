using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Machine_Setup_Worksheet.Components
{
    public class ModalViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(object dtoObject, Guid? Id)
        {
            if(Id != null)
            {
                return View("Delete", Id);
            }
            return View("Modal", dtoObject);
        }
    }
}
