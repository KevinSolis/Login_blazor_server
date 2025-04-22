using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using REST_API.Entidades;

namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private string? _cadenaConexion;

        public BaseController(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("MySqlConnection");
        }

        [HttpGet]
        [Route("ObtenerUsuarios")]
        public IActionResult ObtenerUsuarios()
        {
            var listaUsuarios = new List<Usuarios>();

            using var conexion = new MySqlConnection(_cadenaConexion);
            using var comando = new MySqlCommand("sp_ObtenerUsuarios", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            try
            {
                conexion.Open();
                using var reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var usuario = new Usuarios
                    {
                        USRS_ID = reader.GetInt32("USRS_ID"),
                        USRS_NOMBRE = reader.GetString("USRS_NOMBRE"),
                        USRS_CORREO = reader.GetString("USRS_CORREO"),
                        USRS_CLAVE = reader.GetString("USRS_CLAVE"),
                        USRS_ESTADO = reader.GetString("USRS_ESTADO")
                    };
                    listaUsuarios.Add(usuario);
                }
                conexion.Close();

                return Ok(listaUsuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("InsertarUsuario")]
        public IActionResult InsertarUsuario([FromBody] UsuariosInsert usuario)
        {
            using var conexion = new MySqlConnection(_cadenaConexion);
            using var comando = new MySqlCommand("sp_InsertarUsuario", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            try
            {
                comando.Parameters.AddWithValue("P_NOMBRE", usuario.USRS_NOMBRE);
                comando.Parameters.AddWithValue("P_CORREO", usuario.USRS_CORREO);
                comando.Parameters.AddWithValue("P_CLAVE", usuario.USRS_CLAVE);

                
                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    return Ok("Usuario insertado correctamente");
                    
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    return BadRequest(ex.Message);
                    
                }
                
                
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("LoginUsuario")]
        public IActionResult LoginUsiario([FromBody] UsuariosLogin usuario)
        {
            using var conexion = new MySqlConnection(_cadenaConexion);
            using var comando = new MySqlCommand("sp_LoginUsuario", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            try
            {
                comando.Parameters.AddWithValue("P_CORREO", usuario.USRS_CORREO.Trim());
                comando.Parameters.AddWithValue("P_CLAVE", usuario.USRS_CLAVE.Trim());
                conexion.Open();
                var reader = comando.ExecuteReader();
                reader.Read();
                int aa = reader.GetInt32("EXISTE");
                bool existe = false;
                if (aa == 1)
                {
                    existe = true;
                }
                else
                {
                    existe = false;
                }

                conexion.Close();
                return Ok(existe);


            }
            catch (Exception ex) 
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("ActualizarClaveUsuario")]
        public IActionResult ActualizarClaveUsuario([FromBody] UsuariosActClave usuario)
        {
            using var conexion = new MySqlConnection(_cadenaConexion);
            using var comando = new MySqlCommand("sp_ActualizarClaveUsuario", conexion);
            comando.CommandType = CommandType.StoredProcedure;
            try
            {
                comando.Parameters.AddWithValue("P_CORREO", usuario.USRS_CORREO.Trim());
                comando.Parameters.AddWithValue("P_CLAVE", usuario.USRS_CLAVE.Trim());
                conexion.Open();
                comando.ExecuteNonQuery();
                conexion.Close();
                return Ok("Clave actualizada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
