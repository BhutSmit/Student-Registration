using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Student_Registration_Theme.Areas.Student.Models;
using Student_Registration_Theme.Areas.Branch.Models;
using Student_Registration_Theme.Areas.City.Models;

namespace Student_Registration_Theme.Areas.Student.Controllers
{
    [Area("Student")]
    public class MST_StudentController : Controller
    {
        public IConfiguration Configuration;

        public MST_StudentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult MST_StudentList()
        {
            string connectionstring = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_Student_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return View("MST_StudentList", dt);
        }

        public IActionResult MST_StudentSave(MST_StudentModel student)
        {
            string connectionstring = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;

            if (student.StudentID == null)
            {
                command.CommandText = "PR_MST_Student_Insert";
            }

            else
            {
                command.CommandText = "PR_MST_Student_UpdateByPK";
                command.Parameters.AddWithValue("@StudentID", student.StudentID);
            }

            command.Parameters.AddWithValue("@StudentName", student.StudentName);
            command.Parameters.AddWithValue("@BranchID", student.BranchID);
            command.Parameters.AddWithValue("@CityID", student.CityID);
            command.Parameters.AddWithValue("@MobileNoStudent", student.MobileNoStudent);
            command.Parameters.AddWithValue("@MobileNoFather", student.MobileNoFather);
            command.Parameters.AddWithValue("@Email", student.Email);
            command.Parameters.AddWithValue("@Address", student.Address);
            command.Parameters.AddWithValue("@BirthDate", student.BirthDate);
            command.Parameters.AddWithValue("@Age", student.Age);
            command.Parameters.AddWithValue("@IsActive", student.IsActive);
            command.Parameters.AddWithValue("@Gender", student.Gender);
            command.Parameters.AddWithValue("@Password", student.Password);

            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("MST_StudentList");
        }

        public IActionResult MST_StudentAddEdit(int StudentID = 0)
        {
            string connectionstring = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_Student_SelectbyPK";
            command.Parameters.AddWithValue("@StudentID", StudentID);

            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            MST_StudentModel student = new MST_StudentModel();

            foreach (DataRow row in dt.Rows)
            {
                student.StudentID = Convert.ToInt32(row["StudentID"]);
                student.StudentName = row["StudentName"].ToString();
                student.BranchID = Convert.ToInt32(row["BranchID"]);
                student.CityID = Convert.ToInt32(row["CityID"]);
                student.MobileNoStudent = row["MobileNoStudent"].ToString();
                student.MobileNoFather = row["MobileNoFather"].ToString();
                student.Email = row["Email"].ToString();
                student.Address = row["Address"].ToString();
                student.BirthDate = Convert.ToDateTime(row["BirthDate"]);
                student.IsActive = Convert.ToBoolean(row["IsActive"]);
                student.Age = Convert.ToInt32(row["Age"]);
                student.Gender = row["Gender"].ToString();
                student.Password = row["Password"].ToString();

            }

            SqlConnection connection2 = new SqlConnection(connectionstring);
            connection2.Open();
            SqlCommand command2 = connection2.CreateCommand();
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "PR_MST_BranchComboBox";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(reader2);
            connection2.Close();

            List<MST_BranchDropdownModel> list2 = new List<MST_BranchDropdownModel>();

            foreach (DataRow dr in dt2.Rows)
            {
                MST_BranchDropdownModel model = new MST_BranchDropdownModel();
                model.BranchID = Convert.ToInt32(dr["BranchID"]);
                model.BranchName = dr["BranchName"].ToString();
                list2.Add(model);
            }

            ViewBag.BranchList = list2;

            SqlConnection connection3 = new SqlConnection(connectionstring);
            connection3.Open();
            SqlCommand command3 = connection3.CreateCommand();
            command3.CommandType = CommandType.StoredProcedure;
            command3.CommandText = "PR_LOC_CityComboBox";
            SqlDataReader reader3 = command3.ExecuteReader();
            DataTable dt3 = new DataTable();
            dt3.Load(reader3);
            connection3.Close();

            List<LOC_CityDropdownModel> list3 = new List<LOC_CityDropdownModel>();

            foreach (DataRow dr in dt3.Rows)
            {
                LOC_CityDropdownModel model = new LOC_CityDropdownModel();
                model.CityID = Convert.ToInt32(dr["CityID"]);
                model.CityName = dr["CityName"].ToString();
                list3.Add(model);
            }

            ViewBag.CityList = list3;

            return View(student);
        }

        public IActionResult StudentDelete(int StudentID)
        {
            string connectionstring = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_Student_DeleteByPK";
            command.Parameters.AddWithValue("@StudentID", StudentID);
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("StudentList");
        }

        public IActionResult MST_StudentFilter(string? StudentName, string? BranchName, string? CityName, string? StateName, string? CountryName)
        {

            string connection_string = this.Configuration.GetConnectionString("MyConnectionString");
            SqlConnection conn = new SqlConnection(connection_string);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Student_Filter";

            if (StudentName == null)
            {
                StudentName = DBNull.Value.ToString();
            }

            if (BranchName == null)
            {
                BranchName = DBNull.Value.ToString();
            }

            if (CityName == null)
            {
                CityName = DBNull.Value.ToString();
            }

            if (StateName == null)
            {
                StateName = DBNull.Value.ToString();
            }

            if (CountryName == null)
            {
                CountryName = DBNull.Value.ToString();
            }

            cmd.Parameters.AddWithValue("@StudentName", StudentName);
            cmd.Parameters.AddWithValue("@BranchName", BranchName);
            cmd.Parameters.AddWithValue("@CityName", CityName);
            cmd.Parameters.AddWithValue("@StateName", StateName);
            cmd.Parameters.AddWithValue("@CountryName", CountryName);

            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            return View("MST_StudentList", dt);
        }
    }
}
