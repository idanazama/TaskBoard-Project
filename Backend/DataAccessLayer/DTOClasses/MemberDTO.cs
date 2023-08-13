using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses
{
    internal class MemberDTO : DTO
    {
        int boardID;
        string email;

        public int GetID()
        {
            return boardID;
        }
        public string GetEmail()
        {
            return email;
        }
        public MemberDTO(int boardID, string email,MemberController controller, bool fromDB) : base(controller, fromDB)
        {
            this.boardID = boardID;
            this.email = email;
        }

        /// <summary>
        /// Persists the user into the database.
        /// </summary>
        public override void Persist()
        {
            Insert(new object[] {boardID, email});
            isPersisted = true;
        }

        /// <summary>
        /// Deletes the user from the database.
        /// </summary>
        public void DeleteMember()
        {
            if(isPersisted)
            {
                Delete(new object[] { boardID, email});
                isPersisted = false;
            }
        }
    }
}
