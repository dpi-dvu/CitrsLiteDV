using CitrsLite.Business.DTOs.ParticipantDTOs;
using CitrsLite.Business.Services;
using CitrsLite.Business.ViewModels.ParticipantViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CitrsLite.Controllers
{
    public class ParticipantController : Controller
    {
        private ParticipantService participantService;

        public ParticipantController(ParticipantService service)
        {
            participantService = service;
        }

        [Route("participant/excel")]        
        public IActionResult ParticipantExcel(ParticipantDetailViewModel model)
        {
            IEnumerable<ParticipantExcelDTO> data = participantService.GetParticipantExcelData(model);

            Response.Clear();

            byte[] excelData = participantService.ParticipantData(data);

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            string fileName = "participants.xlsx";

            return File(excelData, contentType, fileName);
        }
    }
}
