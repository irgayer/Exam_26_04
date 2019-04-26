using Exam_26_04.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_26_04.DbManager
{
    public class StreetService : Initializer
    {
        public List<Street> SelectStreets()
        {
            var data = new List<Street>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        command.CommandText = "Select * from Streets";

                        var sqlDataReader = command.ExecuteReader();

                        while (sqlDataReader.Read())
                        {
                            Guid id = Guid.Parse(sqlDataReader["Id"].ToString());
                            Guid cityId = Guid.Parse(sqlDataReader["CityId"].ToString());
                            string name = sqlDataReader["Name"].ToString();

                            data.Add(new Street
                            {
                                Id = id,
                                CityId = cityId,
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

        public void InsertStreet(Street street)
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
                        command.CommandText = "insert into Streets values(@Id,@CityId,@Name)";

                        var idParametr = command.CreateParameter();
                        idParametr.ParameterName = "@Id";
                        idParametr.DbType = System.Data.DbType.Guid;
                        idParametr.Value = street.Id;

                        var nameParametr = command.CreateParameter();
                        nameParametr.ParameterName = "@Name";
                        nameParametr.DbType = System.Data.DbType.String;
                        nameParametr.Value = street.Name;

                        var cityIdParametr = command.CreateParameter();
                        cityIdParametr.ParameterName = "@CityId";
                        cityIdParametr.DbType = System.Data.DbType.Guid;
                        cityIdParametr.Value = street.CityId;

                        command.Parameters.Add(idParametr);
                        command.Parameters.Add(cityIdParametr);
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

        public void UpdateStreet(Street street, string newName)
        {
            using(connection = new SqlConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        transaction = connection.BeginTransaction();
                        command.CommandText = $"update streets set Name=@newName where Id=@idStreet";

                        var newNameParametr = command.CreateParameter();
                        newNameParametr.ParameterName = "@newName";
                        newNameParametr.DbType = System.Data.DbType.String;
                        newNameParametr.Value = newName;

                        var idParametr = command.CreateParameter();
                        idParametr.ParameterName = "@idStreet";
                        idParametr.DbType = System.Data.DbType.Guid;
                        idParametr.Value = street.Id;

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
                    catch(SqlException exception)
                    {
                        transaction?.Rollback();
                    }
                    catch(Exception exception)
                    {
                        transaction?.Rollback();
                    }
                }
            }
        }
        public void DeleteStreet(Street street)
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
                        command.CommandText = $"delete from streets where Id=@idStreet";

                        var idParametr = command.CreateParameter();
                        idParametr.ParameterName = "@idStreet";
                        idParametr.DbType = System.Data.DbType.Guid;
                        idParametr.Value = street.Id;

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
