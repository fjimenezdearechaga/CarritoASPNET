namespace _2024_1C_carritoDeCompras.Models
{
    public class Cliente : Persona
    {
        public Cliente() {  }

        public List<Compra> Compras { get; set; }
        public List <Carrito> Carritos {  get; set; }   
    }
}
