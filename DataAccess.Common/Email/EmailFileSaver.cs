using System;
using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace DataAccess.Common.Email
{
    public class EmailFileSaver
    {
        public static void SaveMailMessageToFile(MailMessage mm, string file)
        {
            Assembly assembly = typeof (SmtpClient).Assembly;
            Type mwt = assembly.GetType("System.Net.Mail.MailWriter");

            using (var stream = new FileStream(file, FileMode.Create))
            {
                ConstructorInfo mwc = mwt.GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic, null, new[] {typeof (Stream)}, null);

                object mw = mwc.Invoke(new object[] {stream});

                MethodInfo send = typeof (MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);
                send.Invoke(mm, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] {mw, true, true}, null);

                MethodInfo close = mw.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);
                close.Invoke(mw, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] {}, null);
            }
        }
    }
}