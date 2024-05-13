using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace BanHangThoiTrangMVC.Common
{
    public class Common
    {
        private readonly IConfiguration _configuration;

        public Common(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static bool SendMail(string name, string subject, string content,
            string toMail, IConfiguration configuration)
        {
            bool rs = false;
            try
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", //host name
                    Port = 587, //port number
                    EnableSsl = true, //whether your smtp server requires SSL
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential
                    {
                        UserName = configuration["Email"],
                        Password = configuration["PasswordEmail"]
                    }
                };

                MailMessage message = new MailMessage
                {
                    From = new MailAddress(configuration["Email"], name),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = content
                };

                message.To.Add(toMail);

                smtp.Send(message);
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                rs = false;
            }
            return rs;
        }

        public static string FormatNumber(object value, int SoSauDauPhay = 2)
        {
            bool isNumber = IsNumeric(value);
            decimal GT = 0;
            if (isNumber)
            {
                GT = Convert.ToDecimal(value);
            }
            string str = "";
            string thapPhan = "";
            for (int i = 0; i < SoSauDauPhay; i++)
            {
                thapPhan += "#";
            }
            if (thapPhan.Length > 0) thapPhan = "." + thapPhan;
            string snumformat = string.Format("0:#,##0{0}", thapPhan);
            str = String.Format("{" + snumformat + "}", GT);

            return str;
        }

        private static bool IsNumeric(object value)
        {
            return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong
                   || value is float
                   || value is double
                   || value is decimal;
        }
    }
}
