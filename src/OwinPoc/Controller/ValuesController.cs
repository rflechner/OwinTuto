using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Http;

namespace OwinPoc.Controller
{
    public class ValuesController : ApiController
    {
        private readonly IIdentity identity;
        private readonly IUserRepository userRepository;
        private readonly ScopeId scopeId;

        public ValuesController(IIdentity identity, IUserRepository userRepository, ScopeId scopeId)
        {
            this.identity = identity;
            this.userRepository = userRepository;
            this.scopeId = scopeId;
        }

        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new[]
            {
                "value1",
                "value2",
                identity?.Name,
                userRepository.GetUserName(10),
                $"scopeId : {scopeId.Id}"
            };
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
}