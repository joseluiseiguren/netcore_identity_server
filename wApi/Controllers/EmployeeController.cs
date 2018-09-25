using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WApi.Controllers
{
    //solo los clientes que tengan el scope al cual hace referencia esta policy (protectedScopeEmployee), podràn acceder a este end point
    [Authorize(Policy = "protectedScopeEmployee")]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "employee1", "employee2", "employee3" };
        }

        // GET api/employee/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "employee" + id.ToString();
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
