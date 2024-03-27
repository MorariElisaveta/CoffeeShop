using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RiseCup.BL.Services;
using RiseCup.Database;

namespace RiseCup.Web.Controllers
{
    /// <summary>
    /// Контроллер, управляющий входом, регистрацией и выходом пользователя.
    /// </summary>
    public class AuthController : Controller
    {
        private readonly UserManagementService _userManager;
        private readonly RiseCupContext _db;

        public AuthController()
        {
            _db = new RiseCupContext();
            _userManager = new UserManagementService(_db);
        }

        /// <summary>
        /// Отображает страницу входа.
        /// </summary>
        /// <returns>Возвращает страницу входа, если пользователь не аутентифицирован.</returns>
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// Обрабатывает попытку входа пользователя.
        /// </summary>
        /// <param name="username">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Перенаправляет на главную страницу при успешном входе, иначе возвращает на страницу входа с ошибкой.</returns>
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = _userManager.AuthenticateUser(username, password);
            if (user != null)
            {
                var authTicket = new FormsAuthenticationTicket(
                    1, user.Username, 
                    DateTime.Now, DateTime.Now.AddMinutes(20), true, user.Role, "/");
                var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        /// <summary>
        /// Отображает страницу регистрации.
        /// </summary>
        /// <returns>Страница регистрации.</returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Обрабатывает регистрацию нового пользователя.
        /// </summary>
        /// <param name="username">Логин нового пользователя.</param>
        /// <param name="password">Пароль нового пользователя.</param>
        /// <returns>Перенаправляет на главную страницу при успешной регистрации, иначе возвращает на страницу регистрации с ошибкой.</returns>
        [HttpPost]
        public ActionResult Register(string username, string password)
        {
            var user = _userManager.RegisterUser(username, password);
            if (user != null)
            {
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    user.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(20), 
                    true, 
                    user.Role,
                    "/");

                var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Unable to register. Username might be already in use.";
            return View();
        }

        /// <summary>
        /// Выполняет выход пользователя из системы.
        /// </summary>
        /// <returns>Перенаправляет на страницу входа.</returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}