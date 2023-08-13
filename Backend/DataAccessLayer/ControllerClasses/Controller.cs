using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using log4net;
using System.Data.SqlClient;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses;

namespace IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses
{
    internal abstract class Controller
    {
        private ILog log = LogClass.log;

        private readonly string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
        protected readonly string connectionString;
        protected Controller()
        {
            this.connectionString = $"Data Source={path}; Version=3;";
        }

        public abstract bool Insert(object[] attributesValues);

        public abstract bool Update(object[] identifiersValues, string varToUpdate, object valueToUpdate);

        public abstract bool Delete(object[] identifiersValues);

        public abstract DTO ConvertReaderToObject(SQLiteDataReader reader);

        /// <summary>
        /// This method inserts the data of the given table into the database.
        /// </summary>
        /// <param name="attributesNames">names of the fields of the table</param>
        /// <param name="attributeValues">values of the fields if the table</param>
        /// <param name="tableName">name of the table in the database</param>
        /// <returns>true/false depending on whether the insertion worked</returns>
        /// <exception cref="Exception"></exception>
        protected bool Insert(string[] attributesNames, object[] attributeValues,string tableName)
        {
            if(attributesNames.Length!=attributeValues.Length)
            {
                log.Error($"Insert into {tableName} failed!");
                throw new Exception("amount of attributes and values differ!");
            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                try
                {
                    command.CommandText = $"INSERT INTO {tableName} (" ;
                    for(int i = 0;i<attributesNames.Length;i++)
                    {
                        command.CommandText += attributesNames[i];
                        if (i < attributesNames.Length - 1)
                        {
                            command.CommandText += ",";
                        }
                        else
                        {
                            command.CommandText += ") VALUES (";
                        }

                    }
                    for(int i=0;i<attributeValues.Length;i++)
                    {
                        command.CommandText += "@paramVal" + i;
                        if(i<attributeValues.Length-1)
                        {
                            command.CommandText += ",";
                        }
                        else
                        {
                            command.CommandText += ");";
                        }
                    }
                    SQLiteParameter[] paramValues = new SQLiteParameter[attributeValues.Length];
                    for (int i = 0; i < attributeValues.Length; i++)
                    {
                        paramValues[i] = new SQLiteParameter(@"paramVal" + i, attributeValues[i]);
                    }
                    foreach (SQLiteParameter param in paramValues)
                    {
                        command.Parameters.Add(param);
                    }
                    connection.Open();
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Info($"Insert into {tableName} commited successfuly!");

                }
                catch (Exception e)
                {
                    log.Error($"Insert into {tableName} failed!");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res == 1;
            }
        }
        /// <summary>
        /// This method updates a tables's data field within the databse.
        /// </summary>
        /// <param name="identifiers">identifiers of the fields of the table</param>
        /// <param name="identifiersValues">values of the identifiers of the fields of the table</param>
        /// <param name="tableName">name of the table in the database</param>
        /// <param name="varToUpdate">the data ield to update</param>
        /// <param name="valueToUpdate">the field's new value</param>
        /// <returns>true/false depending on whether the update worked</returns>
        /// <exception cref="Exception"></exception>
        protected bool Update(string[] identifiers, object[] identifiersValues, string tableName, string varToUpdate, object valueToUpdate)
        {
            if (identifiers.Length != identifiersValues.Length)
            {
                log.Error($"Update in {tableName} failed!");
                throw new Exception("amount of identifiers and values differ!");

            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                try
                {
                    

                    command.CommandText = $"UPDATE {tableName} SET {varToUpdate} = @paramUpdate where "; 

                    for(int i =0;i < identifiersValues.Length;i++)
                    {
                        command.CommandText += identifiers[i] + " = " + "@paramVal" + i + " ";
                        if (i < identifiersValues.Length - 1)
                        {
                            command.CommandText += " and ";
                        }
                    }
                    SQLiteParameter[] paramValues = new SQLiteParameter[identifiersValues.Length];
                    for (int i = 0; i < identifiersValues.Length; i++)
                    {
                        paramValues[i] = new SQLiteParameter(@"paramVal" + i, identifiersValues[i]);
                    }
                    command.Parameters.Add(new SQLiteParameter("@paramUpdate", valueToUpdate));
                    foreach (SQLiteParameter param in paramValues)
                    {
                        command.Parameters.Add(param);
                    }
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Update in {tableName} commited successfuly!"); 

                }
                catch (Exception e)
                {
                    log.Error($"Update in {tableName} failed!"); 
                    throw new Exception(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res == 1;
            }
        }

        /// <summary>
        /// his method deletes an entry from the table in the databse.
        /// </summary>
        /// <param name="identifiers">identifiers of the fields of the table</param>
        /// <param name="identifiersValues">values of the identifiers of the fields of the table</param>
        /// <param name="tableName">name of the table in the database</param>
        /// <returns>true/false depending on whether the deletion worked</returns>
        /// <exception cref="Exception"></exception>
        protected bool Delete(string[] identifiers, object[] identifiersValues, string tableName)
        {
            if (identifiers.Length != identifiersValues.Length)
            {
                log.Error($"Delete in {tableName} failed!");
                throw new Exception("amount of identifiers and values differ!"); 
            }
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                try
                {
                    command.CommandText = $"delete from {tableName} where ";

                    for (int i = 0; i < identifiersValues.Length; i++)
                    {
                        command.CommandText += identifiers[i] + " = " + "@paramVal" + i + " ";
                        if (i < identifiersValues.Length - 1)
                        {
                            command.CommandText += " and ";
                        }
                    }
                    SQLiteParameter[] paramValues = new SQLiteParameter[identifiersValues.Length];
                    for (int i = 0; i < identifiersValues.Length; i++)
                    {
                        paramValues[i] = new SQLiteParameter(@"paramVal" + i, identifiersValues[i]);
                    }
                    foreach (SQLiteParameter param in paramValues)
                    {
                        command.Parameters.Add(param);
                    }
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"Delete in {tableName} commited successfuly!"); 

                }
                catch (Exception e)
                {
                    log.Error($"Delete in {tableName} failed!"); 
                    throw new Exception(e.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res == 1;
            }
        }
        /// <summary>
        /// This method deletes all of the data in the database.
        /// </summary>
        /// <exception cref="Exception"></exception>
        internal void DeleteAll()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = null;
                string[] tables = { MemberController.TABLENAME, TaskController.TABLENAME, ColumnController.TABLENAME, BoardController.TABLENAME, UserController.TABLENAME };

                connection.Open();
                foreach (string table in tables)
                {
                    try
                    {
                        command = new SQLiteCommand(connection);
                        command.CommandText = $"delete from {table}";
                        command.ExecuteNonQuery();
                        log.Info($"Delete all commited successfuly!");

                    }
                    catch (Exception e)
                    {
                        log.Error($"Delete all failed in table name - {table}");
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        command.Dispose();
                    }
                }
                connection.Close();


            }
        }
    }
}
