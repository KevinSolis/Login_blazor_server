using System.Text;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using REST_API.Entidades;


namespace LOGIN_USUARIOS.Components.Pages
{
    public partial class Home // Removed the circular dependency by ensuring no inheritance from itself.
    {
        [SupplyParameterFromForm]
        private UsuariosLogin? usuarioLogin { get; set; }

        [SupplyParameterFromForm]
        private UsuariosInsert? nuevoUsuario { get; set; }

        public string mensaje;
        public HttpClient Http = new HttpClient();

        private bool mostrarFormularioCreacion = false;

        protected override async Task OnInitializedAsync()
        {
            usuarioLogin ??= new UsuariosLogin();
            nuevoUsuario ??= new UsuariosInsert();
            mensaje = string.Empty;
            //MostrarFormularioLogin();
            StateHasChanged();
        }

        private void MostrarFormularioCreacion()
        {
            mostrarFormularioCreacion = true;
            nuevoUsuario = new UsuariosInsert();
            usuarioLogin = new UsuariosLogin();

            StateHasChanged();
        }

        private void MostrarFormularioLogin()
        {
            mostrarFormularioCreacion = false;
            nuevoUsuario = new UsuariosInsert();
            usuarioLogin = new UsuariosLogin();

            StateHasChanged();
        }

        private async Task LoginUsuario()
        {
            if(string.IsNullOrEmpty(usuarioLogin.USRS_CORREO) || string.IsNullOrEmpty(usuarioLogin.USRS_CLAVE))
            {
                mensaje = "Por favor, completa todos los campos.";
                return;
            }

            var result = await Http.PostAsJsonAsync("https://localhost:44308/api/Base/LoginUsuario", usuarioLogin);
            if (result.IsSuccessStatusCode)
            {
                var responseString = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<bool>(responseString);

                if (response == true)
                {
                    mensaje = "Inicio de sesión exitoso.";
                }
                else
                {
                    mensaje = "Credenciales incorrectas.";
                }
            }
            else
            {
                mensaje = "Error al iniciar sesión.";
            }
        }

        private async Task CrearUsuario()
        {
            if (string.IsNullOrEmpty(nuevoUsuario.USRS_NOMBRE) && string.IsNullOrEmpty(nuevoUsuario.USRS_CORREO) && string.IsNullOrEmpty(nuevoUsuario.USRS_CLAVE))
            {
                mensaje = "Estimado usuario, por favor completar todos los campos.";
                return;
            }

            var result = await Http.PostAsJsonAsync("https://localhost:44308/api/Base/InsertarUsuario", nuevoUsuario);
            if (result.IsSuccessStatusCode)
            {
                mensaje = "Usuario creado exitosamente.";
                MostrarFormularioLogin();
            }
            else
            {
                mensaje = "Error al crear el usuario.";
            }
        }


    }
}
