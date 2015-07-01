using System.Web;
using Autofac;
using DataAccess.Common;
using DataAccess.Common.Email;
using DataAccess.Common.Util;
using log4net;

namespace ResourceMetadata.API
{
    /// <summary>
    /// </summary>
    public static class ApplicationDependency
    {
        /// <summary>
        /// </summary>
        /// <param name="containerBuilder"></param>
        public static void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ConfigurationFiles>().WithParameter("root", HttpContext.Current.Server.MapPath("~/"));
            containerBuilder.Register<LogUtils>(ctx => ctx.Resolve<ConfigurationFiles>().CreateLogUtils());
            containerBuilder.Register<ApplicationSettings>(ctx => ctx.Resolve<ConfigurationFiles>().LoadSettings());
            containerBuilder.Register(ctx => ctx.Resolve<LogUtils>().GetLogger(typeof(LogUtils))).As<ILog>().SingleInstance();
            //Email service inject
            containerBuilder.RegisterType<EmailSender>().As<IEmailSender>();
            containerBuilder.Register<EmailFactory>(ctx =>
            {
                var logUtils = ctx.Resolve<LogUtils>();
                var settings = ctx.Resolve<ApplicationSettings>();
                var emailSender=ctx.Resolve<IEmailSender>();
                return new EmailFactory(
                    logUtils.GetLogger(typeof(EmailFactory)),
                    emailSender,
                    settings.SMTPServer,
                    settings.FromEmail,
                    null,
                    settings.SMTPUser,
                    settings.SMTPPassword
                    );
            });

        }
    }
}