using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using iText.Html2pdf;
using iText.Kernel.Pdf;

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
    }
}
