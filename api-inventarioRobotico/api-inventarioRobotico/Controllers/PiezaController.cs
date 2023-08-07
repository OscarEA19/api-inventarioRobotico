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
    public class PiezaController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public PiezaController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        [Route("ObtenerPiezas")]
        public String GetPiezas()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT id, descripcion FROM pieza", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Pieza> piezaList = new List<Pieza>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Pieza pieza = new Pieza();
                    pieza.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    pieza.descripcion = Convert.ToString(dt.Rows[i]["descripcion"]).Trim();
                    piezaList.Add(pieza);
                }
            }
            if (piezaList.Count > 0)
            {
                return JsonConvert.SerializeObject(piezaList);
            }
            else
            {
                response.statusCode = 100;
                response.errorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]  
        [Route("RegistarPieza")]
        public String PostPieza(Pieza pieza)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            String descripcion = pieza.descripcion;

            String query = "Insert into pieza (descripcion) values ('" + descripcion + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "se registro correctamente la pieza";
            }
            else
            {
                return "error al registrar la pieza";
            }
        }

        [HttpDelete]
        [Route("BorrarPiezaPorId/{id}")]
        public String DeletePieza(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            String query = "Delete from pieza where id = '" + id + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Se borro la pieza correctamente";
            }
            else
            {
                return "error al intentar borrar la pieza";
            }
        }


    }
}
