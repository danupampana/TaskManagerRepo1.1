using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.Models
{
    public class TaskStatusList
    {
        public const string New = "New";
        public const string InProgress = "In Progress";
        public const string Finished = "Finished";
    }

    public class TaskPriorityList
    {
        public const string High = "High";
        public const string Middle = "Middle";
        public const string Low = "Low";
    }
}
