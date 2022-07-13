using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResultsShow.Models;

namespace ResultsShow.Controllers
{
    public class ShowController : Controller
    {
        private NavajoShowDataContext _Show = new NavajoShowDataContext();

        // GET: Show
        public ActionResult Index()
        {
            //ViewBag.ElectionDate = _context.tblWebConfigs.Where(o => o.ConfigSetting == "ElectionDate").FirstOrDefault().ConfigValue;
            ViewBag.ElectionDate = "October 24th, 2017";

            // Vote counts
            ViewBag.YesTotal = _Show.tblVotes.Where(o => o.OfficeId == 1).Sum(s => s.VotesYes);
            ViewBag.NoTotal = _Show.tblVotes.Where(o => o.OfficeId == 1).Sum(s => s.VotesNo);
            ViewBag.BallotTotal = ViewBag.YesTotal + ViewBag.NoTotal;
            ViewBag.BallotsCast = _Show.tblVotes.Where(o => o.OfficeId == 1).Sum(s => s.BallotsCast);

            // Counts by Agency
            ViewBag.ChinleYes = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 1).Sum(s => s.VotesYes);
            ViewBag.ChinleNo = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 1).Sum(s => s.VotesNo);
            ViewBag.EasternYes = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 2).Sum(s => s.VotesYes);
            ViewBag.EasternNo = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 2).Sum(s => s.VotesNo);
            ViewBag.FortDefianceYes = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 3).Sum(s => s.VotesYes);
            ViewBag.FortDefianceNo = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 3).Sum(s => s.VotesNo);
            ViewBag.NorthernYes = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 4).Sum(s => s.VotesYes);
            ViewBag.NorthernNo = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 4).Sum(s => s.VotesNo);
            ViewBag.WesternYes = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 5).Sum(s => s.VotesYes);
            ViewBag.WesternNo = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 5).Sum(s => s.VotesNo);

            // Vote percentages
            if (ViewBag.BallotTotal != 0)
            {
                ViewBag.YesPercent = Math.Truncate((float)ViewBag.YesTotal / (float)ViewBag.BallotTotal * 10000)/100;
                ViewBag.NoPercent = Math.Truncate((float)ViewBag.NoTotal / (float)ViewBag.BallotTotal * 10000)/100;
            }
            else
            {
                ViewBag.YesPercent = "00.00";
                ViewBag.NoPercent = "00.00";
            }

            // Format Totals
            ViewBag.YesTotal = String.Format("{0:n0}", ViewBag.YesTotal);
            ViewBag.NoTotal = String.Format("{0:n0}", ViewBag.NoTotal);
            ViewBag.YesPercent = String.Format("{0:n}", ViewBag.YesPercent);
            ViewBag.NoPercent = String.Format("{0:n}", ViewBag.NoPercent);

            // Chapter counts
            ViewBag.Chinle = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 1 && o.BallotsCast > 0).Count();
            ViewBag.ChinleT = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 1).Count();
            ViewBag.ChinleP = Math.Truncate((float)ViewBag.Chinle / (float)ViewBag.ChinleT * 10000) / 100;
            ViewBag.Eastern = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 2 && o.BallotsCast > 0).Count();
            ViewBag.EasternT = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 2).Count();
            ViewBag.EasternP = Math.Truncate((float)ViewBag.Eastern / (float)ViewBag.EasternT * 10000) / 100;
            ViewBag.FortDefiance = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 3 && o.BallotsCast > 0).Count();
            ViewBag.FortDefianceT = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 3).Count();
            ViewBag.FortDefianceP = Math.Truncate((float)ViewBag.FortDefiance / (float)ViewBag.FortDefianceT * 10000) / 100;
            ViewBag.Northern = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 4 && o.BallotsCast > 0).Count();
            ViewBag.NorthernT = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 4).Count();
            ViewBag.NorthernP = Math.Truncate((float)ViewBag.Northern / (float)ViewBag.NorthernT * 10000) / 100;
            ViewBag.Western = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 5 && o.BallotsCast > 0).Count();
            ViewBag.WesternT = _Show.vShowVotes.Where(o => o.OfficeId == 1 && o.AgencyId == 5).Count();
            ViewBag.WesternP = Math.Truncate((float)ViewBag.Western / (float)ViewBag.WesternT * 10000) / 100;

            return View();
        }

        public ActionResult Votes(int? chapter)
        {
            if (Session["UserID"] == null) return RedirectToAction("Login", "Home");

            if (chapter == null) chapter = 1;

            ViewBag.Chapter = chapter;

            IQueryable<vShowVote> votesQuery = from v in _Show.vShowVotes
                                               where v.ChapterId == chapter
                                               select v;

            var votes = votesQuery.ToList();

            return View(votes);
        }

        public bool SaveVotes(int chapterId, int officeId, int ballots, int vyes, int vno)
        {
            bool result = false;

            var votes = _Show.tblVotes
                        .Where(o => o.ChapterId == chapterId && o.OfficeId == officeId)
                        .SingleOrDefault();

            votes.BallotsCast = ballots;
            votes.VotesYes = vyes;
            votes.VotesNo = vno;

            try
            {
                _Show.SubmitChanges();
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}