using Microsoft.AspNetCore.Mvc;
using Student_Registration_Theme.Areas.Country.Models;
using Student_Registration_Theme.Areas.State.Models;
using System.Data;
using System.Data.SqlClient;

namespace Student_Registration.Areas.State.Controllers
{
    [Area("State")]
    public class LOC_StateController : Controller
    {

        private IConfiguration Configuration;

        public LOC_StateController(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IActionResult LOC_StateList()
        {
            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_SelectAll";

            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return View("LOC_StateList", dt);
        }

        public IActionResult Add()
        {
            return View("LOC_StateAddEdit");
        }

        public IActionResult LOC_StateAddEdit(int? StateID = 0)
        {
            string connecction_string = this.Configuration.GetConnectionString("MyConnectionString");

            SqlConnection conn2 = new SqlConnection(connecction_string);
            conn2.Open();

            SqlCommand cmd2 = conn2.CreateCommand();
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.CommandText = "PR_LOC_Country_ComboBox";

            SqlDataReader reader2 = cmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(reader2);
            conn2.Close();

            List<LOC_Country_DropdownModel> list2 = new List<LOC_Country_DropdownModel>();

            foreach (DataRow dr2 in dt2.Rows)
            {
                LOC_Country_DropdownModel model2 = new LOC_Country_DropdownModel();
                model2.CountryID = Convert.ToInt32(dr2["CountryID"]);
                model2.CountryName = dr2["CountryName"].ToString();
                list2.Add(model2);
            }
            ViewBag.CountryList = list2;

            SqlConnection conn = new SqlConnection(connecction_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_SelectByPK";
            cmd.Parameters.AddWithValue("@StateID", StateID);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            LOC_StateModel cm = new LOC_StateModel();

            foreach (DataRow row in dt.Rows)
            {
                cm.StateID = Convert.ToInt32(row["StateID"]);
                cm.StateName = row["StateName"].ToString();
                cm.StateCode = row["StateCode"].ToString();
                cm.CountryID = Convert.ToInt32(row["CountryID"]);
            }

            return View("LOC_StateAddEdit", cm);
        }

        public IActionResult LOC_StateSave(LOC_StateModel cm)
        {
            string connecction_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connecction_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (cm.StateID == 0)
            {
                cmd.CommandText = "PR_LOC_State_Insert";
            }
            else
            {
                cmd.CommandText = "PR_LOC_State_UpdateByPK";
                cmd.Parameters.AddWithValue("@StateID", cm.StateID);
            }
            cmd.Parameters.AddWithValue("@StateName", cm.StateName);
            cmd.Parameters.AddWithValue("@StateCode", cm.StateCode);
            cmd.Parameters.AddWithValue("@CountryID", cm.CountryID);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("LOC_StateList");
        }

        public IActionResult DeleteState(int StateID)
        {
            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_DeleteByPK";
            cmd.Parameters.AddWithValue("@StateID", StateID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("LOC_StateList");
        }

        public IActionResult LOC_StateFilter(string? StateName, string? StateCode, string? CountryName)
        {

            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_Filter";

            if (StateName == null)
            {
                StateName = DBNull.Value.ToString();
            }

            if (StateCode == null)
            {
                StateCode = DBNull.Value.ToString();
            }

            if (CountryName == null)
            {
                CountryName = DBNull.Value.ToString();
            }

            cmd.Parameters.AddWithValue("@StateName", StateName);
            cmd.Parameters.AddWithValue("@StateCode", StateCode);
            cmd.Parameters.AddWithValue("@CountryName", CountryName);

            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            return View("LOC_StateList", dt);
        }

    }
}
