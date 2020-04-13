using Client.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using OfficeOpenXml;
using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

//using Excel = Microsoft.Office.Interop.Excel;

namespace Client.Controllers
{
    public class DepartmentController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44353/api/")
        };

        // GET: Department
        public ActionResult Index()
        {
            return View(LoadDepartment());
        }

        public JsonResult LoadDepartment()
        {
            IEnumerable<Department> models = null;
            var responseTask = client.GetAsync("Department");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Department>>();
                readTask.Wait();
                models = readTask.Result;
            }
            else
            {
                models = Enumerable.Empty<Department>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            //return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = models, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InsertOrUpdate(Department department)
        {
            var myContent = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (department.Id == 0)
            {
                var result = client.PostAsync("Department", byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var result = client.PutAsync("Department/" + department.Id, byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Department/" + Id).Result;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<JsonResult> GetById(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Department");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<Department>>();
                var dept = data.FirstOrDefault(x => x.Id == Id);
                var result = JsonConvert.SerializeObject(dept, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return Json("Internal Server Error");
        }

        //public ActionResult exporttocsv(Department department)
        //{
        //    DepartmentCsv departmentCsv = new DepartmentCsv();
        //    byte[] abytes = departmentCsv.PrepareCsv();
        //    return File(abytes, "application/pdf");
        //}


        //public async Task<IActionResult> exporttocsv()
        //{
        //    var client = new HttpClient
        //    {
        //        BaseAddress = new Uri("https://localhost:44353/api/")
        //    };
        //}

        public async Task<ActionResult> Excel()
        {
            var columnHeaders = new string[]
            {
                "Nama Department",
                "Tanggal Ditambahkan"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Department Excel");
                using (var cells = worksheet.Cells[1, 1, 1, 2])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < columnHeaders.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                }

                var j = 2;
                HttpResponseMessage response = await client.GetAsync("department");
                if (response.IsSuccessStatusCode)
                {
                    var readTask = await response.Content.ReadAsAsync<IList<Department>>();
                    foreach (var department in readTask)
                    {
                        worksheet.Cells["A" + j].Value = department.Name;
                        worksheet.Cells["B" + j].Value = department.CreateDate.ToString("MM/dd/yyyy");
                        j++;
                    }
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Department_{DateTime.Now.ToString("hh:mm:ss_MM/dd/yyyy")}.xlsx");
        }

        public async Task<ActionResult> CSV()
        {
            var columnHeaders = new string[]
            {
                "Nama Department",
                "Tanggal Ditambahkan"
            };

            HttpResponseMessage response = await client.GetAsync("department");
            
                var readTask = await response.Content.ReadAsAsync<IList<Department>>();
                var departmentRecords = from department in readTask select new object[]
                {
                    $"{department.Name}",
                    $"\"{department.CreateDate.ToString("MM/dd/yyyy")}\""
                }.ToList();

                var departmentcsv = new StringBuilder();
                departmentRecords.ForEach(line =>
                {
                    departmentcsv.AppendLine(string.Join(",", line));
                });

            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeaders)}\r\n{departmentcsv.ToString()}");
            return File(buffer, "text/csv", $"Department_{DateTime.Now.ToString("hh:mm:ss_MM/dd/yyyy")}.csv");
        }

        public async Task<ActionResult> PDF()
        {
            DepartmentReport deptreport = new DepartmentReport();
            byte[] abytes = deptreport.PrepareReport(GetDepartments());
            return File(abytes, "application/pdf");
        }

        public List<Department> GetDepartments()
        {
            IEnumerable<Department> datadept = null;
            var responseTask = client.GetAsync("Department");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Department>>();
                readTask.Wait();
                datadept = readTask.Result;
            }
            else
            {
                datadept = Enumerable.Empty<Department>();
                ModelState.AddModelError(String.Empty, "Sorry Server Error, Try Again");
            }
            return datadept.ToList();
        }
    }
}