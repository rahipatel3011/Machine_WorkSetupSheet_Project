using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Machine_Setup_Worksheet.Components
{
    public class ModalViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(object dtoObject, Boolean isDelete)
        {
            if(isDelete)
            {
                return View("Delete", dtoObject);
            }
            return View("Modal", dtoObject);
        }
    }
}
