namespace Student_Registration_Theme.Areas.City.Models
{
    public class LOC_CityModel
    {
        public int CityID { get; set; }
        public string? CityName { get; set; }

        public string? CityCode { get; set; }
        public int? StateID { get; set; }
        public int? CountryID { get; set; }

    }

    public class LOC_CityDropdownModel
    {
        public int? CityID { get; set; }

        public string? CityName { get; set; }
    }
}
