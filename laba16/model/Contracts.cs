using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba16.model
{
    public class Contracts
    {
        public int НомерДоговора { get; set; }
        public DateTime? ДатаДоговора { get; set; }
        public int КодПоставщика { get; set; }
        public string Комментарий { get; set; }
        public string ИмяПоставщика { get; set; }
    }
}
