namespace Student_Registration_Theme.Areas.State.Models
{
    public class LOC_StateModel
    {
        public int StateID { get; set; }
        public string? StateName { get; set; }

        public int? CountryID { get; set; }
        public string? StateCode { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public class LOC_State_DropdownModel
    {
        public int StateID { get; set; }
        public string? StateName { get; set; }
    }
}
