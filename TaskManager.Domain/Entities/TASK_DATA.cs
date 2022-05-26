using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    [Table("TASK_DATA")]
    public class TASK_DATA
    {
        [Key]
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public string TASK_DESCRIPTION { get; set; }
        public DateTime TASK_DUEDATE { get; set; }
        public DateTime? TASK_STARTDATE { get; set; }
        public DateTime? TASK_ENDDATE { get; set; }
        public string TASK_PRIORITY { get; set; }
        public string TASK_STATUS { get; set; }
    }
}
