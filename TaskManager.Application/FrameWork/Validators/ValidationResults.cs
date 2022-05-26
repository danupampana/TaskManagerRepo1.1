using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.FrameWork.Validators
{
    public record ValidationResults
    {

        public bool IsSuccessful { get; set; } = true;
        public ICollection<string> messages { get; init; }

        public static ValidationResults Success()
        {
            return new ValidationResults();
        }

        public static ValidationResults Fail(ICollection<string> messages)
        {
            return new ValidationResults { IsSuccessful = false, messages = messages };
        }
    }
}
