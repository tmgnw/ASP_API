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
    public class DepartmentController : ApiController
    {
        DepartmentRepository department = new DepartmentRepository();

        [HttpGet]
        public IEnumerable<Department> Get()
        {
            return department.Get();
        }

        public IHttpActionResult Post(Department departments)
        {
            var post = department.Create(departments);
            if (post > 0)
            {
                return Ok("Department Added Successfully");
            }
            return BadRequest("Failed to Add Department");
        }

        [HttpGet]
        [ResponseType(typeof(Department))]
        public async Task<IEnumerable<Department>> Get(int Id)
        {
            return await department.Get(Id);
        }

        public IHttpActionResult Put(int Id, Department departments)
        {
            var put = department.Update(Id, departments);
            if (put > 0)
            {
                return Ok("Department Update Successfully");
            }
            return BadRequest("Failed to Update Department");
        }

        public IHttpActionResult Delete(int Id)
        {
            var del = department.Delete(Id);
            if (del > 0)
            {
                return Ok("Department Deleted Successfully");
            }
            return BadRequest("Failed to Delete Department");
        }
    }
}
