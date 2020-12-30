using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            List<SelectListItem> employers = new List<SelectListItem>();

            foreach (Employer em in context.Employers)
            {
                employers.Add(new SelectListItem(em.Name, em.Id.ToString()));
            }
            AddJobViewModel viewModel = new AddJobViewModel(employers);
            return View(viewModel);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string name = viewModel.JobName;
                int employerId = viewModel.EmployerId;

                List<Job> existingJobs = context.Jobs
                    .Where(j => j.Name == name)
                    .Where(j => j.EmployerId == employerId)
                    .ToList();

                if (existingJobs.Count == 0)
                {
                    Job job = new Job
                    {
                        Name = name,
                        EmployerId = employerId
                    };
                    context.Jobs.Add(job);
                    context.SaveChanges();
                }
                return Redirect("/Home/Detail/" + name);
            }

            List<SelectListItem> employers = new List<SelectListItem>();
            foreach (Employer em in context.Employers)
            {
                employers.Add(new SelectListItem(em.Name, em.Id.ToString()));
            }
            AddJobViewModel viewModel2 = new AddJobViewModel(employers);
            return View("AddJob", viewModel2);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
