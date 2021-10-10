using System;
using RestSharp;

namespace consola
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("http://localhost:5000/students");
             client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
