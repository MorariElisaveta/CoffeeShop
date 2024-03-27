using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace RiseCup.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                try
                {
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {
                        // Разделение ролей пользователя, указанных в билете
                        var userRoles = authTicket.UserData.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        // Создание идентичности и принципала с учетом ролей
                        var userIdentity = new FormsIdentity(authTicket);
                        var userPrincipal = new GenericPrincipal(userIdentity, userRoles);

                        // Присваивание контексту HTTP созданного принципала
                        HttpContext.Current.User = userPrincipal;
                    }
                }
                catch (CryptographicException ex)
                {
                    // Логирование ошибки расшифровки куки
                    Trace.Write("Error decrypting cookie: " + ex.Message);
                    FormsAuthentication.SignOut(); // Очистка невалидной аутентификационной куки
                }
            }
        }

    }
}