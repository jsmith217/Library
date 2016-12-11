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
            return this._readersRepo.Select(null);
        }

        public ReaderModel GetById(int id)
        {
            return this._readersRepo.Select(new List<Pair> { new Pair("r.Id", "=", $"{id}") }).First();
        }

        public ReaderModel GetByDetails(ReaderModel reader)
        {
            string passwordCondition = $"Password={reader.Password}" ?? "Password IS NULL";
            return this._readersRepo.Select(new List<Pair>
            {
                new Pair("Email", "=", $"{reader.Email}"),
                
            }).First();
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