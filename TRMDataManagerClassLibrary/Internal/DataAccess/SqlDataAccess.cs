﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            //string connectionString = "Server=DESKTOP-H1OSQC9\\MSSQLSERVER2; Database=TRMDataBase; Trusted_Connection=True";
            
                using (IDbConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                //connection.Open();
                //connection.Close();
                List<T> rows = connection.Query<T>(storedProcedure, parameters, 
                    commandType: CommandType.StoredProcedure).ToList();
                return rows;
                
            }
        }
        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection())
            {
                connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);

            }
        }
    }
}
