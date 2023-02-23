using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace CitrsLite.Business.Services
{
    public class EmailService
    {
        private ParticipantService _participantService;
        
        public EmailService(ParticipantService participantService) 
        { 
            _participantService = participantService;
        }

        public async Task EmailAsync(int participantId, string path)
        {
            try
            {
                string template = await _participantService.GetTemplateAsync(participantId, path);

                MailMessage message = new MailMessage();
                message.IsBodyHtml= true;
                message.Body = template;
                message.To.Add(new MailAddress("Don.Vu@fdacs.gov"));
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

        public async Task OpenOutlookAsync(int participantId, string userName, string path)
        {
            try
            {
                string template = await _participantService.GetTemplateAsync(participantId, path);

                Outlook.Application application = new Outlook.Application();
                Outlook.MailItem mailItem = (Outlook.MailItem)application.CreateItem(Outlook.OlItemType.olMailItem);

                mailItem.Subject = "Participant Detail with Attachment";
                mailItem.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
                mailItem.HTMLBody = template;

                byte[] pdf = await _participantService.GetPdfAsync(participantId, path);
                
                mailItem.Attachments.Add("C:/Users/vud/Downloads/participant-1.pdf", Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);
                mailItem.Display(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
