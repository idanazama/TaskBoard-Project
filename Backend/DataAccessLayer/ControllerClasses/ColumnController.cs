using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses
{
    internal class ColumnController : Controller
    {
        public const string TABLENAME = "columnTable";
        public readonly string[] COLUMNSNAMES = new string[] { "boardID", "columnOrdinal", "columnLimit" };
        public readonly string[] IDENTIFIERS = new string[] { "boardID", "columnOrdinal" };

        private ILog log = LogClass.log;


        public ColumnController() : base() { }

        /// <summary>
        /// This method inserts the columns's data into the database.
        /// </summary>
        /// <param name="attributesValues">values of the fields of the column</param>
        /// <returns>true/false depending on whether the insertion worked</returns>
        public override bool Insert(object[] attributesValues)
        {
            return Insert(COLUMNSNAMES, attributesValues, TABLENAME);
        }

        /// <summary>
        /// This method updates a columns's data field within the databse.
        /// </summary>
        /// <param name="identifiersValues">identifiers of the fields of the column</param>
        /// <param name="varToUpdate">the data field to update</param>
        /// <param name="valueToUpdate">the field's new value</param>
        /// <returns>true/false depending on whether the update worked</returns>

        public override bool Update(object[] identifiersValues, string varToUpdate, object valueToUpdate)
        {
            return Update(IDENTIFIERS, identifiersValues, TABLENAME, varToUpdate, valueToUpdate);
        }

        /// <summary>
        /// This method deletes an entry of a column from the databse.
        /// </summary>
        /// <param name="identifiersValues">identifiers of the fields of the column</param>
        /// <returns>true/false depending on whether the deletion worked</returns>
        public override bool Delete(object[] identifiersValues)
        {
            return Delete(IDENTIFIERS, identifiersValues, TABLENAME);
        }

        /// <summary>
        /// This method retrieves all of the columns that are related to the given board
        /// </summary>
        /// <param name="id">the board's ID</param>
        /// <returns>A list of the board's columns as COlumnDTOs</returns>
        public List<ColumnDTO> SelectColumnsFromID(int id)
        {
            List<ColumnDTO> results = new List<ColumnDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                SQLiteParameter paramValue = new SQLiteParameter(@"paramVal", id);
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
                    log.Info($"Select all columns from id commited successfuly!");

                }
                catch (Exception ex)
                {
                    log.Error($"An unexpected error occurred while selecting columns from id: {ex.Message}");
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

        /// <summary>
        /// This method extracts the values from the reader and create a new ColumnDTO object
        /// </summary>
        /// <param name="reader">One line extracted from the database</param>
        /// <returns>the column's DTO</returns>
        public override ColumnDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new ColumnDTO(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), this,true);
        }


    }
}
