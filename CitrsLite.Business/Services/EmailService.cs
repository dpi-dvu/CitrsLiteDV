using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace CitrsLite.Business.Services
{
    public class EmailService
    {
        private ParticipantService _participantService;
        private UserService _userService;
        
        public EmailService(ParticipantService participantService, UserService userService) 
        { 
            _participantService = participantService;
            _userService = userService;
        }

        public async Task EmailAsync(int participantId, string path)
        {
            try
            {
                string template = await _participantService.GetTemplateAsync(participantId, path);

                MailMessage message = new MailMessage();
                message.IsBodyHtml= true;
                message.Body = template;
                message.To.Add(new MailAddress("test@test.gov"));
                message.From = (new MailAddress("Citrs@fdacs.gov", "Citrs"));
                message.Subject = "Testing Attachment";

                using (MemoryStream stream = new MemoryStream())
                {
                    await Task.Run(() =>
                    {
                        using (PdfWriter pdfWriter = new PdfWriter(stream))
                        {
                            stream.Position = 0;                            
                            pdfWriter.IsCloseStream();
                            pdfWriter.SetCloseStream(false);
                            HtmlConverter.ConvertToPdf(template, pdfWriter);
                            byte[] document = stream.ToArray();
                            message.Attachments.Add(new Attachment(new MemoryStream(document), "participant.pdf", "application/pdf"));
                            SmtpClient client = new SmtpClient("relay.freshfromflorida.com", 25);
                            client.Send(message);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task OpenOutlookAsync(int participantId, string path)
        {
            try
            {
                string template = await _participantService.GetTemplateAsync(participantId, path);

                Outlook.Application application = new Outlook.Application();
                Outlook.MailItem mailItem = (Outlook.MailItem)application.CreateItem(Outlook.OlItemType.olMailItem);

                mailItem.Subject = "Participant Detail with Attachment";
                mailItem.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
                mailItem.HTMLBody = template;

                string filePath = System.IO.Path.Combine(path, $@"Documents/Part_Detail.pdf");
                byte[] document = await _participantService.GetPdfAsync(participantId, path);
                File.WriteAllBytes(filePath, document);
                
                mailItem.Attachments.Add(filePath, Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);
                mailItem.Display(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
