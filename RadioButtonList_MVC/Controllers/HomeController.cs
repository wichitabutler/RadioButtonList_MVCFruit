using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace RadioButtonList_MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<SelectListItem> items = PopulateFruits();
            return View(items);
        }

        [HttpPost]
        public ActionResult Index(string fruit)
        {
            List<SelectListItem> items = PopulateFruits();
            var selectedItem = items.Find(p => p.Value == fruit);
            if (selectedItem != null)
            {
                selectedItem.Selected = true;
                ViewBag.Message = "Selected Fruit: " + selectedItem.Text;
            }

            return View(items);
        }

        private static List<SelectListItem> PopulateFruits()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT FruitName, FruitId FROM Fruits";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["FruitName"].ToString(),
                                Value = sdr["FruitId"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }
    }
}