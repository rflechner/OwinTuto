using System;
using System.Web.Http;

namespace OwinPoc.Controller
{
    public class ProductsController : ApiController
    {

        public IHttpActionResult GetProduct(int id)
        {
            return Ok(new 
            {
                Name = $"name_{id}",
                Date = DateTime.UtcNow
            });
        }
    }
}