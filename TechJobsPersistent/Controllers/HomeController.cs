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
            List<Skill> skills = new List<Skill>();

            foreach (Employer em in context.Employers)
            {
                employers.Add(new SelectListItem(em.Name, em.Id.ToString()));
            }
            foreach (Skill skill in context.Skills)
            {
                skills.Add(skill);
            }

            AddJobViewModel viewModel = new AddJobViewModel(employers, skills);
            return View(viewModel);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel viewModel, String[] selectedSkills)
        {
            List<SelectListItem> employers = new List<SelectListItem>();
            List<Skill> skillsList = new List<Skill>();
            foreach (Employer em in context.Employers)
            {
                employers.Add(new SelectListItem(em.Name, em.Id.ToString()));
            }
            foreach (Skill skill in context.Skills)
            {
                skillsList.Add(skill);
            }

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

                    foreach (String skill in selectedSkills)
                    {
                        JobSkill jobSkill = new JobSkill
                        {
                            JobId = employerId,
                            Job = job,
                            SkillId = int.Parse(skill),
                            Skill = context.Skills.Find(int.Parse(skill))
                        };
                        context.JobSkills.Add(jobSkill);
                    }

                    context.SaveChanges();
                }
                return Redirect("Index");
            }
            

            

            AddJobViewModel viewModel2 = new AddJobViewModel(employers, skillsList);
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
