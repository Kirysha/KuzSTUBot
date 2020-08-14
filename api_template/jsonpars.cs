using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api_template
{
   static public class jsonpars
    {
        private static readonly HttpClient client = new HttpClient();
        private const string scheduleKey = "listSchedule";
        static public List<MainSchedule> jsonp(IMemoryCache _cache)
        {
            List<MainSchedule> result; 
            if (!_cache.TryGetValue(scheduleKey, out result))
            {
                string urlAIP = "https://portal.kuzstu.ru/extra/api/schedule";// ConfigurationManager.AppSettings["urlAIP"];
                HttpWebRequest Request = WebRequest.Create(new Uri(urlAIP)) as HttpWebRequest;
                Request.Method = "GET";
                Request.ContentType = "application/json";

                string _TextRequest = "";

                byte[] utf8bytes = Encoding.UTF8.GetBytes(_TextRequest);
                byte[] iso8859bytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("iso-8859-1"), utf8bytes);

                Request.ContentLength = iso8859bytes.Length;
                HttpWebResponse response = Request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string _rawResponse = reader.ReadToEnd();
                string pattern = "\"[0-9]{6}\" *: *";
                string target = "\"schedule\":";
                Regex regex = new Regex(pattern);
                string regstr = "[" + regex.Replace(_rawResponse, target) + "]";
                string pattern2 = "},";
                string target2 = "}},{";
                Regex regex2 = new Regex(pattern2);
                string jsonSchedule = regex2.Replace(regstr, target2);
                IEnumerable<SSchedule> sc = JsonConvert.DeserializeObject<IEnumerable<SSchedule>>(jsonSchedule);
                int i = 0;
                result = new List<MainSchedule>();
                foreach (var res in sc)
                {
                    i = i+1;
                    MainSchedule main = new MainSchedule();
                    main.day_number = res.schedule.day_number;
                    main.education_group_id = res.schedule.education_group_id;
                    main.education_group_name = res.schedule.education_group_name;
                    main.id_schedule = i.ToString();
                    main.lesson_number = res.schedule.lesson_number;
                    main.place = res.schedule.place;
                    main.subgroup = res.schedule.subgroup;
                    main.subject = res.schedule.subject;
                    main.teacher_id = res.schedule.teacher_id;
                    main.type = res.schedule.type;
                    main.week_type = res.schedule.week_type;
                    result.Add(main);
                }
                //var teacher = result.DistinctBy(o => o.teacher_id);
                //foreach(var teache in teacher)
                //{

                //}
                _cache.Set(scheduleKey, result, TimeSpan.FromHours(24));
            }
            return result;
        }
    }
}
