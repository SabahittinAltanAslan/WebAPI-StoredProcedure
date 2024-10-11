using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrderService.API.Models;

namespace OrderService.Repositories
{
    public class OrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Order> GetOrders()
        {
            var orders = new List<Order>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("spGetOrders", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var orderId = (int)reader["OrderId"];
                            var order = orders.Find(o => o.OrderId == orderId);
                            if (order == null)
                            {
                                order = new Order
                                {
                                    OrderId = orderId,
                                    OrderDate = (DateTime)reader["OrderDate"],
                                    OrderProducts = new List<OrderProduct>()
                                };
                                orders.Add(order);
                            }

                            order.OrderProducts.Add(new OrderProduct
                            {
                                OrderId = orderId,
                                ProductId = (int)reader["ProductId"],
                                Quantity = (int)reader["Quantity"]
                            });
                        }
                    }
                }
            }
            return orders;
        }

        public int AddOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SqlCommand("spAddOrder", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                            order.OrderId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        foreach (var product in order.OrderProducts)
                        {
                            using (var command = new SqlCommand("spAddOrderProduct", connection, transaction))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@OrderId", order.OrderId);
                                command.Parameters.AddWithValue("@ProductId", product.ProductId);
                                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return order.OrderId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SqlCommand("spUpdateOrder", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@OrderId", order.OrderId);
                            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                            command.ExecuteNonQuery();
                        }

                        using (var command = new SqlCommand("spDeleteOrderProductsByOrderId", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@OrderId", order.OrderId);
                            command.ExecuteNonQuery();
                        }

                        foreach (var product in order.OrderProducts)
                        {
                            using (var command = new SqlCommand("spAddOrderProduct", connection, transaction))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@OrderId", order.OrderId);
                                command.Parameters.AddWithValue("@ProductId", product.ProductId);
                                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("spDeleteOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
