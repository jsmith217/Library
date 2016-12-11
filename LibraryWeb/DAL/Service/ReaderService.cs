using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml;
using System.Net.Mail;
using LibraryWeb.Models.Readers;
using LibraryWeb.Repository;

namespace LibraryWeb.Service
{
    public class ReaderService : ICommonQueries<ReaderModel>
    {
        private ReadersRepository _readersRepo;

        public ReaderService()
        {
            this._readersRepo = new ReadersRepository();
        }

        public List<ReaderModel> GetAllReaders()
        {
            return this._readersRepo.LightWeightSelect(null);
        }

        public ReaderModel GetById(int id)
        {
            return this._readersRepo.LightWeightSelect(new List<string> { $"Id={id}" }).First();
        }

        public void SendMail(ReaderModel reader)
        {
            using (SmtpClient client = new SmtpClient())
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("MailConfig.xml");
                XmlNode rootNode = doc.SelectSingleNode("mailConfig");
                var address = rootNode.ChildNodes[0].Attributes["address"].Value;
                var password = rootNode.ChildNodes[0].Attributes["passwors"].Value;

                client.Credentials = new System.Net.NetworkCredential(address, password);
                client.UseDefaultCredentials = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                // ?
                client.EnableSsl = true;
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(address);
                    message.To.Add(new MailAddress(reader.Email));
                    message.Body = "You have taken a book.";

                    client.Send(message);
                }
            }
        }
    }
}