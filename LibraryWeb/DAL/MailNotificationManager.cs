using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LibraryWeb
{
    public class MailNotificationSettings : ConfigurationSection
    {
        private static MailNotificationSettings _settings
            = ConfigurationManager.GetSection("MailSettings") as MailNotificationSettings;

        public static MailNotificationSettings Settings => _settings;

        [ConfigurationProperty("address", IsRequired = true)]
        [StringValidator(MaxLength = 30)]
        public string Adress {
            get { return this["address"].ToString(); }
            set{ this["address"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        [StringValidator(MaxLength = 30)]
        public string Password
        {
            get{ return this["password"].ToString(); }
            set{ this["password"] = value; }
        }
    }
}