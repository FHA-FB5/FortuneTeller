using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sorting_Algorithm.Models;


namespace Sorting_Algorithm.Controllers
{
    public class DataOutputController : Controller
    {
        // GET: DataOutput
        public ActionResult DataOutputView(DataOutputModel model)
        {
            
            return View(model);
        }

   
    }
}