using api_inventarioRobotico.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace api_inventarioRobotico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraPrestadoController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public BitacoraPrestadoController(IConfiguration configuration)
        {
            _configuration = configuration; 

        }

        [HttpGet]
        [Route("ObtenerBitacora")]
        public String GetBitacoraPrestado()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT id, cedulaUsuario, idKit, fecha FROM bitacoraPrestado", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<BitacoraPrestado> bitacoraList = new List<BitacoraPrestado>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    BitacoraPrestado bitacora = new BitacoraPrestado();
                    bitacora.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    bitacora.cedulaUsuario = Convert.ToString(dt.Rows[i]["cedulaUsuario"]).Trim();
                    bitacora.idKit = Convert.ToString(dt.Rows[i]["idKit"]).Trim();
                    bitacora.fecha = Convert.ToDateTime(dt.Rows[i]["fecha"]);
                    bitacoraList.Add(bitacora);
                }
            }
            if(bitacoraList.Count > 0)
            {
                return JsonConvert.SerializeObject(bitacoraList);        
            }
            else
            {
                response.statusCode = 100;
                response.errorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }
        [HttpGet]
        [Route("ObtenerBitacoraPorCedula/{cedula}")]
        public String GetUsuarioPorCedula(String cedula)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT id, cedulaUsuario, idKit, fecha FROM bitacoraPrestado where cedulaUsuario='" + cedula + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<BitacoraPrestado> bitacoraList = new List<BitacoraPrestado>();
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BitacoraPrestado bitacora = new BitacoraPrestado();
                    bitacora.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    bitacora.cedulaUsuario = Convert.ToString(dt.Rows[i]["cedulaUsuario"]).Trim();
                    bitacora.idKit = Convert.ToString(dt.Rows[i]["idKit"]).Trim();
                    bitacora.fecha = Convert.ToDateTime(dt.Rows[i]["fecha"]);
                    bitacoraList.Add(bitacora);
                }
            }
            if (bitacoraList.Count > 0)
            {
                return JsonConvert.SerializeObject(bitacoraList);
            }
            else
            {
                response.statusCode = 100;
                response.errorMessage = "No data found";
                return JsonConvert.SerializeObject(response);
            }
        }

        [HttpPost]
        [Route("RegistarBitacora")]
        public String PostBitacora(BitacoraPrestado bitacoraPrestado)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("InventarioRoboticoApp").ToString());
            String cedulaUsuario = bitacoraPrestado.cedulaUsuario;
            String idKit = bitacoraPrestado.idKit;
            DateTime fecha = bitacoraPrestado.fecha;

            String query = "Insert into bitacoraPrestado (cedulaUsuario, idKit, fecha) values ('" + cedulaUsuario + "','" + idKit + "','"+fecha+"')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "se registro correctamente la bitacora";
            }
            else
            {
                return "error al registrar la bitacora";
            }
        }
    }
}
