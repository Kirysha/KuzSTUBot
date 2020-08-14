using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace api_template.Controllers
{
    [Produces("application/json")]
    [Route("api/groupSchedule")]
    public class groupScheduleController : Controller
    {
        private IMemoryCache _cache;

        public groupScheduleController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        // GET: api/groupSchedule
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/groupSchedule/5
        [HttpGet("{group}")]
        public MainScheduleL Get(string group)
        {
            IEnumerable<MainSchedule> glschedule = jsonpars.jsonp(_cache).Where(o => o.education_group_id == group);
            MainScheduleL result = new MainScheduleL() { schedules = glschedule };
            return result;
        }
        
        // POST: api/groupSchedule
        [HttpPost]
        public MainScheduleL Post([FromBody]groupJson group)
        {
            //string group = JsonConvert.DeserializeObject<groupJson>(value).group;
            IEnumerable<MainSchedule> glschedule = jsonpars.jsonp(_cache).Where(o => o.education_group_name.ToLower() == group.group.ToLower());
            MainScheduleL result = new MainScheduleL() { schedules = glschedule };
            return result;
        }
        
    }
}
