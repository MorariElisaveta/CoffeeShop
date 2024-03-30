using System.Linq;
using System.Web.Mvc;
using RiseCup.Database;

namespace RiseCup.Web.Controllers
{
    [Authorize(Roles = "Administrator")] // Доступ только для администраторов
    public class OrdersController : Controller
    {
        private readonly RiseCupContext _db;

        public OrdersController()
        {
            _db = new RiseCupContext(); // Инициализация контекста базы данных
        }

        public ActionResult Index()
        {
            var orders = _db.Orders.ToList(); // Получение всех заказов из базы данных
            return View(orders); // Передача списка заказов в представление
        }
    }
}