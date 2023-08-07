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
    public class RolController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public RolController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        [Route("ObtenerRoles")]
        public String GetRoles()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT id, descripcion FROM rol", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Rol> rolList = new List<Rol>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Rol rol = new Rol();
                    rol.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    rol.descripcion = Convert.ToString(dt.Rows[i]["descripcion"]).Trim();
                    rolList.Add(rol);
                }
            }
            if (rolList.Count > 0)
            {
                return JsonConvert.SerializeObject(rolList);
            }
            else
            {
                response.statusCode = 100;
                response.errorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("RegistarRole")]
        public String PostRole(Rol role)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());

            String descripcion = role.descripcion;
           
            String query = "Insert into rol (descripcion) values ('"+descripcion+"')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "se registro correctamente el rol";
            }
            else
            {
                return "error al registrar el rol";
            }
        }


        [HttpDelete]
        [Route("BorrarRolPorId/{id}")]
        public String DeleteRole(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            String query = "Delete from rol where id = '" + id + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Se borro el rol correctamente";
            }
            else
            {
                return "error al intentar borrar el rol";
            }
        }

    }
}
