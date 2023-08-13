using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

namespace BackendTests
{
    internal class Assert
    { 
        public static void IsEmptyResponse(string response,string testName)
        { 
            Response? r = JsonSerializer.Deserialize<Response>(response);
            if (r != null)
            {
                bool error = r.ErrorMessage != null;
                bool returnValue = r.ReturnValue != null;
                if (error || returnValue)
                {
                    Console.WriteLine(testName);
                    Console.WriteLine("Expected Empty Response");
                    if (returnValue)
                    {
                        Console.WriteLine("Return value: " + r.ReturnValue);
                    }
                    else
                    {
                        Console.WriteLine("Return value: null");
                    }
                    if (error)
                    {
                        Console.WriteLine("Error Message: " + r.ErrorMessage);
                    }
                    else
                    {
                        Console.WriteLine("Error Message: null");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                throw new Exception("Got null as a response!");
            }
        }
        public static void IsErrorMessageResponse(string response, string testName)
        {
            Response? r = JsonSerializer.Deserialize<Response>(response);
            if (r != null)
            {
                if (r.ReturnValue != null)
                {
                    Console.WriteLine(testName);
                    Console.WriteLine("Expected error message, got:");
                    Console.WriteLine("Return value: " + r.ReturnValue.ToString());
                    if (r.ErrorMessage != null)
                    {
                        Console.WriteLine("Error Message: " + r.ErrorMessage);
                    }
                    else
                    {
                        Console.WriteLine("Error Message: null");
                    }
                    Console.WriteLine();
                }
                else if (r.ErrorMessage == null)
                {
                    Console.WriteLine(testName);
                    Console.WriteLine("Expected error message, got:");
                    if (r.ReturnValue != null)
                    {
                        Console.WriteLine("Return value: " + r.ReturnValue);
                    }
                    else
                    {
                        Console.WriteLine("Return value: null");
                    }
                    Console.WriteLine("Error Message: null");
                    Console.WriteLine();
                }
            }
            else
            {
                throw new Exception("Got null as a response!");
            }
        }
        public static void IsReturnEqualTo(string response, string to, string testName)
        {
            Response? r = JsonSerializer.Deserialize<Response>(response);
            if (r != null)
            {
                if (r.ReturnValue == null || !r.ReturnValue.ToString().Equals(to))
                {
                    Console.WriteLine(testName);
                    Console.WriteLine("Expected return value = " + to + " got:");
                    if (r.ReturnValue != null)
                    {
                        Console.WriteLine("Return value: " + r.ReturnValue);
                    }
                    else
                    {
                        Console.WriteLine("Return value: null");
                    }
                    if (r.ErrorMessage != null)
                    {
                        Console.WriteLine("Error Message: " + r.ErrorMessage);
                    }
                    else
                    {
                        Console.WriteLine("Error Message: null");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                throw new Exception("Got null as a response!");
            }
        }
        public static void IsReturnValueEmptyArray(string response, string testName)
        {
            Response? r = JsonSerializer.Deserialize<Response>(response);
            if (r != null)
            {
                if(r.ReturnValue!=null)
                {
                    Object[]? obj = JsonSerializer.Deserialize<Object[]>((JsonElement)r.ReturnValue);
                    if(obj != null)
                    {
                        if (obj.Length == 0)
                        {
                            return;
                        }
                    }
                }
                Console.WriteLine(testName);
                Console.WriteLine("Expected return value = empty array"  + " got:");
                if (r.ReturnValue != null)
                {
                    Console.WriteLine("Return value: " + r.ReturnValue);
                }
                else
                {
                    Console.WriteLine("Return value: null");
                }
                if (r.ErrorMessage != null)
                {
                    Console.WriteLine("Error Message: " + r.ErrorMessage);
                }
                else
                {
                    Console.WriteLine("Error Message: null");
                }
                Console.WriteLine();

            }
            else
            {
                throw new Exception("Got null as a response!");
            }
        }
    }
}
