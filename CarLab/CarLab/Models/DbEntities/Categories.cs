namespace CarLab.Models.DbEntities
{
    public class Categories
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
        public int? ParentCategoryID { get; set; }
        public bool IsActive { get; set; }
    }
}
