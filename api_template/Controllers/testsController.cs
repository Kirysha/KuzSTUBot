using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace api_template.Controllers
{
    [Produces("application/json")]
    [Route("api/tests")]
    public class testsController : Controller
    {
        private IMemoryCache _cache;

        public testsController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        [HttpPost]
        public ContentResult Post([FromBody]JObject value)
        {
            string type = GetValueParam(value, "type");
            var idUserValues =
                   value.DescendantsAndSelf()
                       .OfType<JProperty>()
                       .Where(p => p.Name == "type")
                       .Select(p => p.Value);
            string idUser = idUserValues.FirstOrDefault().ToString();

            switch (type)
            {
                case "confirmation": // Тест апи вк
                    var group_idValues =
                            value.DescendantsAndSelf()
                            .OfType<JProperty>()
                            .Where(p => p.Name == "group_id")
                            .Select(p => p.Value);
                    string group_id = group_idValues.FirstOrDefault().ToString();
                    if (group_id == "172755957")
                    {
                        return Content("b39ef7f3");
                    }
                    break;


                case "message_new": // Новое сообщение
                    var messageValues =
                            value.DescendantsAndSelf()
                            .OfType<JProperty>()
                            .Where(p => p.Name == "text")
                            .Select(p => p.Value);
                    string message = messageValues.FirstOrDefault().ToString();
                    string[] messagecomand = message.Split(' ');
                    string group = "";
                    string comand = "";
                    if (messagecomand.Count() == 1)
                    {
                        group = messagecomand[0];
                        comand = null;
                    }
                    else
                    {
                        group = messagecomand[1];
                        comand = messagecomand[0];
                    }
                    string pattern = " +";
                    string target = "";
                    Regex regex = new Regex(pattern);
                    string group_name = regex.Replace(group, target);
                    string[] day = { "Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };
                    string[] comands = { "сегодня", "завтра", "неделя", "пн", "вт", "ср", "чт", "пт", "сб" };
                    string[] startlessons = { "9:00", "10:50", "13:20", "15:10", "17:00", "18:50", "20:30" };
                    string[] endlessons = { "10:30", "12:20", "14:50", "16:40", "18:30", "20:20", "22:00" };
                    string text = "";
                    List<MainSchedule> allscheduleforgroup = (jsonpars.jsonp(_cache).ToList()).Where(o => o.education_group_name.ToLower() == group_name.ToLower()).ToList();
                    //var a = ((allscheduleforgroup.Where(q => q.day_number == 3.ToString()).ToList()).Where(o => o.lesson_number == "1").FirstOrDefault()).subject;
                    int dayofweek = (int)DateTime.Now.DayOfWeek;
                    int weekofyear = ((((int)DateTime.Now.DayOfYear) / 7) % 2) == 0 ? 1 : 2;

                    if (comand == null)
                    {
                        text = "";
                        if (allscheduleforgroup.Count != 0)
                        {
                            text += "«" + day[dayofweek] + "» \n";
                            for (int i = 1; i <= 6; i++)
                            {
                                var les = ((allscheduleforgroup.
                                    Where(q => (q.day_number == dayofweek.ToString()) || (q.day_number == 0.ToString())).ToList()).
                                    Where(o => o.lesson_number == i.ToString()).ToList()).
                                    Where(w => (w.week_type == weekofyear.ToString()) || (w.week_type == 0.ToString())).FirstOrDefault();
                                if (les == null)
                                {
                                    text += i.ToString() + " | — нет пары — \n";
                                }
                                else
                                {
                                    text += i.ToString() + " | " + les.subject + " " + les.place + " (" + startlessons[i - 1] + "-" + endlessons[i - 1] + ")" + "\n";
                                }
                            }
                        }
                        else
                        {
                            text += "Группа не найдена :(";
                        }
                        //    " - 10:30 \n • 10:50 - "+ (allscheduleforgroup.Where(q => q.day_number == dayofweek.ToString()).ToList()).Where(o => o.lesson_number == "2").FirstOrDefault().subject+" ДОРОГАНОВ В.С.а."+ (allscheduleforgroup.Where(q => q.day_number == dayofweek.ToString()).ToList()).Where(o => o.lesson_number == "2").FirstOrDefault().place + 
                        //    " - 12:20 \n • 13:20 - "+ (allscheduleforgroup.Where(q => q.day_number == dayofweek.ToString()).ToList()).Where(o => o.lesson_number == "3").FirstOrDefault().subject + " РЕЧКО Г.Н. - а."+ (allscheduleforgroup.Where(q => q.day_number == dayofweek.ToString()).ToList()).Where(o => o.lesson_number == "3").FirstOrDefault().place + 
                        //    " - 14:50 \n • 15:10 - "+ (allscheduleforgroup.Where(q => q.day_number == dayofweek.ToString()).ToList()).Where(o => o.lesson_number == "4").FirstOrDefault().subject + " ТАЙЛАКОВА А.А.а."+ (allscheduleforgroup.Where(q => q.day_number == dayofweek.ToString()).ToList()).Where(o => o.lesson_number == "4").FirstOrDefault().place + " - 17:20";
                    }
                    else
                    {

                    }

                    var from_idValues = // Поиск id отправителя
                            value.DescendantsAndSelf()
                            .OfType<JProperty>()
                            .Where(p => p.Name == "from_id")
                            .Select(p => p.Value);
                    string from_id = from_idValues.FirstOrDefault().ToString();
                    //*****************************************
                    var random_idValues = // Рандомное число для предотвращения зацикливания
                            value.DescendantsAndSelf()
                            .OfType<JProperty>()
                            .Where(p => p.Name == "id")
                            .Select(p => p.Value);
                    string random_id = random_idValues.FirstOrDefault().ToString();
                    //*****************************************
                    var client = new RestClient("https://api.vk.com/method/messages.send?user_id=" + from_id + "&peer_id=-172755957&message=" + text + "&group_id=172755957&dont_parse_links=0&random_id=" + random_id + "&v=5.85&access_token=13d1f2ee3e7d319db8332b088288fdfc41a42d09cf13d81e415e2b82fbd2770d08dcd4bb460261151fb09");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    IRestResponse response = client.Execute(request);
                    return Content("ok");
                    //switch (message)
                    //{
                    //    case "ПИб-162":
                    //        var client = new RestClient("https://api.vk.com/method/messages.send?user_id=24700445&peer_id=-70715556&message=" + "Здравствуйте \n кирилл \n вот \n ваше \n расписание" + "&group_id=70715556&dont_parse_links=0&v=5.85&access_token=d3dda5cdbb12d08dcd4487f779458a53c875477a0dc417e6755a9ab1bf6f92397b3c78bdc965732db54f0");
                    //        var request = new RestRequest(Method.POST);
                    //        request.AddHeader("cache-control", "no-cache");
                    //        IRestResponse response = client.Execute(request);
                    //        return Content("ok");
                    //}
                    break;
                default:
                    return Content("ok");
            }
            //string reqest = "9f22019a";
            //return Ok("9f22019a");
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            //HttpResponseMessage response = Request.re;
            //response.Content = new StringContent("hello", Encoding.Unicode);
            return Content("ok");
            return Content("9f22019a");
        }
    

        private static string GetValueParam(JObject value, string Name)
        {
            var Values =
                                value.DescendantsAndSelf()
                                    .OfType<JProperty>()
                                    .Where(p => p.Name == Name)
                                    .Select(p => p.Value);
            string ValParam = Values.FirstOrDefault().ToString();
            return ValParam;
        }
    }
}