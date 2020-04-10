using ProjectApi.Models;
using ProjectApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ProjectApi.Controllers
{
    public class DevisiController : ApiController
    {

        DevisiRepository devisi = new DevisiRepository();

        //code old
        //[HttpGet]
        //public IEnumerable<Devisi> Get()
        //{
        //    return devisi.Get();
        //}

        public IHttpActionResult Get()
        {
            if (devisi.Get() == null)
            {
                return Content(HttpStatusCode.NotFound, "Data Devisi is Empty!");
            }
            return Ok(devisi.Get());
        }

        //code old
        //[HttpGet]
        //[ResponseType(typeof(Devisi))]
        //public async Task<IEnumerable<Devisi>> Get(int Id)
        //{
        //    return await devisi.Get(Id);
        //}

        [HttpGet]
        [ResponseType(typeof(DevisiVM))]
        public async Task<IEnumerable<DevisiVM>> Get(int Id)
        {
            if (await devisi.Get(Id) == null)
            {
                return null;
            }
            return await devisi.Get(Id);
        }

        public IHttpActionResult Post(Devisi devisies)
        {
            if ((devisies.Name != null) && (devisies.Name != ""))
            {
                devisi.Create(devisies);
                return Ok("Devisi Added Successfully!"); //Status 200 OK
            }
            return BadRequest("Failed to Add Devisi");
        }

        public IHttpActionResult Put(int Id, Devisi devisies)
        {
            if ((devisies.Name != null) && (devisies.Name != ""))
            {
                devisi.Update(Id, devisies);
                return Ok("Devisi Updated Successfully!"); //Status 200 OK
            }
            return BadRequest("Failed to Update Devisi");
        }

        public IHttpActionResult Delete(int Id)
        {
            var del = devisi.Delete(Id);
            if (del > 0)
            {
                return Ok("Devisi Deleted Successfully");
            }
            return BadRequest("Failed to Delete Devisi");
        }

    }
}
