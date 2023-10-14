using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Norboev_Asilbek_HW3.Models
{
    public class Category
    {
        public Int32 CategoryID { get; set; }

        [Display(Name = "Category Name")]
        public String CategoryName { get; set; }

        public List<JobPosting> JobPostings { get; set; }


        public Category()
        {
            JobPostings ??= new List<JobPosting>();
        }
    }
}
