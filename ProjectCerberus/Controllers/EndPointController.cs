using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProjectCerberus.Controllers
{
    public class EndPointController : ApiController
    {
        // GET: api/EndPoint
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/EndPoint/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/EndPoint
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/EndPoint/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EndPoint/5
        public void Delete(int id)
        {
        }
    }
}
