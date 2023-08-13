using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Member
    {
        public string memberEmail { get; set; }
        private MemberDTO memberDTO;
        public Member(int boardID,string email, MemberController controller)
        {
            memberDTO = new MemberDTO(boardID, email, controller, false);
            this.memberEmail = email;
        }
        /// <summary>
        /// This method persists the data.
        /// </summary>
        public void PersistDTO()
        {
            memberDTO.Persist();
        }
        /// <summary>
        /// This method deletes the member
        /// </summary>
        public void DeleteMember()
        {
            memberDTO.DeleteMember();
        }
        public Member(MemberDTO member) {
            this.memberDTO = member;
            this.memberEmail = member.GetEmail();
        }
    }
}
