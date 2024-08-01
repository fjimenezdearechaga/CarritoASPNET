namespace _2024_1C_carritoDeCompras
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = Startup.InicializarApp(args); //Pasamos los argumentos que son recibidos en la ejecucion.
            app.Run();
        }
    }
}
