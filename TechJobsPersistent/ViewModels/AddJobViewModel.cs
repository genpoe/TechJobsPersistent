using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TechJobsPersistent.Models;


namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string JobName { get; set; }

        [Required(ErrorMessage = "Employer is required")]
        public int EmployerId { get; set; }
        public List<SelectListItem> Employers { get; set; }

        public List<Skill> Skills { get; set; }

        public AddJobViewModel()
        { }

        public AddJobViewModel(List<SelectListItem> employers, List<Skill> skills)
        {
            Employers = employers;
            Skills = skills;
        }
    }
}
