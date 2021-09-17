using ASP.Net_MVC_Educational.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ASP.Net_MVC_Educational.ActionResults;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASP.Net_MVC_Educational.Controllers
{
    //Центральным звеном в архитектуре ASP.NET Core MVC является контроллер. При получении запроса система маршрутизации
    //выбирает для обработки запроса нужный контроллер и передает ему данные запроса. Контроллер обрабатывает эти данные
    //и посылает обратно результат обработки.

    //Чтобы обратиться контроллеру из веб-браузера, нам надо в адресной строке набрать адрес_сайта/Имя_контроллера/Действие_контроллера.
    //Так, по запросу адрес_сайта/Home/Index система маршрутизации по умолчанию вызовет метод Index контроллера HomeController
    //для обработки входящего запроса.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly CarStoreDbContext db;
        private readonly IWebHostEnvironment _appEnvironment;


        // В контроллер можно передавать сервисы и использовать их в методах контроллера, пример сервисов в проекте ASP.NET Core


        // Переопределение контроллера: перекрытие пустого метода, который исполняется ДО вызова какого-либо метода контроллера;
        // В данном случае при вызове метода контроллера будет проверяться, не является ли браузер клиента Internet Explorer
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.ContainsKey("User-Agent"))
            {
                // получаем заголовок User-Agent
                var useragent = context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
                // сравниваем его значение
                if (useragent.Contains("MSIE") || useragent.Contains("Trident"))
                {
                    context.Result = Content("Internet Explorer не поддерживается");
                }
            }
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Конструктор контроллера 
        /// </summary>
        /// <param name="logger">логирование</param>
        /// <param name="context">контекст подключения к базе данных</param>
        /// <param name="appEnvironment">сервис, содержащий физический путь к каталогу проекта, нужен для отправки файлов</param>
        public HomeController(ILogger<HomeController> logger, CarStoreDbContext context, IWebHostEnvironment appEnvironment)
        {
            this.logger = logger;
            db = context;
            _appEnvironment = appEnvironment;
        }

        // Название метода (ДЕЙСТВИЯ контроллера) ассоциируется с названием компонента View - контроллер 
        // загружает представление с таким же названием. ВАЖНО: сопоставление происходит в маршрутизаторе
        // app.UseEndpoints
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

        [NonAction] // НЕ РАБОТАЕТ: НЕ РАССМАТРИВАЕТСЯ КАК ДЕЙСТВИЕ
        public IActionResult Hi()
        {
            return View();
        }

        // НЕ РАБОТАЕТ: модификатор доступа не public. Тем не менее, закрытые методы полезны для промежуточных вычислений.
        protected internal string Hello()
        {
            return "Hello ASP.NET";
        }
        // Благодаря аннотации мы позволяем системе выбрать нужный метод в зависимости от типа запроса;

        // Здесь используются оба метода
        // При переходе на главной странице по ссылке "/Home/Buy/1" контроллер будет получать запрос к первому действию Buy,
        // передавая ему в качестве параметра id значение 1. И так как такой запрос представляет тип GET, пользователю 
        // будет возвращаться представление с формой для ввода данных.
        // Интерфейс IActionResult содержит методы возвращения результата на запроса пользователя (на введенный адрес)
        [HttpGet] //Если атрибут явным образом не указан, то метод может обрабатывать все типы запросов: GET, POST, PUT, DELETE.
        public IActionResult Buy(int? id)
        {
            // Если Id не передан - идет переадресация на вызов страницы Index()
            if (id == null) return RedirectToAction("Index");

            // ViewBag представляет объект, позволяющий определить любую переменную и передать ей некоторое значение, а затем
            // в представлении извлечь это значение. С помощью этого ViewBag переносится id автомобиля с Index.cshtml на Buy.cshtml
            ViewBag.CarId = id;
            return View();
        }

        // Вывод кастомного результата на действие пользователя, результат представляет из себя КЛАСС - наследник IActionResult;
        // в классе уже прописано, что именно нужно выводить на страницу; в данном случае в класс вшита html разметка
        public SimpleCustomResult ShowResult()
        {
            return new SimpleCustomResult("Hello World!!! Custom action result here!!!");

            //// Если метод возвращает IActionResult, можно использовать множество уже встроенных вариантов ответа пользователю
            //// Здесь и варианты ответов с кодам ошибок, и переадресация на другие страницы
            //return new OkResult();
            //return new NotFoundResult();
        }

        // Можно возвращать Json в ответ на определенный запрос. Указываем определенные данные, которые сериализуем методом Json
        public JsonResult ShowJson()
        {
            //return Json("Hello world");
            ClassForJsonResult jsonClass = new ClassForJsonResult { Property1 = "Hello", Property2 = 125, Property3 = "Hi!" };
            return Json(jsonClass);
        }

        // После нажатия на кнопку "отправить" с формы, вызванной предыдущим методом, выполняется запрос типа POST,
        // отправляющий данные на сервер, и выводящий сообщение на новой странице;
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

        // Метод контроллера (действие контроллера) может проводить вычисления с переданными аргументами и сразу выводить 
        // результат на view; Home/Square?a=10&h=3
        // Home/Square вызовет параметры по умолчанию, присвоенные аргументам при их инициализации
        // ДОПОЛНЕНИЕ: можно передавать вместо примитивных аргументов нечто более сложное, например класс, а в строке запроса
        // будут указаны его свойства
        // ДОПОЛНЕНИЕ: можно передавать массив объектов, где объект может быть любым типом данных
        public string Square(int a = 3, int h = 10)
        {
            double s = a * h / 2;
            return $"Площадь треугольника с основанием {a} и высотой {h} равна {s}";
        }

        // Если в аргументы ничего не передается, но передается в строку, мы можем "выдернуть" данные из строки запроса,
        // и использовать их как нам угодно
        public string Area()
        {
            string altitudeString = Request.Query.FirstOrDefault(p => p.Key == "altitude").Value;
            int altitude = Int32.Parse(altitudeString);

            string heightString = Request.Query.FirstOrDefault(p => p.Key == "height").Value;
            int height = Int32.Parse(heightString);

            double square = altitude * height / 2;
            return $"Площадь треугольника с основанием {altitude} и высотой {height} равна {square}";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // ПЕРЕАДРЕСАЦИЯ
        // Переадресация бывает временная (код 302, запрашиваемый объект временно перемещен, но url нигде не меняется)
        // и постоянная (код 301, запрашиваемый объект считается навсегда перенесенным в другое место, url всюду должен быть заменен)
        public IActionResult CheckRedirect()
        {
            // При обращении к Home/CheckRedirect моментально перенаправит на Home/Index, переадресация помечается как временная
            //return Redirect("~/Home/Index");

            // Переадресация помечается как постоянная
            //return RedirectPermanent("http://microsoft.com");

            // Есть перегрузка, позволяющая перенаправить на метод ДРУГОГО КОНТРОЛЛЕРА, а не текущего
            //return RedirectToAction(string actionName, string controllerName);

            // Можно делать сложные перенаправления, например направить на метод принимающий параметры, сразу указывая в методе
            // перенаправления нужные параметры; также можно перегрузить с указанием конкретного контроллера
            return RedirectToAction("Square", new { altitude = 10, height = 3 });
        }



        // СТАТУСНЫЕ КОДЫ
        // Любой код по желанию (???)
        public IActionResult GiveCustomStatus()
        {
            return StatusCode(315);
        }

        // Не найдено (404)
        public IActionResult GiveStatus()
        {
            //return StatusCode(401);

            // Отправка объекта "не найдено"
            return NotFound("Ничего не найдено");
        }

        // Отказ в авторизации (401)
        public IActionResult GiveAgeStatus(int age)
        {
            if (age < 21)
            {
                // Если нужно, можно дополнительно приписать в этот объект сообщение
                return Unauthorized();
            }
            else return Content("Проверка пройдена");
        }

        // Неправильный запрос (400)
        public IActionResult WrongRequest(string addition)
        {
            if (String.IsNullOrEmpty(addition))
            {
                // Если нужно, можно дополнительно приписать в этот объект сообщение
                return BadRequest();
            }
            else return View();
        }

        // Успешно (200)
        public IActionResult GiveOk()
        {
            // Если нужно, можно дополнительно приписать в этот объект сообщение
            return Ok("Норм!");
        }


        // ОТПРАВКА ФАЙЛОВ КЛИЕНТУ
        // Отправка физического файла
        public IActionResult GetTxtFile()
        {
            // Путь к файлу
            string filePath = Path.Combine(_appEnvironment.ContentRootPath, "FilesToSend/TxtForClient.txt");
            // Тип файла - content-type
            string fileType = "application/txt";
            // Имя файла - необязательно
            string fileName = "TxtForClient.txt";
           
            return PhysicalFile(filePath, fileType, fileName);
        }

        // Отправка массива байтов (например, данные отправляются из базы, где тип byte[])
        public FileResult GetBytes()
        {
            string path = Path.Combine(_appEnvironment.ContentRootPath, "FilesToSend/TxtForClient.txt");
            byte[] bytesArr = System.IO.File.ReadAllBytes(path);
            string fileType = "application/txt";
            string fileName = "TxtForClient.txt";
            return File(bytesArr, fileType, fileName);

            //// ИЛИ можно использовать filestream, с помощью которого можно генерировать файл "на лету", не сохраняя его физически
            //// на дисковом пространстве
            //FileStream fs = new FileStream(path, FileMode.Open);
            //return File(fs, fileType, fileName);
        }

        // Отправка файла по виртуальному пути (требуется, чтобы файл находился в каталоге wwwroot)
        public VirtualFileResult GetVirtualFile()
        {
            var filePath = Path.Combine("~/Files", "hello.txt");
            return File(filePath, "text/plain", "hello.txt");
        }

        // Дополнение: "application/octet-stream" - универсальный идентификатор типа файла, использовать если точно не известен тип


        
        // Если сервис используется только 1 раз, нет необходимости передавать его в контроллер, можно сразу внедрить в метод 
        public string Index([FromServices] ITimeService timeService)
        {
            return timeService.Time;
        }












    }
}
