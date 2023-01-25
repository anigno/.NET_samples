using System;
using System.Collections.Generic;
using System.Web.Http;

namespace WorkingWithHttp.OwinHosting
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }
        // POST api/values
        public void Post([FromBody]string value)
        {
        }
        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }
        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
    public class PirController : ApiController
    {
        [HttpPost]
        public IHttpActionResult PostPir(dynamic pir)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        [HttpDelete]
        public IHttpActionResult DeletePir(string id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}