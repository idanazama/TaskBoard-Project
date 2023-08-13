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
    internal class MemberController : Controller
    {
        public const string TABLENAME = "memberTable";
        public readonly string[] COLUMNSNAMES = new string[] { "boardID", "email" };
        public readonly string[] IDENTIFIERS = new string[] { "boardID", "email" };

        private ILog log = LogClass.log;


        public MemberController() : base() { }

        /// <summary>
        /// This method inserts the member's data into the database.
        /// </summary>
        /// <param name="attributesValues">values of the fields of the member</param>
        /// <returns>true/false depending on whether the insertion worked</returns>
        public override bool Insert(object[] attributesValues)
        {
            return Insert(COLUMNSNAMES, attributesValues, TABLENAME);
        }

        /// <summary>
        /// This method updates a member's data field within the databse.
        /// </summary>
        /// <param name="identifiersValues">identifiers of the fields of the member</param>
        /// <param name="varToUpdate">the data field to update</param>
        /// <param name="valueToUpdate">the field's new value</param>
        /// <returns>true/false depending on whether the update worked</returns>
        public override bool Update(object[] identifiersValues, string varToUpdate, object valueToUpdate)
        {
            return Update(IDENTIFIERS, identifiersValues, TABLENAME, varToUpdate, valueToUpdate);
        }

        /// <summary>
        /// This method deletes an entry of a member from the databse.
        /// </summary>
        /// <param name="identifiersValues">identifiers of the fields of the member</param>
        /// <returns>true/false depending on whether the deletion worked</returns>
        public override bool Delete(object[] identifiersValues)
        {
            return Delete(IDENTIFIERS, identifiersValues, TABLENAME);
        }

        /// <summary>
        /// This method extracts the values from the reader and create a new memberDTO object
        /// </summary>
        /// <param name="reader">One line extracted from the database</param>
        /// <returns>the member's DTO</returns>
        public override MemberDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new MemberDTO(reader.GetInt32(0), reader.GetString(1),this,true);
        }

        /// <summary>
        /// This method retrieves all of the members that belong to the given board
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A list of the board's members as MemberDTOs</returns>
        public List<MemberDTO> SelectMembersFromID(int id)
        {
            List<MemberDTO> results = new List<MemberDTO>();
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
                    log.Info($"Select all members commited successfuly!");

                }
                catch (Exception ex)
                {
                    log.Error($"An unexpected error occurred while selecting members: {ex.Message}");
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
