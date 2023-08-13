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
    internal class UserController : Controller
    {
        public const string TABLENAME = "userTable";
        public readonly string[] COLUMNSNAMES = new string[] { "email", "userPassword" };
        public readonly string[] IDENTIFIERS = new string[] { "email"};
        private ILog log = LogClass.log;


        public UserController() : base() { }

        /// <summary>
        /// This method inserts the user's data into the database.
        /// </summary>
        /// <param name="attributesValues">values of the fields of the user</param>
        /// <returns>true/false depending on whether the insertion worked</returns>
        public override bool Insert(object[] attributesValues)
        {
            return Insert(COLUMNSNAMES, attributesValues, TABLENAME);
        }

        /// <summary>
        /// This method updates a user's data field within the databse.
        /// </summary>
        /// <param name="identifiersValues">identifiers of the fields of the user</param>
        /// <param name="varToUpdate">the data field to update</param>
        /// <param name="valueToUpdate">the field's new value</param>
        /// <returns>true/false depending on whether the update worked</returns>
        public override bool Update(object[] identifiersValues, string varToUpdate, object valueToUpdate)
        {
            return Update(IDENTIFIERS, identifiersValues, TABLENAME, varToUpdate, valueToUpdate);
        }

        /// <summary>
        /// This method deletes an entry of a user from the databse.
        /// </summary>
        /// <param name="identifiersValues">identifiers of the fields of the user</param>
        /// <returns>true/false depending on whether the deletion worked</returns>
        public override bool Delete(object[] identifiersValues)
        {
            return Delete(IDENTIFIERS, identifiersValues, TABLENAME);
        }

        /// <summary>
        /// This method retrieves all of the users that are owned by the given user
        /// </summary>
        /// <returns>A list of the users as UserDTOs</returns>
        public List<UserDTO> Select()
        {
            List<UserDTO> results = new List<UserDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TABLENAME};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                    log.Info($"Select all users commited successfuly!");

                }
                catch (Exception ex)
                {
                    log.Error($"An unexpected error occurred while selecting users: {ex.Message}");
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
        /// This method extracts the values from the reader and create a new userDTO object
        /// </summary>
        /// <param name="reader">One line extracted from the database</param>
        /// <returns>the user's DTO</returns>
        public override UserDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new UserDTO(reader.GetString(0), reader.GetString(1),this, true);
        }

        
    }
}
