using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Norboev_Asilbek_HW3.DAL;
using Norboev_Asilbek_HW3.Models;


namespace Norboev_Asilbek_HW3.Controllers
{
    public class HomeController : Controller
    {        // GET: /<controller>/
        private readonly AppDbContext _context;
        public HomeController(AppDbContext db)
        {
            _context = db;
        }
        public ActionResult Index(String SearchString)
        {
            var query = from jp in _context.JobPostings select jp;
            if (SearchString == null)
            {
                ViewBag.AllJobs = _context.JobPostings.Count();
                //Populate the view bag with a count of selected job postings
                List<JobPosting> SelectedJobPostings = query.Include(jp => jp.Category).ToList();
                ViewBag.SelectedJobs = SelectedJobPostings.Count();
                return View(SelectedJobPostings.OrderByDescending(jp => jp.PostedDate));
            }
            else
            {
                query = query.Where(jp => jp.Title.Contains(SearchString) || jp.Description.Contains(SearchString));
                List<JobPosting> SelectedJobPostings = query.Include(jp => jp.Category).ToList();
                ViewBag.AllJobs = _context.JobPostings.Count();
                //Populate the view bag with a count of selected job postings
                ViewBag.SelectedJobs = SelectedJobPostings.Count();


                return View(SelectedJobPostings.OrderByDescending(jp => jp.PostedDate));
            }
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error", new String[] { "JobPostingID not specified - which job posting do you want to view?" });
            }

            JobPosting jobPosting = _context.JobPostings.Include(j => j.Category).FirstOrDefault(j => j.JobPostingID == id);

            if (jobPosting == null)//Job posting does not exist in database
            {
                return View("Error", new String[] { "Job Posting not found in the database" });
            }

            return View(jobPosting);
        }

        public IActionResult DetailedSearch()
        {
            ViewBag.AllCategories = GetAllCategorySelectList();

            SearchViewModel svm = new SearchViewModel();

            svm.SelectedCategoryID = 0;
            return View(svm);
        }

        private SelectList GetAllCategorySelectList()
        {
            //Get the list of months from the database (Select *)  
            //This would be Book in HW3
            List<Category> CategoryList = _context.Categories.ToList();

            //add a dummy entry so the user can select all months
            //type of Month class; name SelectNone -- creating a fake month
            //Don't want this in the DB, but needs to be in the drop down
            //code below creates new instance of Month Class, and add new item and set two properties

            //What happens if I choose MonthID that already exists?
            Category SelectNone = new Category() { CategoryID = 0, CategoryName = "All Categories" };

            //incrementally added this to monthList 
            CategoryList.Add(SelectNone);

            SelectList CategorySelectList = new SelectList(CategoryList.OrderBy(m => m.CategoryID), "CategoryID", "CategoryName");

            //return the SelectList
            return CategorySelectList;
        }

        public ActionResult DisplaySearchResults(SearchViewModel svm)
        {
            //*************************************************************************************
            //Code for string result
            var query = from jp in _context.JobPostings select jp;

            if (svm.Title != null && svm.Title != "") //user entered something
            {
                //In this example, we are just showing the output.
                //In a real search, you would put a query here that 
                //selects records that match the name
                query = query.Where(jp => jp.Title.Contains(svm.Title));
            }
            //*************************************************************************************
            //code for searching GPA
            if (svm.Description != null && svm.Description != "")//they searched for something
            {
                query = query.Where(jp => jp.Description.Contains(svm.Description));
            }
            //Code for date
            if (svm.SelectedDate != null)//They selected a date
            {
                //In a real search, you would add a query to search by date
                ViewBag.SelectedDate = "The value you selected for date is " + svm.SelectedDate.ToString();
                query = query.Where(jp => jp.PostedDate >= svm.SelectedDate);
            }
            if (svm.Salary != null)
            {
                if (svm.TypeSearch == SearchType.Greater)
                {
                    query = query.Where(jp => jp.MinimumSalary >= svm.Salary);

                }
                else
                {
                    query = query.Where(jp => jp.MinimumSalary <= svm.Salary);

                }
            }
            if (svm.SelectedCategoryID != 0)
            {
                query = query.Where(jp => jp.Category.CategoryID == svm.SelectedCategoryID);
            }
            //go to the search view
            List<JobPosting> jobPostings = query.Include(jp => jp.Category).ToList();
            //in a 'real' search, you would execute the query here and pass the selected records to the view
            ViewBag.AllJobs = _context.JobPostings.Count();
            //Populate the view bag with a count of selected job postings
            ViewBag.SelectedJobs = jobPostings.Count();
            return View(jobPostings);
        }

    }
}

