using Final.Models;
using Final.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace Final.Controllers
{
    [Authorize]
    public class CandidatesController : Controller
    {
        private readonly CandidateDbContext db = new CandidateDbContext();
        // GET: Candidates
        [AllowAnonymous]
        public async Task<ActionResult> Index(int pg = 1)
        {
            var data = await db.Candidates.OrderBy(a => a.CandidateId).ToPagedListAsync(pg, 5);
            return View(data);
        }
        public ActionResult Create()
        {

            CandidateViewModel c = new CandidateViewModel();
            c.Qualifications.Add(new Qualification { });
            return View(c);
        }
        [HttpPost]
        public ActionResult Create(CandidateViewModel data, string act = "")
        {
            if (act == "add")
            {
                data.Qualifications.Add(new Qualification { });

                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Qualifications.RemoveAt(index);
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                }
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    var b = new Candidate
                    {
                        CandidateName = data.CandidateName,
                        BirthDate = data.BirthDate,
                        AppliedFor = (Models.AppliedFor)data.AppliedFor,
                        ExpectedSalary = data.ExpectedSalary,
                        Conditions = data.Conditions,
                    };
                    string ext = Path.GetExtension(data.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Server.MapPath("~/Pictures/") + fileName;
                    data.Picture.SaveAs(savePath);
                    b.Picture = fileName;
                    foreach (var l in data.Qualifications)
                    {

                        b.Qualifications.Add(l);
                    }
                    db.Candidates.Add(b);
                    db.SaveChanges();
                }
            }
            ViewBag.Act = act;
            return PartialView("_CreatePartial", data);
        }
        public ActionResult Edit(int id)
        {
            var c = db.Candidates
                .Select(x => new CandidateEditModel
                {
                    CandidateId = x.CandidateId,
                    CandidateName = x.CandidateName,
                    AppliedFor = (ViewModels.AppliedFor)x.AppliedFor,
                    ExpectedSalary = x.ExpectedSalary,
                    Conditions = x.Conditions,
                    Qualifications = x.Qualifications.ToList()

                })
                  .FirstOrDefault(x => x.CandidateId == id);
            ViewData["CurrentPic"] = db.Candidates.First(x => x.CandidateId == id).Picture;
            return View(c);

        }
        [HttpPost]
        public ActionResult Edit(CandidateEditModel data, string act = "")
        {
            if (act == "add")
            {
                data.Qualifications.Add(new Qualification { });
            }

            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Qualifications.RemoveAt(index);
            }

            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var c = db.Candidates.First(x => x.CandidateId == data.CandidateId);
                    c.CandidateName = data.CandidateName;
                    c.AppliedFor = (Models.AppliedFor)data.AppliedFor;
                    c.ExpectedSalary = data.ExpectedSalary;
                    c.Conditions = data.Conditions;
                    if (data.Picture != null)
                    {
                        string ext = Path.GetExtension(data.Picture.FileName);
                        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Server.MapPath("~/Pictures/") + fileName;
                        data.Picture.SaveAs(savePath);
                        c.Picture = fileName;
                    }
                    db.Qualifications.RemoveRange(db.Qualifications.Where(x => x.CandidateId == data.CandidateId).ToList());
                    foreach (var item in data.Qualifications)
                    {
                        c.Qualifications.Add(new Qualification
                        {
                            CandidateId = data.CandidateId,
                            Degree = item.Degree,
                            Institute = item.Institute,
                            PassingYear = item.PassingYear,
                            Result = item.Result,

                        });
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            ViewData["CurrentPic"] = db.Candidates.First(x => x.CandidateId == data.CandidateId).Picture;
            return View(data);

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var candidate = new Candidate { CandidateId = id };
            db.Entry(candidate).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Json(new { success = true, deleted = id });
        }
    }
}
    
