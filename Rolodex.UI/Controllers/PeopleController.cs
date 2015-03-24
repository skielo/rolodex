using Rolodex.Domain.Abstract;
using Rolodex.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Rolodex.UI.Controllers
{
    public class PeopleController : Controller
    {
        IPeopleRepository repo;

        public PeopleController(IPeopleRepository rep)
        {
            repo = rep;
        }

        public ViewResult Index(string start)
        {
            if (String.IsNullOrEmpty(start))
                start = "*";
            return View(this.FilterList(repo.ListContacts(),start));
        }

        [HttpGet]
        public ViewResult GetContact(int id)
        {
            return View(repo.GetContact(id));
        }

        public ViewResult MassiveImport()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            return View(repo.GetContact(id));
        }

        [HttpPost]
        public ActionResult Edit(Person model)
        {
            if (ModelState.IsValid)
            {
                repo.UpdatePerson(model);
                ViewBag.Feedback = "Contact updated!";
            }
            return View("GetContact", model);
        }

        [HttpPost]
        public ActionResult ImportRecords(HttpPostedFileBase FileUpload)
        {
            if (FileUpload.ContentLength > 0)
            {
                string fileName = Path.GetFileName(FileUpload.FileName);
                string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                try {
                    FileUpload.SaveAs(path);
                    this.ProcessFile(path);
                    ViewBag.Feedback = "Upload Complete";
                }
                catch (Exception ex)
                {
                    ViewBag.Feedback = ex.Message;
                }
            }
            return View("MassiveImport");
        }

        public ViewResult Search()
        {
            return View();
        }

        public ActionResult SearchContact(string lastName)
        {
            ViewBag.Searching = lastName;
            return View("Index",repo.SearchContacts(lastName));
        }

        private void ProcessFile(string fileName)
        {
            string line = string.Empty;
            string[] strArray;
            List<Person> retval = new List<Person>();

            Regex rex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            StreamReader sr = new StreamReader(fileName);
            while ((line = sr.ReadLine()) != null)
            {
                strArray = rex.Split(line);
                retval.Add(new Person { Name = strArray[0], LastName = strArray[1], Description = strArray[2], 
                                        Phone = strArray[3], Email = strArray[4], Url = strArray[5], Title = strArray[6]
                });
            }
            sr.Dispose();
            repo.MassiveInsert(retval);
        }

        private List<Person> FilterList(IList<Person> list, string start)
        {
            List<Person> retval = (List<Person>)list;
            if(!start.Equals("*"))
            {
                retval = (from p in list
                            where p.LastName.ToUpper().StartsWith(start.ToUpper())
                            || p.Name.ToUpper().StartsWith(start.ToUpper())
                            select p).ToList();
            }
            
            return retval;
        }
    }
}
