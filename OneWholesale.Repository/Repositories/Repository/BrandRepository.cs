using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using OneWholesale.Model.Models;
using OneWholesale.Repository.DbFactory;


namespace OneWholesale.Repository.Repositories.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IDbOWConnection _dbOWConnection;

        [ActivatorUtilitiesConstructor]
        public BrandRepository(IDbOWConnection dbOWConnection)
        {
            _dbOWConnection = dbOWConnection;
        }

        public Brand GetBrandById(int id)
        {
            Brand brand = null;

            using (var connection = _dbOWConnection.Connection)
            {
                using (var command = new SqlCommand("ManageBrand", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ActionType", "SelectById");
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@BrandName", DBNull.Value);
                    command.Parameters.AddWithValue("@CompanyLogo", DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                    command.Parameters.AddWithValue("@UpdatedBy", DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedDateTime", DBNull.Value);
                    command.Parameters.AddWithValue("@UpdatedDateTime", DBNull.Value);

                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            brand = new Brand
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                BrandName = reader["BrandName"]?.ToString(),
                                CompanyLogo = reader["CompanyLogo"]?.ToString(),
                                CreatedBy = reader["CreatedBy"]?.ToString(),
                                UpdatedBy = reader["UpdatedBy"]?.ToString(),
                                CreatedDateTime = Convert.ToDateTime(reader["CreatedDateTime"]),
                                UpdatedDateTime = Convert.ToDateTime(reader["UpdatedDateTime"]),
                                DeletedBy = reader["DeletedBy"]?.ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                        }
                    }
                }
            }

            return brand;
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            var brands = new List<Brand>();

            using (var connection = _dbOWConnection.Connection)
            {
                using (var command = new SqlCommand("ManageBrand", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ActionType", "SelectAll");
                    command.Parameters.AddWithValue("@Id", 0);
                    command.Parameters.AddWithValue("@BrandName", DBNull.Value);
                    command.Parameters.AddWithValue("@CompanyLogo", DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                    command.Parameters.AddWithValue("@UpdatedBy", DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedDateTime", DBNull.Value);
                    command.Parameters.AddWithValue("@UpdatedDateTime", DBNull.Value);

                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var brand = new Brand
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                BrandName = reader["BrandName"]?.ToString(),
                                CompanyLogo = reader["CompanyLogo"]?.ToString(),
                                CreatedBy = reader["CreatedBy"]?.ToString(),
                                UpdatedBy = reader["UpdatedBy"]?.ToString(),
                                CreatedDateTime = Convert.ToDateTime(reader["CreatedDateTime"]),
                                UpdatedDateTime = Convert.ToDateTime(reader["UpdatedDateTime"]),
                                DeletedBy = reader["DeletedBy"]?.ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };

                            brands.Add(brand);
                        }
                    }
                }
            }

            return brands;
        }

        public bool ManageBrand(string actionType, Brand brand, string user)
        {
            using (var connection = _dbOWConnection.Connection)
            {
                using (var command = new SqlCommand("ManageBrand", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ActionType", actionType);
                    command.Parameters.AddWithValue("@Id", brand.Id);
                    command.Parameters.AddWithValue("@BrandName", brand.BrandName ?? "");
                    command.Parameters.AddWithValue("@CompanyLogo", brand.CompanyLogo ?? "");
                    command.Parameters.AddWithValue("@CreatedBy", user);
                    command.Parameters.AddWithValue("@UpdatedBy", user);
                    command.Parameters.AddWithValue("@CreatedDateTime", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@UpdatedDateTime", DateTime.UtcNow);

                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool DeleteBrand(int id, string updatedBy)
        {
            using (var connection = _dbOWConnection.Connection)
            {
                using (var command = new SqlCommand("ManageBrand", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ActionType", "Delete");
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                    command.Parameters.AddWithValue("@BrandName", DBNull.Value);
                    command.Parameters.AddWithValue("@CompanyLogo", DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedDateTime", DBNull.Value);
                    command.Parameters.AddWithValue("@UpdatedDateTime", DBNull.Value);

                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
