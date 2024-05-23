using Microsoft.AspNetCore.Mvc;
using Student_Registration_Theme.Areas.Country.Models;
using System.Data;
using System.Data.SqlClient;

namespace Student_Registration_Theme.Areas.Country.Controllers
{
    [Area("Country")]
    public class LOC_CountryController : Controller
    {
        private IConfiguration Configuration;

        public LOC_CountryController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult LOC_CountryList()
        {

            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_SelectAll";

            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            return View(dt);
        }

        public IActionResult Add()
        {
            return View("LOC_CountryAddEdit");
        }

        public IActionResult LOC_CountryAddEdit(int? CountryID = 0)
        {
            Console.WriteLine("CountryID : ", CountryID);
            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_SelectByPK";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);

            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            LOC_CountryModel CM = new LOC_CountryModel();

            foreach(DataRow dr in dt.Rows)
            {
                CM.CountryID = Convert.ToInt32(@dr["CountryID"]);
                CM.CountryName = @dr["CountryName"].ToString();
                CM.CountryCode = @dr["CountryCode"].ToString();
            }

            return View("LOC_CountryAddEdit" , CM);
        }

        public IActionResult LOC_CountrySave(LOC_CountryModel CM)
        {

                string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
                SqlConnection conn = new SqlConnection(connection_string);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                /* DataTable dt = new DataTable();*/

                if (CM.CountryID == 0)
                {
                    cmd.CommandText = "PR_LOC_Country_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_LOC_Country_UpdateByPK";
                    cmd.Parameters.AddWithValue("@CountryID", CM.CountryID);
                }

                cmd.Parameters.AddWithValue("@CountryName", CM.CountryName);
                cmd.Parameters.AddWithValue("@countryCode", CM.CountryCode);

                cmd.ExecuteNonQuery();
                conn.Close();

                return RedirectToAction("LOC_CountryList");

            //return View("LOC_CountryAddEdit",CM);
        }

        public IActionResult DeleteCountry(int CountryID)
        {

            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_DeleteByPK";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.ExecuteNonQuery();

            return RedirectToAction("");
        }

        public IActionResult LOC_CountryFilter(string? CountryName, string? CountryCode)
        {

            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_Country_Filter";

            if(CountryName == null)
            {
                CountryName = DBNull.Value.ToString();
            }

            if (CountryCode == null)
            {
                CountryCode = DBNull.Value.ToString();
            }

            cmd.Parameters.AddWithValue("@CountryName", CountryName);
            cmd.Parameters.AddWithValue("@CountryCode", CountryCode);

            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            return View("LOC_CountryList" , dt);
        }

    }
}
