using Client.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using OfficeOpenXml;
using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class DevisiController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44353/api/")
        };

        // GET: Devisi
        public ActionResult Index()
        {
            return View(LoadDevisi());
        }

        public JsonResult LoadDevisi()
        {
            IEnumerable<DevisiVM> models = null;
            var responseTask = client.GetAsync("Devisi");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<DevisiVM>>();
                readTask.Wait();
                models = readTask.Result;
            }
            else
            {
                models = Enumerable.Empty<DevisiVM>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            //return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = models, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InsertOrUpdate(Devisi devisi)
        {
            var myContent = JsonConvert.SerializeObject(devisi);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (devisi.Id == 0)
            {
                var result = client.PostAsync("Devisi", byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var result = client.PutAsync("Devisi/" + devisi.Id, byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Devisi/" + Id).Result;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<JsonResult> GetById(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Devisi");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<DevisiVM>>();
                var dev = data.FirstOrDefault(x => x.Id == Id);
                var result = JsonConvert.SerializeObject(dev, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return Json("Internal Server Error");
        }

        public async Task<ActionResult> Excel()
        {
            var columnHeaders = new string[]
            {
                "Nama Devisi",
                "Nama Department",
                "Tanggal Ditambahkan"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Devisi Excel");
                using (var cells = worksheet.Cells[1, 1, 1, 3])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < columnHeaders.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                }

                var j = 2;
                HttpResponseMessage response = await client.GetAsync("devisi");
                if (response.IsSuccessStatusCode)
                {
                    var readTask = await response.Content.ReadAsAsync<IList<DevisiVM>>();
                    foreach (var devisi in readTask)
                    {
                        worksheet.Cells["A" + j].Value = devisi.Name;
                        worksheet.Cells["B" + j].Value = devisi.DepartmentName;
                        worksheet.Cells["C" + j].Value = devisi.CreateDate.ToString("MM/dd/yyyy");
                        j++;
                    }
                }   
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Devisi_{DateTime.Now.ToString("hh:mm:ss_MM/dd/yyyy")}.xlsx");
        }

        public async Task<ActionResult> CSV()
        {
            var columnHeaders = new string[]
            {
                "Nama Devisi",
                "Nama Department",
                "Tanggal Ditambahkan"
            };

            HttpResponseMessage response = await client.GetAsync("devisi");

            var readTask = await response.Content.ReadAsAsync<IList<DevisiVM>>();
            var devisiRecords = from devisi in readTask select new object[]
            {
                    $"{devisi.Name}",
                    $"\"{devisi.DepartmentName}\"",
                    $"\"{devisi.CreateDate.ToString("MM/dd/yyyy")}\""
            }.ToList();

            var devisicsv = new StringBuilder();
            devisiRecords.ForEach(line =>
            {
                devisicsv.AppendLine(string.Join(",", line));
            });

            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeaders)}\r\n{devisicsv.ToString()}");
            return File(buffer, "text/csv", $"Devisi_{DateTime.Now.ToString("hh:mm:ss_MM/dd/yyyy")}.csv");
        }

        public async Task<ActionResult> PDF()
        {
            DevisiReport devreport = new DevisiReport();
            byte[] abytes = devreport.PrepareReport(GetDevisi());
            return File(abytes, "application/pdf");
        }

        public List<DevisiVM> GetDevisi()
        {
            IEnumerable<DevisiVM> datadev = null;
            var responseTask = client.GetAsync("Devisi");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<DevisiVM>>();
                readTask.Wait();
                datadev = readTask.Result;
            }
            else
            {
                datadev = Enumerable.Empty<DevisiVM>();
                ModelState.AddModelError(String.Empty, "Sorry Server Error, Try Again");
            }
            return datadev.ToList();
        }
    }
}