using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechJobsPersistent.Models;


namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel
    {

        public string JobName { get; set; }

        public int EmployerId { get; set; }

        public List<SelectListItem> Employers { get; set; }


    }
}
