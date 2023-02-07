using CitrsLite.Business.DTOs.ParticipantDTOs;
using CitrsLite.Business.Services;
using CitrsLite.Business.ViewModels.ParticipantViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components;

namespace CitrsLite.Controllers
{
    public class ParticipantController : Controller
    {
        private ParticipantService participantService;
        private IWebHostEnvironment appEnvironment;

        public ParticipantController(ParticipantService service, IWebHostEnvironment environment)
        {
            participantService = service;
            appEnvironment = environment;
        }

        [HttpGet]        
        public IActionResult Excel(ParticipantDetailViewModel model)
        {
            IEnumerable<ParticipantExcelDTO> data = participantService.GetParticipantExcelData(model);

            byte[] excelData = participantService.ParticipantData(data);

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            string fileName = "participants.xlsx";

            return File(excelData, contentType, fileName);
        }

        [HttpGet]
        public async Task<IActionResult> Pdf(int id)
        {
            byte[] pdfData = await participantService.GetPdfAsync(id, appEnvironment.WebRootPath);

            return File(pdfData, "application/pdf", "participant.pdf");
        }
    }
}
