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
    public class InventarioController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public InventarioController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        [Route("ObtenerInventario")]
        public String GetInventario()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT id, idPieza, idKit, cantidadPieza FROM inventario", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Inventario> inventarioList = new List<Inventario>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Inventario inventario = new Inventario();
                    inventario.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    inventario.idPieza = Convert.ToInt32(dt.Rows[i]["idPieza"]);
                    inventario.idKit = Convert.ToString(dt.Rows[i]["idKit"]).Trim();
                    inventario.cantidadPieza = Convert.ToInt32(dt.Rows[i]["cantidadPieza"]);
                    inventarioList.Add(inventario);
                }
            }
            if (inventarioList.Count > 0)
            {
                return JsonConvert.SerializeObject(inventarioList);
            }
            else
            {
                response.statusCode = 100;
                response.errorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpGet]
        [Route("ObtenerInventarioPorKit/{idKit}")]
        public String GetUsuarioPorCedula(String idKit)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT id, idPieza, idKit, cantidadPieza FROM inventario where idKit='" + idKit+"'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Inventario> inventarioList = new List<Inventario>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Inventario inventario = new Inventario();
                    inventario.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    inventario.idPieza = Convert.ToInt32(dt.Rows[i]["idPieza"]); 
                    inventario.idKit = Convert.ToString(dt.Rows[i]["idKit"]).Trim();
                    inventario.cantidadPieza = Convert.ToInt32(dt.Rows[i]["cantidadPieza"]);
                    inventarioList.Add(inventario);
                }
            }
            if (inventarioList.Count > 0)
            {
                return JsonConvert.SerializeObject(inventarioList[0]);
            }
            else
            {
                response.statusCode = 100;
                response.errorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpDelete]
        [Route("BorrarInventarioPorKit/{idKit}")]
        public String DeleteKit(String idKit)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            String query = "Delete from inventario where idKit = '" + idKit + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Se borro el inventario correctamente";
            }
            else
            {
                return "error al intentar borrar el inventario";
            }
        }

        [HttpPost]
        [Route("RegistarInventario")]
        public String PostInventario(Inventario inventario)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            int idPieza = inventario.idPieza;
            String idKit = inventario.idKit;
            int cantidadPieza = inventario.cantidadPieza;


            String query = "Insert into inventario (idPieza, idKit, cantidadPieza) values (" + idPieza + ",'" + idKit + "',"+cantidadPieza+")";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "se registro correctamente el inventario";
            }
            else
            {
                return "error al registrar el inventario";
            }
        }

        [HttpPut]
        [Route("ActualizarInventario")]
        public String PutInventario(Inventario inventario)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());

            int idPieza = inventario.idPieza;
            String idKit = inventario.idKit;
            int cantidadPieza = inventario.cantidadPieza;

            String query = "Update inventario set cantidadPieza = " + cantidadPieza + " where idPieza = " + idPieza + " and idKit = '"+idKit+"'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "se actualizo correctamente el inventario";
            }
            else
            {
                return "error al actualizar el inventario";
            }

        }
    }
}
