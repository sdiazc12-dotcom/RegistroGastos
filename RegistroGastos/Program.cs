namespace RegistroGastos
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string ruta = "gastos.csv";
            string opcion;
            List<Gasto> gastos = Cargar(ruta);
            int SiguienteId = 1;
            foreach (Gasto g in gastos)
            {
                if (g.Id >= SiguienteId)
                {
                    SiguienteId = g.Id + 1;
                }
            }

            do
            {
                opcion = LeerOpcionMenu();

                switch (opcion)
                {
                    case "1":
                        Agregar(gastos, ref SiguienteId);
                        break;
                    case "2":
                        Listar(gastos);
                        break;
                    case "3":
                        BuscarCategoria(gastos);
                        break;
                    case "4":
                        Guardar(gastos, ruta);
                        Console.WriteLine("Saliendo...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            while (opcion != "4");
        }

        private static string LeerOpcionMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== REGISTRO DE GASTOS ===");
            Console.WriteLine("1. Agregar gasto");
            Console.WriteLine("2. Listar gastos");
            Console.WriteLine("3. Buscar por categoría");
            Console.WriteLine("4. Salir");
            Console.Write("Elige una opción: ");
            return Console.ReadLine();
        }

        private static void Agregar(List<Gasto> gas, ref int id)
        {
            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine().Trim();
            Console.Write("Monto: ");
            bool montoOk = decimal.TryParse(Console.ReadLine(), out decimal monto);
            Console.Write("Categoría: ");
            string categoria = Console.ReadLine().Trim();

            if (!montoOk || monto <= 0 || string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("Datos inválidos.");
                return;
            }
            else
            {
                gas.Add(new Gasto
                {
                    Id = id,
                    Descripcion = descripcion,
                    Monto = monto,
                    Categoria = categoria,
                });
                id++;
                Console.WriteLine("Gasto agregado.");
            }
        }

        private static void Listar(List<Gasto> gas)
        {
            if (gas.Count == 0)
            {
                Console.WriteLine("No hay gastos.");
                return;
            }
            decimal totalGeneral = 0;
            foreach (Gasto g in gas)
            {
                Console.WriteLine(g);
                totalGeneral += g.Monto;
            }

            Console.WriteLine($"TOTAL GASTADO: Q {totalGeneral:N2}");
        }

        private static void BuscarCategoria(List<Gasto> gas)
        {
            Console.Write("Categoría a buscar: ");
            string t = Console.ReadLine().Trim().ToUpper();
            bool encontrado = false;
            foreach (Gasto g in gas)
            {
                if (g.Categoria.ToUpper().Contains(t))
                {
                    Console.WriteLine($"- {g}");
                    encontrado = true;
                }
            }

            if (!encontrado)
            {
                Console.WriteLine("Sin coincidencias");
            }
        }

        private static void Guardar(List<Gasto> gas, string ruta)
        {
            var lineas = new List<string>();

            foreach (Gasto g in gas)
            {
                lineas.Add($"{g.Id};{g.Descripcion};{g.Monto};{g.Categoria}");
            }

            File.WriteAllLines(ruta, lineas);
            Console.WriteLine($"Gastos guardados en {ruta} ({gas.Count} registros).");
        }

        private static List<Gasto> Cargar(string ruta)
        {
            var gas = new List<Gasto>();
            if (!File.Exists(ruta)) return gas;

            foreach (string linea in File.ReadLines(ruta))
            {
                string[] campos = linea.Split(';');

                if (campos.Length != 4) continue;

                gas.Add(new Gasto
                {
                    Id = int.Parse(campos[0]),
                    Descripcion = campos[1],
                    Monto = decimal.Parse(campos[2]),
                    Categoria = campos[3],
                });
            }

            Console.WriteLine($"Cargados {gas.Count} gastos desde {ruta}.");

            return gas;
        }
    }
}