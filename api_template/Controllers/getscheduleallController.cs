using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace api_template.Controllers
{
    [Produces("application/json")]
    [Route("api/getscheduleall")]
    public class getscheduleallController : Controller
    {
        private IMemoryCache _cache;

        public getscheduleallController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        // GET: api/ItemsSchedule
        [HttpGet]
        public IEnumerable<MainSchedule> Get()
        {
            IEnumerable<MainSchedule> result = jsonpars.jsonp(_cache);            
            return result;
        }
    }
}