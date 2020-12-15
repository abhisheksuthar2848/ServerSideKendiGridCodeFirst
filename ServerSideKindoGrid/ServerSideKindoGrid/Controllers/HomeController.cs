using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServerSideKindoGrid.Models;
using System.Collections;



namespace ServerSideKindoGrid.Controllers
{
    public class HomeController : Controller
    {

        public readonly Context _context = new Context();

        public ActionResult Index()
        {
            return View();
        }      


        [HttpPost]
        public JsonResult GetHistoricalAndForecastData()
        {
            //JsonResult result = new JsonResult();
            try
            {
                string pageNo = Request.Params.GetValues("page").FirstOrDefault();
              
                int pageOffSet = (Convert.ToInt32(pageNo) - 1) * 10;
                string pageSize = Request.Params.GetValues("pagesize").FirstOrDefault();

                string sortColumn = Request.Params.GetValues("sort[0][field]") == null ? "" : Request.Params.GetValues("sort[0][field]").FirstOrDefault();
               string sortOrder = Request.Params.GetValues("sort[0][dir]").FirstOrDefault();
                string searchFilter = Request.Params.GetValues("filter[filters][2][value]") == null ? "" : Request.Params.GetValues("filter[filters][2][value]").FirstOrDefault();

                var data = _context.Employees.ToList();

                int totalRecords = data.Count;
                if (!string.IsNullOrEmpty(searchFilter) &&
                    !string.IsNullOrWhiteSpace(searchFilter))
                {
                    data = data.Where(p => p.EmpId.ToString().ToLower().Contains(searchFilter.ToLower()) ||
                        p.FirstName.ToString().Contains(searchFilter.ToLower()) ||
                        p.LastName.ToString().Contains(searchFilter.ToLower()) ||
                        p.Address.ToString().Contains(searchFilter.ToLower()) 
                      
                     ).ToList();
                }

                data = SortTableData(sortColumn, sortOrder, data); 

                int recFilter = data.Count;
                data = data.Skip(pageOffSet).Take(Convert.ToInt32(pageSize)).ToList();
                var modifiedData = data.Select(d =>
                    new
                    {
                        d.EmpId,
                        d.FirstName,
                        d.LastName,
                        d.Address
                    }
                    );


                return Json(new
                {
                    total = totalRecords,
                    data = modifiedData
                }, JsonRequestBehavior.AllowGet);

               
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    total = 0,
                    data = ""
                }, JsonRequestBehavior.AllowGet);
            };
        }

        private List<Employee> SortTableData(string order, string orderDir, List<Employee> data)
        {
            List<Employee> lst = new List<Employee>();
            try
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Address).ToList()
                                                                                                 : data.OrderBy(p => p.Address).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FirstName).ToList()
                                                                                                 : data.OrderBy(p => p.FirstName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastName).ToList()
                                                                                                 : data.OrderBy(p => p.LastName).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmpId).ToList()
                                                                                                 : data.OrderBy(p => p.EmpId).ToList();
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return lst;
        }
    }
 
}



