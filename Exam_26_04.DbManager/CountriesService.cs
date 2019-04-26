using Exam_26_04.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_26_04.DbManager
{
    public class CountriesService : Initializer
    {
        public List<Country> SelectCountries()
        {
            var data = new List<Country>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        command.CommandText = "Select * from Countries";

                        var sqlDataReader = command.ExecuteReader();

                        while (sqlDataReader.Read())
                        {
                            Guid id = Guid.Parse(sqlDataReader["Id"].ToString());
                            string name = sqlDataReader["Name"].ToString();

                            data.Add(new Country
                            {
                                Id = id,
                                Name = name
                            });
                        }
                    }
                    catch (SqlException exception)
                    {

                    }
                    catch (Exception exception)
                    {

                    }
                }
            }
            return data;
        } 

        public void InsertCountry(Country country)
        {
            using (connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        transaction = connection.BeginTransaction();
                        command.CommandText = "insert into Countries values(@Id,@Name)";

                        var idParametr = command.CreateParameter();
                        idParametr.ParameterName = "@Id";
                        idParametr.DbType = System.Data.DbType.Guid;
                        idParametr.Value = country.Id;

                        var nameParametr = command.CreateParameter();
                        nameParametr.ParameterName = "@Name";
                        nameParametr.DbType = System.Data.DbType.String;
                        nameParametr.Value = country.Name;

                        command.Parameters.Add(idParametr);
                        command.Parameters.Add(nameParametr);

                        command.Transaction = transaction;
                        int affectedRows = command.ExecuteNonQuery();

                        if (affectedRows < 1)
                        {
                            throw new Exception("Вставка не удалась");
                        }
                        transaction.Commit();

                    }
                    catch (SqlException exception)
                    {
                        transaction?.Rollback();
                    }
                    catch (Exception exception)
                    {
                        transaction?.Rollback();
                    }
                }
            }
        }

        public void UpdateCountry(Country country, string newName)
        {
            using (connection = new SqlConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        transaction = connection.BeginTransaction();
                        command.CommandText = $"update Countries set Name=@newName where Id=@idCountry";

                        var newNameParametr = command.CreateParameter();
                        newNameParametr.ParameterName = "@newName";
                        newNameParametr.DbType = System.Data.DbType.String;
                        newNameParametr.Value = newName;

                        var idParametr = command.CreateParameter();
                        idParametr.ParameterName = "@idCountry";
                        idParametr.DbType = System.Data.DbType.Guid;
                        idParametr.Value = country.Id;

                        command.Parameters.Add(newNameParametr);
                        command.Parameters.Add(idParametr);

                        command.Transaction = transaction;
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows < 1)
                        {
                            throw new Exception("Обновление не удалось!");
                        }
                        transaction.Commit();
                    }
                    catch (SqlException exception)
                    {
                        transaction?.Rollback();
                    }
                    catch (Exception exception)
                    {
                        transaction?.Rollback();
                    }
                }
            }
        }

        public void DeleteCountry(Country country)
        {
            using (connection = new SqlConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        transaction = connection.BeginTransaction();
                        command.CommandText = $"delete from countries where Id=@idCountry";

                        var idParametr = command.CreateParameter();
                        idParametr.ParameterName = "@idCountry";
                        idParametr.DbType = System.Data.DbType.Guid;
                        idParametr.Value = country.Id;

                        command.Parameters.Add(idParametr);

                        command.Transaction = transaction;
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows < 1)
                        {
                            throw new Exception("Удаление не удалось!");
                        }
                        transaction.Commit();
                    }
                    catch (SqlException exception)
                    {
                        transaction?.Rollback();
                    }
                    catch (Exception exception)
                    {
                        transaction?.Rollback();
                    }
                }
            }
        }
    }
}
