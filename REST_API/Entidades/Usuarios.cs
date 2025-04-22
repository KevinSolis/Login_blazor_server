namespace REST_API.Entidades
{
    public class Usuarios
    {
        public int USRS_ID { get; set; }
        public string USRS_NOMBRE { get; set; }
        public string USRS_CORREO { get; set; }
        public string USRS_CLAVE { get; set; }
        public string USRS_ESTADO { get; set; }
    }

    public class UsuariosLogin
    {
        public string USRS_CORREO { get; set; }
        public string USRS_CLAVE { get; set; }
    }

    public class UsuariosInsert
    {
        public string USRS_NOMBRE { get; set; }
        public string USRS_CORREO { get; set; }
        public string USRS_CLAVE { get; set; }
    }

    public class UsuariosActClave
    {
        public string USRS_CORREO { get; set; }
        public string USRS_CLAVE { get; set; }
    }
}
