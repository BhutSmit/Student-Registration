namespace Student_Registration_Theme.Areas.Branch.Models
{
    public class MST_BranchModel
    {
        public int BranchID { get; set; }
        public string? BranchName { get; set; }
        public string? BranchCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public class MST_BranchDropdownModel
    {
        public int? BranchID { get; set; }

        public string? BranchName { get; set; }
    }
}
