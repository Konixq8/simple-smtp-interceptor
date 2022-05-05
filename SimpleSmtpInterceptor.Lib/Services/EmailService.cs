using System;
using System.Collections.Generic;
using SimpleSmtpInterceptor.Data.Entities;
using Iee = SimpleSmtpInterceptor.Lib.Exceptions.InvalidEmailException;

namespace SimpleSmtpInterceptor.Lib.Services
{
    public class EmailService
        : CommonBase
    {
        private readonly Email _email;

        public EmailService(Email email)
        {
            _email = email;
        }

        public void ValidateEmail()
        {
            var dict = new Dictionary<Iee.Part, string>()
            {
                {Iee.Part.From, _email.From},
                {Iee.Part.To, _email.To},
                {Iee.Part.Subject, _email.Subject}
            };

            foreach (var kvp in dict)
            {
                if (kvp.Value != null) continue;

                //If the value is null throw an exception ahead of time to prevent saving
                throw new Iee(kvp.Key);
            }
        }

        public void SaveEmail()
        {
            /* 2022.05.05 EHH - This is to save the email to the database. For the sake of time,
               I am just going to comment this out for now and handle this better in my main branch.
               I will use dependency injection so this doesn't happen again. There are a ton of
               changes I want/need to make because I have learned a lot since I made this project
               more than three years ago. */
            //using (var context = GetContext())
            //{
            //    context.Emails.Add(_email);
            //    context.SaveChanges();
            //}

            //This is to make the entries unique. If this isn't unique enough then use a GUID.
            var section = $"Email_{DateTime.UtcNow:yyyyMMddHHmmss}";

            //Saving the email to an INI file here using the assemblies path and name. This can be changed however it is needed.
            var ini = new IniFileService();
            ini.Write(nameof(Email.From), _email.From, section);
            ini.Write(nameof(Email.To), _email.To, section);
            ini.Write(nameof(Email.Cc), _email.Cc, section);
            ini.Write(nameof(Email.Bcc), _email.Bcc, section);
            ini.Write(nameof(Email.Subject), _email.Subject, section);
            ini.Write(nameof(Email.Message), _email.Message, section);
            ini.Write(nameof(Email.HeaderJson), _email.HeaderJson, section);
            ini.Write(nameof(Email.AttachmentCount), Convert.ToString(_email.AttachmentCount), section);
        }
    }
}
