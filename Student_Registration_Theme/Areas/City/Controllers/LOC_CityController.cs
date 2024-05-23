using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Student_Registration_Theme.Areas.City.Models;
using Student_Registration_Theme.Areas.Country.Models;
using Student_Registration_Theme.Areas.State.Models;

namespace Student_Registration_Theme.Areas.City.Controllers
{
    [Area("City")]
    public class LOC_CityController : Controller
    {
        public IConfiguration Configuration;

        public LOC_CityController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult LOC_CityList()
        {
            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_City_SelectAll";

            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return View("LOC_CityList", dt);
        }

        public IActionResult LOC_CitySave(LOC_CityModel cm)
        {
            string connecction_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connecction_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (cm.CityID == 0)
            {
                cmd.CommandText = "PR_LOC_City_Insert";
            }
            else
            {
                cmd.CommandText = "PR_LOC_City_UpdateByPK";
                cmd.Parameters.AddWithValue("@CityID", cm.CityID);
            }
            cmd.Parameters.AddWithValue("@CityName", cm.CityName);
            cmd.Parameters.AddWithValue("@CityCode", cm.CityCode);
            cmd.Parameters.AddWithValue("@StateID", cm.StateID);
            cmd.Parameters.AddWithValue("@CountryID", cm.CountryID);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("LOC_CityList");
        }

        public IActionResult LOC_CityAddEdit(int? CityID = 0)
        {
            string connecction_string = this.Configuration.GetConnectionString("MyConnectionString");

            //SqlConnection conn1 = new SqlConnection(connecction_string); 
            //conn1.Open();

            //SqlCommand cmd1 = conn1.CreateCommand();
            //cmd1.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd1.CommandText = "PR_LOC_State_ComboBox";

            //SqlDataReader reader1 = cmd1.ExecuteReader();
            //DataTable dt1 = new DataTable();
            //dt1.Load(reader1);
            //conn1.Close();

            //List<LOC_State_DropdownModel> list1 = new List<LOC_State_DropdownModel>();

            //foreach(DataRow dr1 in dt1.Rows){
            //    LOC_State_DropdownModel model1 = new LOC_State_DropdownModel();
            //    model1.StateID = Convert.ToInt32(dr1["StateID"]);
            //    model1.StateName = dr1["StateName"].ToString();
            //    list1.Add(model1);
            //}
            //ViewBag.StateList = list1;

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
            cmd.CommandText = "PR_LOC_City_SelectByPK";
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            LOC_CityModel cm = new LOC_CityModel();

            foreach (DataRow row in dt.Rows)
            {
                cm.CityID = Convert.ToInt32(row["CityID"]);
                cm.CityName = row["CityName"].ToString();
                cm.CityCode = row["CityCode"].ToString();
                cm.StateID = Convert.ToInt32(row["StateID"]);
                cm.CountryID = Convert.ToInt32(row["CountryID"]);
            }

            return View("LOC_CityAddEdit", cm);
        }

        public IActionResult DeleteCity(int CityID)
        {
            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_City_DeleteByPK";
            cmd.Parameters.AddWithValue("@CityID", CityID);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("LOC_CityList");
        }

        public dynamic StatesForComboBox(int CountryID)
        {
            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_State_StateByCountryID";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);

            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            conn.Close();

            List<LOC_State_DropdownModel> list = new List<LOC_State_DropdownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                LOC_State_DropdownModel state = new LOC_State_DropdownModel();
                state.StateID = Convert.ToInt32(dr["StateID"]);
                state.StateName = dr["StateName"].ToString();
                list.Add(state);
            }

            ViewBag.StateList = list;

            return Json(list);
        }

        public IActionResult LOC_CityFilter(string? CityName, string? CityCode, string? StateName, string? CountryName)
        {

            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_Filter";

            if (CityName == null)
            {
                CityName = DBNull.Value.ToString();
            }

            if (CityCode == null)
            {
                CityCode = DBNull.Value.ToString();
            }

            if (StateName == null)
            {
                StateName = DBNull.Value.ToString();
            }

            if (CountryName == null)
            {
                CountryName = DBNull.Value.ToString();
            }

            cmd.Parameters.AddWithValue("@CityName", CityName);
            cmd.Parameters.AddWithValue("@CityCode", CityCode);
            cmd.Parameters.AddWithValue("@StateName", StateName);
            cmd.Parameters.AddWithValue("@CountryName", CountryName);

            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            return View("LOC_CityList", dt);
        }
    }
}
