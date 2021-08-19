using ASP.Net_MVC_Educational.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_MVC_Educational.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CarStoreDbContext db;

        /// <summary>
        /// Конструктор контроллера 
        /// </summary>
        /// <param name="logger">логирование</param>
        /// <param name="context">контекст подключения к базе данных</param>
        public HomeController(ILogger<HomeController> logger, CarStoreDbContext context)
        {
            _logger = logger;
            db = context;
        }

        // Название метода ассоциируется с названием компонента View - контроллер загружает представление
        // с таким же названием
        public IActionResult Index()
        {
            //return View();

            // Передача данных в представление 
            return View(db.Cars.ToList());
        }

        // Если запрос Home/Privacy - вызовется этот метод, и он передаст файл Privacy.cshtml
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CustomPage()
        {
            return View();
        }


        // Благодаря аннотации мы позволяем системе выбрать нужный метод в зависимости от типа запроса;

        // Здесь используются оба метода
        // При переходе на главной странице по ссылке "/Home/Buy/1" контроллер будет получать запрос к первому действию Buy,
        // передавая ему в качестве параметра id значение 1. И так как такой запрос представляет тип GET, пользователю 
        // будет возвращаться представление с формой для ввода данных.
        [HttpGet]
        public IActionResult Buy(int? id)
        {
            // Если Id не передан - идет переадресация на вызов страницы Index()
            if (id == null) return RedirectToAction("Index");

            // ViewBag представляет объект, позволяющий определить любую переменную и передать ей некоторое значение, а затем
            // в представлении извлечь это значение. С помощью этого ViewBag переносится id автомобиля с Index.cshtml на Buy.cshtml
            ViewBag.CarId = id;
            return View();
        }

        // После нажатия на кнопку "отправить" с формы, вызванной предыдущим методом, выполняется запрос типа POST,
        // отправляющий данные на сервер, и выводящий сообщение на новой странице
        [HttpPost]
        public string Buy(Order order)
        {
            // на форме у нас простые поля ввода, но в post-версии метода Buy мы получаем именно модель Order.
            // Для связывания приходящих на сервер данных с параметрами методов фреймворк MVC использует специальный
            // компонент привязчик модели (model binder), который по названию пришедших данных сопоставит их со
            // свойствами модели Order и создаст объект этой модели.
            db.Orders.Add(order);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо, " + order.User + ", за покупку!";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
