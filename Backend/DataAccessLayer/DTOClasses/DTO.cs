using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses
{
    internal abstract class DTO 
    {
        protected bool isPersisted;
        private Controller controller;

        public DTO(Controller controller,bool fromDB)
        {
            isPersisted = fromDB;
            this.controller = controller;
        }
        public abstract void Persist();

        /// <summary>
        /// Inserts a new record into the database.
        /// </summary>
        /// <param name="attributesValues">The attribute values for the record.</param>
        protected void Insert(object[] attributesValues)
        {
            if(!controller.Insert(attributesValues))
            {
                throw new Exception("An unexpected error occurred while insert");
            }
        }

        /// <summary>
        /// Updates a record in the database.
        /// </summary>
        /// <param name="identifiersValues">The identifier values for the record.</param>
        /// <param name="varToUpdate">The variable to update.</param>
        /// <param name="valueToUpdate">The new value for the variable.</param>
        protected void Update(object[] identifiersValues,string varToUpdate,object valueToUpdate)
        {
            if(!controller.Update(identifiersValues,varToUpdate,valueToUpdate))
            {
                throw new Exception("An unexpected error occurred while update");
            }
        }

        /// <summary>
        /// Deletes a record from the database.
        /// </summary>
        /// <param name="identifiersValues">The identifier values for the record.</param>
        protected void Delete(object[] identifiersValues)
        {
            if(!controller.Delete(identifiersValues))
            {
                throw new Exception("An unexpected error occurred while delete");
            }
        }
    }
}
