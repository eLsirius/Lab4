using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;


namespace Lab4.Models
{
    public class TodoTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime Created { get; set; }
    }
    public enum Priority
    {
        Low, Medium, High
    }
}
