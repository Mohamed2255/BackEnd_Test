using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private List<User> _users = new List<User>
        {
            new User { Id = 1, Name = "User 1" },
            new User { Id = 2, Name = "User 2" },
            new User { Id = 3, Name = "User 3" },
        };

        private List<Category> _categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Clothing" },
            new Category { Id = 3, Name = "Foods" }
        };

        private List<UserCategory> _userCategories = new List<UserCategory>
        {
            new UserCategory { Id = 1, UserId = 1, CategoryId = 1 },
            new UserCategory { Id = 2, UserId = 1, CategoryId = 2 },
            new UserCategory { Id = 3, UserId = 2, CategoryId = 2 }
        };

        private List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Latitude = 40.7128, Longitude = -74.0060, TopSeller = 100, CreatedAt = DateTime.Now.AddDays(-2), CategoryID = 1 },
            new Product { Id = 2, Name = "Product 2", Latitude = 34.0522, Longitude = -118.2437, TopSeller = 115, CreatedAt = DateTime.Now.AddDays(-3), CategoryID = 1 },
            new Product { Id = 3, Name = "Product 3", Latitude = 45.7128, Longitude = -78.0060, TopSeller = 120, CreatedAt = DateTime.Now.AddDays(-4), CategoryID = 1 },
            new Product { Id = 4, Name = "Product 4", Latitude = 55.7128, Longitude = -85.0060, TopSeller = 150, CreatedAt = DateTime.Now.AddDays(-5), CategoryID = 2 },
            new Product { Id = 5, Name = "Product 5", Latitude = 55.7128, Longitude = -85.0060, TopSeller = 1200, CreatedAt = DateTime.Now.AddDays(-7), CategoryID = 2 },
            new Product { Id = 6, Name = "Product 6", Latitude = 60.0522, Longitude = -125.2437, TopSeller = 180, CreatedAt = DateTime.Now.AddDays(-6), CategoryID = 2 },
            new Product { Id = 7, Name = "Product 7", Latitude = 65.7128, Longitude = -7000.0060, TopSeller = 120, CreatedAt = DateTime.Now.AddDays(-8), CategoryID = 3 },
            new Product { Id = 8, Name = "Product 8", Latitude = 70.0522, Longitude = -1300.2437, TopSeller = 110, CreatedAt = DateTime.Now.AddDays(-9), CategoryID = 3 }

        };

        [HttpGet]
        public IActionResult GetProducts(int userID, double latitude, double longitude)
        {
            var favouriteCategories = _userCategories.Where(uc => uc.UserId == userID).Select(o => o.CategoryId).ToList();

            List<Product> products = new List<Product>();
            
            if (favouriteCategories.Count > 0)
            {
                products = _products
                    .Where(p => favouriteCategories.Contains(p.CategoryID))
                    .OrderBy(p => CalculateDistance(p.Latitude, p.Longitude, latitude, longitude))
                    .ThenByDescending(p => p.TopSeller)
                    .ThenByDescending(p => p.CreatedAt)
                    .ToList();
            }
            else
            {
                products = _products
                    .OrderBy(p => CalculateDistance(p.Latitude, p.Longitude, latitude, longitude))
                    .ThenByDescending(p => p.TopSeller)
                    .ThenByDescending(p => p.CreatedAt)
                    .ToList();
            }

            return Ok(products);
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadiusKm = 6371; // Earth's radius in kilometers
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadiusKm * c;

            return distance;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
