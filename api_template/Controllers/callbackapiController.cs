using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/callbackapi")]
    public class callbackapiController : Controller
    {
        private IMemoryCache _cache;
        string[] day = { "Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };
       // readonly string[] comands = new string[9]{ "сегодня", "завтра", "неделя", "пн", "вт", "ср", "чт", "пт", "сб" };
        string[] startlessons = { "9:00", "10:50", "13:20", "15:10", "17:00", "18:50", "20:30" };
        string[] endlessons = { "10:30", "12:20", "14:50", "16:40", "18:30", "20:20", "22:00" };
        string text;

        public callbackapiController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        [HttpPost]
        public ContentResult Post([FromBody]JObject value)
        {
            string type = GetValueParam(value, "type");

            switch (type)
            {
                case "confirmation": // Тест апи вк
                    string group_id = GetValueParam(value, "group_id");
                    if (group_id == "172755957")
                    {
                        return Content("b39ef7f3");
                    }
                    break;


                case "message_new": // Новое сообщение
                    string message = GetValueParam(value, "text");
                    string messageCopy = message;
                    string[] messagecomand = message.Split(' ');
                    string comand = messagecomand[0];
                    string group = message.Substring(comand.Length);
                    string pattern = " +";
                    string target = "";
                    Regex regex = new Regex(pattern);
                    string group_name = regex.Replace(group, target);
                    Regex regexGroup = new Regex(@"[а-яА-Я ]*-\s*[0-9 ]*", RegexOptions.IgnoreCase);                    
                    text = "";
                    
                    //var a = ((allscheduleforgroup.Where(q => q.day_number == 3.ToString()).ToList()).Where(o => o.lesson_number == "1").FirstOrDefault()).subject;
                    int dayofweek = (int)DateTime.Now.DayOfWeek;
                    int weekofyear = ((((int)DateTime.Now.DayOfYear) / 7) % 2) == 0 ? 1 : 2;
                    text = "";
                    if (regexGroup.IsMatch(comand))
                    {
                        group = comand;
                        group_name = regex.Replace(group, target);
                        getScheduleDay(group_name, dayofweek, weekofyear);
                    }
                    else
                    {
                        switch (comand.ToLower())
                        {
                            case "сегодня":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, dayofweek, weekofyear);
                                break;
                            case "завтра":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, dayofweek+1, weekofyear);
                                break;
                            case "неделя":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 1, weekofyear);
                                text += "\n";
                                getScheduleDay(group_name, 2, weekofyear);
                                text += "\n";
                                getScheduleDay(group_name, 3, weekofyear);
                                text += "\n";
                                getScheduleDay(group_name, 4, weekofyear);
                                text += "\n";
                                getScheduleDay(group_name, 5, weekofyear);
                                text += "\n";
                                getScheduleDay(group_name, 6, weekofyear);
                                break;
                            case "пн":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 1, weekofyear);
                                break;
                            case "вт":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 2, weekofyear);
                                break;
                            case "ср":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 3, weekofyear);
                                break;
                            case "чт":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 4, weekofyear);
                                break;
                            case "пт":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 5, weekofyear);
                                break;
                            case "сб":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 6, weekofyear);
                                break;
                            case "понедельник":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 1, weekofyear);
                                break;
                            case "вторник":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 2, weekofyear);
                                break;
                            case "среда":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 3, weekofyear);
                                break;
                            case "четверг":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 4, weekofyear);
                                break;
                            case "пятница":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 5, weekofyear);
                                break;
                            case "суббота":
                                group_name = regex.Replace(group, target);
                                getScheduleDay(group_name, 6, weekofyear);
                                break;
                            case "спасибо":
                                text += "Пожалуйста, обращайся :)";
                                break;
                            default:
                                text += "Привет! Я выучил только эти команды: \n сегодня пиб-162 \n завтра пиб-151 \n неделя пиб-151 \n пн пиб-182 \n вт пиб-162 \n ср пиб-162 \n чт пиб-162  \n пт пиб-162 \n сб пиб-162 \n понедельник пиб-182 \n вторник пиб-162 \n среда пиб-162 \n четверг пиб-162 \n пятница пиб-162 \n суббота пиб-162 \n не забудь поделиться мной с друзьями :)";
                                break;
                        }
                    }

                    // Поиск id отправителя
                    string from_id = GetValueParam(value, "from_id");
                    //*****************************************
                    // Рандомное число для предотвращения зацикливания
                    string random_id = GetValueParam(value, "id");
                    //*****************************************
                    //***************Тест**********************
                    //string key = @"";
                    //List<Button> lbuttons = new List<Button>();
                    //lbuttons.Add(new Button() { color = "positive" , action = new Action() { label = "Green", payload = "{\"button\": \"2\"}", type = "text"} });
                    
                   
                    //allcodejsonkey allcodejsonkey = new allcodejsonkey { one_time = false, buttons = lbuttons };
                    //var s = JsonConvert.SerializeObject(allcodejsonkey);
                    //string str = "{\"one_time\":false,\"buttons\":[{\"action\":{\"type\":\"text\",\"payload\":\"{\\\"button\\\": \\\"2\\\"}\",\"label\":\"Green\"},\"color\":\"positive\"}]}";
                    ////string ss = {"one_time":false,"buttons":[{"action":{"type":"text","payload":"{\"button\": \"2\"}","label":"Green"},"color":"positive"}]}"

                    //string ss = str.Replace(Convert.ToChar(@"\"),' ');
                    
                    //*****************************************
                    var client = new RestClient("https://api.vk.com/method/messages.send");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    request.AddHeader("cache-control", "no-cache");
                    request.AddParameter("application/x-www-form-urlencoded", "user_id=" + from_id + "&peer_id=-172755957&message=" + text + "&group_id=172755957&dont_parse_links=0&random_id=" + random_id + "&v=5.85&access_token=*****", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    return Content("ok");
                    //switch (message)
                    //{
                    //    case "ПИб-162":
                    //        var client = new RestClient("https://api.vk.com/method/messages.send?user_id=24700445&peer_id=-70715556&message=" + "Здравствуйте \n кирилл \n вот \n ваше \n расписание" + "&group_id=70715556&dont_parse_links=0&v=5.85&access_token=***");
                    //        var request = new RestRequest(Method.POST);
                    //        request.AddHeader("cache-control", "no-cache");
                    //        IRestResponse response = client.Execute(request);
                    //        return Content("ok");
                    //}
                default:
                    return Content("ok");
            }
            //string reqest = "9f22019a";
            //return Ok("9f22019a");
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            //HttpResponseMessage response = Request.re;
            //response.Content = new StringContent("hello", Encoding.Unicode);
            return Content("ok");
        }

        private void getScheduleDay(string group_name, int dayofweek, int weekofyear)
        {
            
            List<MainSchedule> allscheduleforgroup = (jsonpars.jsonp(_cache).ToList()).Where(o => o.education_group_name.ToLower() == group_name.ToLower()).ToList();
            if (allscheduleforgroup.Count != 0)
            {
                //if (weekofyear == 1) text += "нечетная".ToUpper();
                //if (weekofyear == 2) text += "четная".ToUpper();
                if (dayofweek == 0) dayofweek += 1;
                text += "«" + day[dayofweek] + "» \n";
                for (int i = 1; i <= 6; i++)
                {
                    var lesall = ((allscheduleforgroup.
                        Where(q => (q.day_number == dayofweek.ToString())).ToList()).
                        Where(o => o.lesson_number == i.ToString()).ToList()).
                        Where(w => (w.week_type == weekofyear.ToString()) || (w.week_type == 0.ToString()));
                    if (lesall.Count() > 1)
                    {
                        int j = 1;
                        text += "(" +i.ToString() + ") — ";
                        foreach (var les in lesall)
                        {
                            if (les == null)
                            {
                                text += "— нет пары — \n";
                            }
                            else
                            {
                                if (j == 1)
                                {
                                    text += les.type.ToString().ToLower().Remove(3).Replace(".", "") + ". " + les.subject + " " + les.place + " / ";
                                } else
                                {
                                    text += les.type.ToString().ToLower().Remove(3).Replace(".", "") + ". " + les.subject + " " + les.place + " [" + startlessons[i - 1] + "-" + endlessons[i - 1] + "]" + "\n";
                                }
                            }
                            j += 1;
                        }
                    } else
                    {
                        var les = lesall.FirstOrDefault();
                        if (les == null)
                        {
                            text += "(" + i.ToString() + ") — нет пары — \n";
                        }
                        else
                        {
                            text += "(" + i.ToString() + ") — " + les.type.ToString().ToLower().Remove(3).Replace(".", "") + ". " + les.subject + " " + les.place + " [" + startlessons[i - 1] + "-" + endlessons[i - 1] + "]" + "\n";
                        }
                    }
                }
            }
            else
            {
                text += "Группа не найдена :(";
            }
        }

        private static string GetValueParam(JObject value,string name)
        {
            var values = value.DescendantsAndSelf()
                                    .OfType<JProperty>()
                                    .Where(p => p.Name == name)
                                    .Select(p => p.Value);
            string valueParam = values.FirstOrDefault().ToString();
            return valueParam;
        }

        public class allcodejsonkey
        {
            public bool one_time { get; set; }
            public List<Button> buttons { get; set; }
        }

        public class Action
        {
            public string type { get; set; }
            public string payload { get; set; }
            public string label { get; set; }
        }

        public class Button
        {
            public Action action { get; set; }
            public string color { get; set; }
        }
    }
}