namespace Test
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int TopSeller { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
