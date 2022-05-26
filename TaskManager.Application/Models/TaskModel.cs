using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage="{0} is required")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime? DueDate { get; set; } = null!;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string Priority { get; set; } = null!;

        [Required(ErrorMessage = "{0} is required")]
        public string Status { get; set; } = null!;
    }
}
