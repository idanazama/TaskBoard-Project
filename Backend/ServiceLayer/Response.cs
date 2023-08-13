using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        public string ErrorMessage { get; set; }
        public object ReturnValue { get; set; }
        public bool ErrorOccured { get => ErrorMessage != null; }

        public Response(string errorMessage,object returnValue) 
        {
            ErrorMessage = errorMessage;
            ReturnValue = returnValue;
        }
    }
}
