using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace wApi.Controllers
{
    //solo los clientes que tengan el scope al cual hace referencia esta policy (protectedScopeCustomer), podràn acceder a este end point
    [Authorize(Policy = "protectedScopeCustomer")] 
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "cutomer1", "cutomer2", "cutomer3" };
        }

        // GET api/cutomer/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "cutomer" + id.ToString();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}