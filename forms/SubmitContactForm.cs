<%@ WebHandler Language = "C#" Class="SubmitForm" %>

using System;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

public class SubmitForm : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.TrySkipIisCustomErrors = true;

        // 1. Enforce POST Method
        if (context.Request.HttpMethod != "POST")
        {
            context.Response.StatusCode = 405;
            context.Response.Write("{\"success\": false, \"message\": \"Method not allowed\"}");
            return;
        }

        try
        {
            // 2. Read Payload
            string jsonString;
            using (var reader = new StreamReader(context.Request.InputStream))
            {
                jsonString = reader.ReadToEnd();
            }

            // Limit payload size (e.g., max 50KB to prevent Denial of Service)
            if (jsonString.Length > 50000)
            {
                throw new Exception("Payload size exceeded limit.");
            }

            var serializer = new JavaScriptSerializer();
            var formData = serializer.Deserialize<FormDataModel>(jsonString);

            if (formData == null || string.IsNullOrEmpty(formData.FormTitle))
            {
                throw new Exception("Invalid submission payload.");
            }

            // 3. Build HTML Email with Context Encoding
            string safeTitle = HttpUtility.HtmlEncode(formData.FormTitle);
            string emailBody = $"<h2>Form Submission: {safeTitle}</h2>";
            emailBody += $"<p><strong>Submitted On:</strong> {DateTime.Now:f}</p><hr/>";
            emailBody += "<table border='1' cellpadding='8' cellspacing='0' style='border-collapse:collapse; width:100%;'>";

            if (formData.Fields != null)
            {
                foreach (var item in formData.Fields)
                {
                    // HTML Encode Keys and Values to prevent XSS in email client
                    string safeKey = HttpUtility.HtmlEncode(item.Key ?? "");
                    string safeValue = HttpUtility.HtmlEncode(item.Value ?? "");

                    emailBody += $"<tr><td style='background:#f4f4f4; width:30%;'><strong>{safeKey}</strong></td>";
                    emailBody += $"<td>{safeValue}</td></tr>";
                }
            }
            emailBody += "</table>";

            // 4. Configure & Validate Email
            using (MailMessage mail = new MailMessage())
            using (SmtpClient smtp = new SmtpClient())
            {
            mail.From = new MailAddress("noreply@easitpa.com", "Website Forms");
            // mail.To.Add("sonm@easitpa.com"); // Hardcoded TPA Address
            mail.To.Add("ereed@easitpa.com"); // for testing

            // Validate User Email before adding to ReplyTo / CC
            if (!string.IsNullOrWhiteSpace(formData.UserEmail) && IsValidEmail(formData.UserEmail))
            {
                mail.ReplyToList.Add(new MailAddress(formData.UserEmail));
                if (formData.SendUserCopy)
                {
                    mail.CC.Add(formData.UserEmail);
                }
            }

            mail.Subject = $"Form Submission: {safeTitle}";
            mail.Body = emailBody;
            mail.IsBodyHtml = true;

            // 5. Send Email via local/network SMTP
            smtp.Send(mail);
            }

            context.Response.Write("{\"success\": true, \"message\": \"Submitted successfully.\"}");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.Write("{\"success\": false, \"message\": \"An error occurred while processing your form.\"}");
        }
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        // Basic anti-header injection check
        if (email.Contains("\r") || email.Contains("\n")) return false;

        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
    }

    public bool IsReusable => false;

    public class FormDataModel
    {
        public string FormTitle { get; set; }
        public string UserEmail { get; set; }
        public bool SendUserCopy { get; set; }
        public System.Collections.Generic.Dictionary<string, string> Fields { get; set; }
    }
}