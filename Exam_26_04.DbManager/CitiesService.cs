using Exam_26_04.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_26_04.DbManager
{
    public class CitiesService : Initializer
    {
        public List<City> SelectCities()
        {
            var data = new List<City>();

            using (var connection = new SqlConnection(connectionString))
            {
                using(var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        command.CommandText = "Select * from cities";

                        var sqlDataReader = command.ExecuteReader();

                        while (sqlDataReader.Read())
                        {
                            Guid id = Guid.Parse(sqlDataReader["Id"].ToString());
                            Guid countryId = Guid.Parse(sqlDataReader["CountryId"].ToString());
                            string name = sqlDataReader["Name"].ToString();

                            data.Add(new City
                            {
                                Id = id,
                                CountryId = countryId,
                                Name = name
                            });
                        }
                    }
                    catch(SqlException exception)
                    {

                    }
                    catch(Exception exception)
                    {

                    }
                }
            }
            return data;
        }
        public void InsertCity(City city)
        {
            using(connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        transaction = connection.BeginTransaction();
                        command.CommandText = "insert into Cities values(@Id,@CountryId,@Name)";

                        var idParametr = command.CreateParameter();
                        idParametr.ParameterName = "@Id";
                        idParametr.DbType = System.Data.DbType.Guid;
                        idParametr.Value = city.Id;

                        var countryIdParametr = command.CreateParameter();
                        countryIdParametr.ParameterName = "@CountryId";
                        countryIdParametr.DbType = System.Data.DbType.Guid;
                        countryIdParametr.Value = city.CountryId;

                        var nameParametr = command.CreateParameter();
                        nameParametr.ParameterName = "@Name";
                        nameParametr.DbType = System.Data.DbType.String;
                        nameParametr.Value = city.Name;

                        command.Parameters.Add(idParametr);
                        command.Parameters.Add(countryIdParametr);
                        command.Parameters.Add(nameParametr);

                        command.Transaction = transaction;
                        int affectedRows = command.ExecuteNonQuery();

                        if(affectedRows < 1)
                        {
                            throw new Exception("Вставка не удалась");
                        }
                        transaction.Commit();

                    }
                    catch (DbException exception)
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
        public void UpdateCity(City city, string newName)
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
                        command.CommandText = $"update Cities set Name=@newName where Id=@idCity";

                        var newNameParametr = command.CreateParameter();
                        newNameParametr.ParameterName = "@newName";
                        newNameParametr.DbType = System.Data.DbType.String;
                        newNameParametr.Value = newName;

                        var idParametr = command.CreateParameter();
                        idParametr.ParameterName = "@idCity";
                        idParametr.DbType = System.Data.DbType.Guid;
                        idParametr.Value = city.Id;

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

        public void DeleteCity(City city)
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
                        command.CommandText = $"delete from cities where Id=@idCity";

                        var idParametr = command.CreateParameter();
                        idParametr.ParameterName = "@idCity";
                        idParametr.DbType = System.Data.DbType.Guid;
                        idParametr.Value = city.Id;

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
