using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace api_template
{
    public static class WorkTeacher
    {
        public static  void addteacher()
        {
            var client = new RestClient("https://portal.kuzstu.ru/extra/api/persons");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            System.IO.File.WriteAllText("teacher.json", response.Content);
        }  
    }
}
