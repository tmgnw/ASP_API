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

        [HttpGet]
        [ResponseType(typeof(Department))]
        public async Task<IEnumerable<Department>> Get(int Id)
        {
            return await department.Get(Id);
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

        static List<Department> departments = new List<Department>()
        {
            new Department() { Id = 1, Name = "Thom"},
            new Department() { Id = 2, Name = "Tum"},
            new Department() { Id = 3, Name = "Tom"}
        };

        //public HttpResponseMessage Get()
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, departments);
        //}

        //public HttpResponseMessage Get(int id)
        //{
        //    var department = departments.FirstOrDefault(d => d.Id == id);
        //    if(department == null)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department not found");
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, department);
        //}
    }
}
