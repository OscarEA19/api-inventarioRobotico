using api_inventarioRobotico.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace api_inventarioRobotico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public UsuarioController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        [Route("ObtenerUsuarios")]
        public String GetUsuarios()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT cedula, carnet, nombre, apellido, idRol, contrasena, correoElectronico FROM usuario", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Usuario> usuarioList = new List<Usuario>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Usuario usuario = new Usuario();
                    usuario.cedula = Convert.ToString(dt.Rows[i]["cedula"]).Trim();
                    usuario.carnet = Convert.ToString(dt.Rows[i]["carnet"]).Trim();
                    usuario.nombre = Convert.ToString(dt.Rows[i]["nombre"]).Trim();
                    usuario.apellido = Convert.ToString(dt.Rows[i]["apellido"]).Trim();
                    usuario.idRol = Convert.ToInt32(dt.Rows[i]["idRol"]);
                    usuario.contrasena = Convert.ToString(dt.Rows[i]["contrasena"]).Trim();
                    usuario.correoElectronico = Convert.ToString(dt.Rows[i]["correoElectronico"]).Trim();
                    usuarioList.Add(usuario);
                }
            }
            if (usuarioList.Count > 0)
            {
                return JsonConvert.SerializeObject(usuarioList);
            }
            else
            {
                response.statusCode = 100;
                response.errorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("RegistarUsuario")]
        public String PostUsuario(Usuario usuario)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());

            //User Information

            String cedula = usuario.cedula;
            String carnet = usuario.carnet;
            String nombre = usuario.nombre;
            String apellido = usuario.apellido;
            int idRol = usuario.idRol;
            String conrasena = usuario.contrasena;
            String correoElectronico = usuario.correoElectronico;

            String query = "Insert into usuario (cedula, carnet, nombre, apellido, idRol, contrasena, correoElectronico) values ('" + cedula + "','" + carnet + "','" + nombre + "','" + apellido + "'," + idRol + ", '" + conrasena + "','" + correoElectronico + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();    
            if (i > 0)
            {
                return "se registro correctamente el usuario";
            }
            else
            {
                return "error al registrar el usuario";
            }

        }

        [HttpGet]
        [Route("ObtenerUsuarioPorCedula/{cedula}")]
        public String GetUsuarioPorCedula(String cedula)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT cedula, carnet, nombre, apellido, idRol, contrasena, correoElectronico FROM usuario where cedula="+cedula, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Usuario> usuarioList = new List<Usuario>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Usuario usuario = new Usuario();
                    usuario.cedula = Convert.ToString(dt.Rows[i]["cedula"]).Trim();
                    usuario.carnet = Convert.ToString(dt.Rows[i]["carnet"]).Trim();
                    usuario.nombre = Convert.ToString(dt.Rows[i]["nombre"]).Trim();
                    usuario.apellido = Convert.ToString(dt.Rows[i]["apellido"]).Trim();
                    usuario.idRol = Convert.ToInt32(dt.Rows[i]["idRol"]);
                    usuario.contrasena = Convert.ToString(dt.Rows[i]["contrasena"]).Trim();
                    usuario.correoElectronico = Convert.ToString(dt.Rows[i]["correoElectronico"]).Trim();
                    usuarioList.Add(usuario);
                }
            }
            if (usuarioList.Count > 0)
            {
                return JsonConvert.SerializeObject(usuarioList[0]);
            }
            else
            {
                response.statusCode = 100;
                response.errorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpDelete]
        [Route("BorrarUsuarioPorCedula/{cedula}")]
        public String DeleteUsuario(String cedula)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());

           
            String query = "Delete from usuario where cedula = '"+ cedula+"'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Se borro el usuario correctamente";
            }
            else
            {
                return "error al intentar borrar el usuario";
            }
        }

        [HttpPut]
        [Route("ActualizarUsuario")]
        public String PutUsuario(Usuario usuario)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());

            //User Information

            String cedula = usuario.cedula;
            String carnet = usuario.carnet;
            String nombre = usuario.nombre;
            String apellido = usuario.apellido;
            int idRol = usuario.idRol;
            String contrasena = usuario.contrasena;
            String correoElectronico = usuario.correoElectronico;

            String query = "Update usuario set carnet = '" + carnet + "', nombre='" + nombre + "', apellido = '" + apellido + "', idRol=" + idRol + " ,contrasena ='" + contrasena + "', correoElectronico = '" + correoElectronico + "' where cedula ='"+cedula+"'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "se actualizo correctamente el usuario";
            }
            else
            {
                return "error al actualizar el usuario";
            }

        }

    }
}
