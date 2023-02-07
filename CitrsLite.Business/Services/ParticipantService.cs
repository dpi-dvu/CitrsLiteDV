using CitrsLite.Business.Repositories;
using CitrsLite.Business.ViewModels.ParticipantViewModels;
using CitrsLite.Data.Models;
using OfficeOpenXml;
using CitrsLite.Business.DTOs.ParticipantDTOs;
using iText.Kernel.Pdf;
using iText.Html2pdf;


namespace CitrsLite.Business.Services
{
    public class ParticipantService
    {
        private IUnitOfWork _data;

        public ParticipantService(string connectionString)
        {
            _data= new UnitOfWork(connectionString);
        }

        public Participant GetParticipantFromForm(ParticipantFormViewModel formModel)
        {
            return new Participant()
            {
                Name = formModel.Name,
                Type = formModel.Type,
                Description = formModel.Description,
                PhoneNumber = formModel.PhoneNumber,
                Address = formModel.Address,
                City = formModel.City,
                State = formModel.State,
                IsActive = true,
                CreatedBy = formModel.UserName,
                CreationDate = DateTime.Now,
                ModifiedBy = formModel.UserName,
            };
        }

        public ParticipantDetailViewModel GetParticipant(int id)
        {
            Participant part = _data.Participants.GetFirst(p => p.Id == id);

            return new ParticipantDetailViewModel()
            {
                Id = part.Id,
                Name = part.Name,
                Type = part.Type,
                Description = part.Description ?? "No description given",
                PhoneNumber = part.PhoneNumber,
                Address = part.Address,
                City = part.City,
                State = part.State,
                IsActive = part.IsActive
            };
        }

        public void Create(ParticipantFormViewModel formModel)
        {
            Participant participant = GetParticipantFromForm(formModel);

            _data.Participants.Create(participant);
            _data.SaveChanges();
            
        }

        public async void CreateAsync(ParticipantFormViewModel formModel)
        {
            Participant participant = GetParticipantFromForm(formModel);

            await _data.Participants.CreateAsync(participant);

            await _data.SaveChangesAsync();
        }

        public IEnumerable<Participant> GetList()
        {
            return _data.Participants.GetList();
        }

        public async Task<IEnumerable<Participant>> GetListAsync()
        { 
            return await _data.Participants.GetListAsync();
        }

        public void Update(ParticipantFormViewModel model, string userName)
        {
            try
            {
                Participant participant = _data.Participants.GetFirst(p => p.Id == model.Id);

                participant.Name = model.Name;
                participant.Type = model.Type;
                participant.Address = model.Address;
                participant.PhoneNumber = model.PhoneNumber;
                participant.City = model.City;
                participant.State = model.State;
                participant.Description= model.Description;

                participant.ModificationDate = DateTime.Now;
                participant.ModifiedBy = userName;

                _data.Participants.Edit(participant);
                _data.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update Participant Error");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public IEnumerable<ParticipantExcelDTO> GetParticipantExcelData(ParticipantDetailViewModel request)
        {
            try
            {
                IEnumerable<Participant> participants;

                if (request.Name == null)
                {
                    participants = _data.Participants.GetList(); 
                } 
                else
                {
                    participants = _data.Participants.GetList()
                    .Where(p => p.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
                }
                IEnumerable<ParticipantExcelDTO> response = participants
                    .Select(participant => new ParticipantExcelDTO()
                    {
                        Id = participant.Id,
                        Name = participant.Name,
                        Type = participant.Type,
                        Description = participant.Description,
                        PhoneNumber = participant.PhoneNumber,
                        Address = participant.Address,
                        City = participant.City,
                        State = participant.State,
                        IsActive = participant.IsActive,
                    });

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public byte[] ParticipantData(IEnumerable<ParticipantExcelDTO> data)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Participants");

                    sheet.Cells["A1"].Value = "Participant Id";
                    sheet.Cells["B1"].Value = "Name";
                    sheet.Cells["C1"].Value = "Participant Type";
                    sheet.Cells["D1"].Value = "Description";
                    sheet.Cells["E1"].Value = "Phone Number";
                    sheet.Cells["F1"].Value = "Address";
                    sheet.Cells["G1"].Value = "City";
                    sheet.Cells["H1"].Value = "State";
                    sheet.Cells["I1"].Value = "Is Active";

                    int row = 2;

                    foreach (ParticipantExcelDTO participant in data)
                    {
                        sheet.Cells[string.Format("A{0}", row)].Value = participant.Id;
                        sheet.Cells[string.Format("B{0}", row)].Value = participant.Name;
                        sheet.Cells[string.Format("C{0}", row)].Value = participant.Type;
                        sheet.Cells[string.Format("D{0}", row)].Value = participant.Description ?? "No Description Provided";
                        sheet.Cells[string.Format("E{0}", row)].Value = participant.PhoneNumber;
                        sheet.Cells[string.Format("F{0}", row)].Value = participant.Address;
                        sheet.Cells[string.Format("G{0}", row)].Value = participant.City;
                        sheet.Cells[string.Format("H{0}", row)].Value = participant.State;
                        sheet.Cells[string.Format("I{0}", row)].Value = participant.IsActive ? "Active" : "Inactive";

                        row++;
                    }

                    sheet.Cells["A:BZ"].AutoFitColumns();

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<byte[]> GetPdfAsync(int id, string path)
        {
            Participant participant = await _data.Participants.GetFirstAsync(p => p.Id == id);

            string templateString = "";

            using (StreamReader reader = new StreamReader(path + "/Templates/ParticipantTemplate.html"))
            {
                templateString = reader.ReadToEnd();
            }

            templateString = templateString.Replace("(Name)", participant.Name);
            templateString = templateString.Replace("(Type)", participant.Type);
            templateString = templateString.Replace("(Description)", participant.Description ?? "No Description Given");

            using (MemoryStream stream = new MemoryStream())
            {
                await Task.Run(() =>
                {
                    using (PdfWriter pdfWriter = new PdfWriter(stream))
                    {
                        HtmlConverter.ConvertToPdf(templateString, pdfWriter);
                    }
                });

                return stream.ToArray();
            }
        }
    }
}
