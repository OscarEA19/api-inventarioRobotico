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
    public class KitController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public KitController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        [Route("ObtenerKits")]
        public String GetKits()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT id, isPrestado FROM Kit", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Kit> KitList = new List<Kit>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Kit kit = new Kit();
                    kit.id = Convert.ToString(dt.Rows[i]["id"]).Trim();
                    kit.isPrestado = Convert.ToInt32(dt.Rows[i]["isPrestado"]);              
                    KitList.Add(kit);
                }
            }
            if (KitList.Count > 0)
            {
                return JsonConvert.SerializeObject(KitList);
            }
            else
            {
                response.statusCode = 100;
                response.errorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("RegistarKit")]
        public String PostKit(Kit kit)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            String id = kit.id;
            int isPrestado = kit.isPrestado;

            String query = "Insert into kit (id, isPrestado) values ('" + id + "',"+isPrestado+")";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "se registro correctamente el kit";
            }
            else
            {
                return "error al registrar el kit";
            }
        }

        [HttpDelete]
        [Route("BorrarKitPorId/{id}")]
        public String DeleteKit(String id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            String query = "Delete from kit where id = '" + id + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Se borro el kit correctamente";
            }
            else
            {
                return "error al intentar borrar el kit";
            }
        }
    }
}
