using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses
{
    internal class BoardDTO : DTO
    {
        int id;
        string owner;
        string name;

        public int GetID()
        {
            return id;
        }
        public string GetName() 
        {
            return name;
        }
        public string GetOwner()
        {
            return owner;
        }
        public BoardDTO(int id, string owner, string name, BoardController controller, bool fromDB) : base(controller,fromDB)
        {
            this.id = id;
            this.owner = owner;
            this.name = name;
        }

        /// <summary>
        /// Persists the board DTO into the database.
        /// </summary>
        public override void Persist()
        {
            Insert(new object[]{ id,owner,name});
            isPersisted = true;
        }

        /// <summary>
        /// Updates the owner of the board in the database.
        /// </summary>
        /// <param name="newOwner">The new owner.</param>
        public void UpdateOwner(string newOwner)
        {
            if (isPersisted)
            {
                owner = newOwner;
                Update(new object[] { id }, "owner", newOwner);
            }
        }

        /// <summary>
        /// Deletes the board from the database.
        /// </summary>
        public void DeleteBoard()
        {
            if(isPersisted)
            {
                Delete(new object[] { id });
                isPersisted = false;
            }
        }
        
    }
}
