using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_MVC_Educational.Models
{
    // Чаще всего, в MVC в моделях хранятся классы, в которых находятся только свойства, БЕЗ поведения
    // (анемичная модель), поскольку управление поведением осуществляет контроллер.
    // Классический класс, содержащий поведение, называется "толстая модель"
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }
    }
}
