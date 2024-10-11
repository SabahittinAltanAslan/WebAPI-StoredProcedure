using CategoryService.API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CategoryService.API.Data.Repositories
{
    public class CategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("spGetCategories", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                CategoryId = (int)reader["CategoryId"],
                                Name = reader["Name"].ToString()
                            });
                        }
                    }
                }
            }
            return categories;
        }

        public void AddCategory(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("spAddCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", name);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCategory(int categoryId, string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("spUpdateCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    command.Parameters.AddWithValue("@Name", name);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("spDeleteCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }

}
