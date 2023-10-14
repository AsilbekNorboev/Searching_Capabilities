using System;
using System.ComponentModel.DataAnnotations;


namespace Norboev_Asilbek_HW3.Models
{
    public enum SearchType { Greater, Less}
	public class SearchViewModel
	{
        //Title – a textbox to allow the user to search the title field
        [Display(Name = "Search by Title:")]
        [Required(ErrorMessage = "Search by Title field is required.")]
        public String Title { get; set; }

        //Description – a textbox to allow the user to search the description field
        [Display(Name = "Search by Description:")]
        [Required(ErrorMessage = "Search by Description field is required.")]
        public String Description { get; set; }

        //Category – a drop-down list that allows the user to select the desired category. The user can only select a single
        [Display(Name = "Search by Category:")]
        public Int32 SelectedCategoryID { get; set; }

        //Salary – this will be a combination of a textbox for the desired salary and two radio buttons. The radio buttons
        [Display(Name = "Search by Salary:")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number or salary.")]
        public Int32 Salary { get; set; }

        [Display(Name ="Search Type: ")]
        public SearchType TypeSearch { get; set; }

        //Posted Date – a date picker that will allow the user to select a date. The search should return any job postings that
        [Display(Name = "Search by Posted Date:")]
        [DataType(DataType.Date)]
        //DateTime?  means this date is nullable - we want to allow them to 
        //be able to NOT select a date
        public DateTime? SelectedDate { get; set; }
    }
}

