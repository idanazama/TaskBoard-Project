using IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses
{
    internal class TaskController : Controller
    {
        public const string TABLENAME = "taskTable";
        public readonly string[] COLUMNSNAMES = new string[] { "taskID", "boardID", "creationDate", "title", "description", "dueDate", "assigneeEmail", "columnOrdinal" };
        public readonly string[] IDENTIFIERS = new string[] { "taskID", "boardID" };
        private ILog log = LogClass.log;


        public TaskController() : base () { }

        /// <summary>
        /// This method inserts the task's data into the database.
        /// </summary>
        /// <param name="attributesValues">values of the fields of the task</param>
        /// <returns>true/false depending on whether the insertion worked</returns>
        public override bool Insert(object[] attributesValues)
        {
            return Insert(COLUMNSNAMES, attributesValues, TABLENAME);
        }

        /// <summary>
        /// This method updates a task's data field within the databse.
        /// </summary>
        /// <param name="identifiersValues">identifiers of the fields of the task</param>
        /// <param name="varToUpdate">the data field to update</param>
        /// <param name="valueToUpdate">the field's new value</param>
        /// <returns>true/false depending on whether the update worked</returns>
        public override bool Update(object[] identifiersValues, string varToUpdate, object valueToUpdate)
        {
            return Update(IDENTIFIERS, identifiersValues, TABLENAME, varToUpdate, valueToUpdate);
        }

        /// <summary>
        /// This method deletes an entry of a task from the databse.
        /// </summary>
        /// <param name="identifiersValues">identifiers of the fields of the task</param>
        /// <returns>true/false depending on whether the deletion worked</returns>
        public override bool Delete(object[] identifiersValues)
        {
            return Delete(IDENTIFIERS, identifiersValues, TABLENAME);
        }

        /// <summary>
        /// This method extracts the values from the reader and create a new taskDTO object
        /// </summary>
        /// <param name="reader">One line extracted from the database</param>
        /// <returns>the task's DTO</returns>
        public override TaskDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            string assigneeEmail = reader[6].GetType() == typeof(DBNull) ? null : reader[6].ToString();
            return new TaskDTO(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2), reader.GetString(3)
                , reader.GetString(4), reader.GetDateTime(5), assigneeEmail, reader.GetInt32(7),this, true);
        }

        /// <summary>
        /// This method retrieves all of the tasks that belong to the given board
        /// </summary>
        /// <param name="boardID"></param>
        /// <returns>A list of the board's tasks as TaskDTOs</returns>
        public List<TaskDTO> SelectAllTasksFromBoard(int boardID)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {

                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteParameter paramValue = new SQLiteParameter(@"paramVal", boardID);
                command.CommandText = $"select * from {TABLENAME} where boardID = @paramVal;";
                command.Parameters.Add(paramValue);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                    log.Info($"Select all tasks from board commited successfuly!");

                }
                catch (Exception ex)
                {
                    log.Error($"An unexpected error occurred while selecting tasks from the board: {ex.Message}");
                    throw new Exception(ex.Message);

                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        
    }
}
