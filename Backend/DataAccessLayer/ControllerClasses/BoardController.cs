using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses
{
    internal class BoardController : Controller
    {
        public const string TABLENAME = "boardTable";
        public readonly string[] COLUMNSNAMES = new string[] { "ID", "owner", "boardName" };
        public readonly string[] IDENTIFIERS = new string[] { "ID" };

        private ILog log = LogClass.log;


        public BoardController() : base () { }
        /// <summary>
        /// This method inserts the board's data into the database.
        /// </summary>
        /// <param name="attributesValues">values of the fields of the board</param>
        /// <returns>true/false depending on whether the insertion worked</returns>
        public override bool Insert(object[] attributesValues)
        {
            return Insert(COLUMNSNAMES, attributesValues, TABLENAME);
        }
        /// <summary>
        /// This method updates a board's data field within the databse.
        /// </summary>
        /// <param name="identifiersValues">identifiers of the fields of the board</param>
        /// <param name="varToUpdate">the data field to update</param>
        /// <param name="valueToUpdate">the field's new value</param>
        /// <returns>true/false depending on whether the update worked</returns>
        public override bool Update(object[] identifiersValues, string varToUpdate,object valueToUpdate)
        {
            return Update(IDENTIFIERS, identifiersValues, TABLENAME, varToUpdate, valueToUpdate);
        }
        /// <summary>
        /// This method deletes an entry of a board from the databse.
        /// </summary>
        /// <param name="identifiersValues">identifiers of the fields of the board</param>
        /// <returns>true/false depending on whether the deletion worked</returns>
        public override bool Delete(object[] identifiersValues)
        {
            Delete(new string[] { "boardID" }, identifiersValues, MemberController.TABLENAME);
            Delete(new string[] { "boardID" }, identifiersValues, TaskController.TABLENAME);
            Delete(new string[] { "boardID"}, identifiersValues, ColumnController.TABLENAME);
            return Delete(IDENTIFIERS, identifiersValues, TABLENAME);
        }
        /// <summary>
        /// This method extracts the values from the reader and create a new BoardDTO object
        /// </summary>
        /// <param name="reader">One line extracted from the database</param>
        /// <returns>the board's DTO</returns>
        public override BoardDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new BoardDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), this, true);
        }
        /// <summary>
        /// This method retrieves all of the boards that are owned by the given user
        /// </summary>
        /// <param name="owner"></param>
        /// <returns>A list of the owners boards as BoardDTOs</returns>
        public List<BoardDTO> SelectAllOwnerBoards(string owner) 
        {
            List<BoardDTO> results = new List<BoardDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteParameter paramValue = new SQLiteParameter(@"paramVal", owner);
                command.CommandText = $"select * from {TABLENAME} where owner = @paramVal;";
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
                    log.Info($"Select all owner boards commited successfuly!");

                }
                catch (Exception ex)
                {
                    log.Error($"An unexpected error occurred while selecting owner boards: {ex.Message}");
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
