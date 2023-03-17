namespace P620231_API.ModelsDTOs
{
    public class UserDTO
    {
        //Un DTO (Data Transfer Object) sirve para;
        //1. Para que el equipor de desarrollo de los front ends (app en este caso)
        //no entiendan la estrutura real de la tabla a nivel de la base de datos.

        //2. Simplificar objetos complejos en estructuras mas simples para que los 
        //json resultantes sean mucho mas faciles de gestionar

        //3. En caso en los que se deba regenerar los modelos por medio de 
        //scaffold -f los controles sigan trabajando con normalidad

        //en este he decidido escribir los nombres en espanol solo por ejemplo de uso de los DTOs



        public int IDUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string NumeroTelefono { get; set; } = null!;
        public string Contrasennia { get; set; } = null!;
        public string? Cedula { get; set; }
        public string? Direccion { get; set; }
        public int IdRol { get; set; }
        public int IdEstado { get; set; }
        public string EstadoDescripcion { get; set; } = null!;
        public string RolDescripcion { get; set; } = null!;



    }
}
