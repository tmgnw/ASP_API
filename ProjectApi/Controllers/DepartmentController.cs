using ProjectApi.Models;
using ProjectApi.MyContext;
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
        myContext conn = new myContext();

        DepartmentRepository department = new DepartmentRepository();

        //code old
        [HttpGet]
        public IEnumerable<Department> Get()
        {
            return department.Get();
        }

        //public IHttpActionResult Get()
        //{
        //    return Ok(department);
        //}

        [HttpGet]
        [ResponseType(typeof(Department))]
        public async Task<IEnumerable<Department>> Get(int Id)
        {
            return await department.Get(Id);
        }
        
        public IHttpActionResult Post(Department departments)
        {
            if (departments.Name == "")
            {
                return Content(HttpStatusCode.NotFound, "Failed to Add Department");
            }
            department.Create(departments);
            return Ok("Department Added Successfully");
                
            //if (departments.Name == "")
            //{
            //    return Content(HttpStatusCode.NotFound, "Failed to Add Department");
            //}
            //department.Create(departments);
            //return Ok("Department Added Successfully");

            //code old
            //var post = department.Create(departments);
            //if (post > 0)
            //{
            //    return Ok("Department Added Successfully");
            //}
            //return BadRequest("Failed to Add Department");
        }

        public IHttpActionResult Put(int Id, Department departments)
        {
            var dept_id = conn.department.FirstOrDefault(x => x.Id == Id);

            if (dept_id == null)
            {
                return Content(System.Net.HttpStatusCode.NotFound, "Id not found");
            }
            else if (departments.Name == "")
            {
                return Content(System.Net.HttpStatusCode.NotFound, "Name cannot empty");
            }
            else
            {
                department.Update(Id, departments);
                return Ok("Update successfully");
            }
            //if (departments.Name == null && departments.Name == "")
            //{
            //    return Ok("Failed to Update Department");
            //}
            //department.Update(Id, departments);
            //return Ok("Department Update Successfully");

            //var put = department.Update(Id, departments);
            //if (put > 0)
            //{
            //    return Ok("Department Update Successfully");
            //}
            //return BadRequest("Failed to Update Department");
        }

        public IHttpActionResult Delete(int Id)
        {
            //var dept_id = conn.department.FirstOrDefault(x => x.Id == Id);

            //if (dept_id == null)
            //{
            //    return BadRequest("Failed to delete department");
            //}
            //else
            //{
            //    department.Delete(Id);
            //    return Ok("Deleted successfully");
            //}

            var del = department.Delete(Id);
            if (del > 0)
            {
                return Ok("Department Deleted Successfully");
            }
            return BadRequest("Failed to Delete Department");
        }
    }
}
