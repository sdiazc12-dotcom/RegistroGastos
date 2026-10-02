using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroGastos
{
    public class Gasto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = "";
        public decimal Monto { get; set; }
        public string Categoria { get; set; } = "";

        public override string ToString()
        {
            return $"{Id,-3} {Descripcion,-20} Q {Monto,8:N2} {Categoria}";
        }
    }
}