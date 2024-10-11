using System.ComponentModel.DataAnnotations;

namespace ProductService.Models
{

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }


}
