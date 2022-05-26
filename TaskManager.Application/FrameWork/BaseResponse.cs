using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.FrameWork
{
    public class BaseResponse
    {
        public HttpStatusCode responseCode { get; set; } = HttpStatusCode.OK;
        public ICollection<string> responseMessages { get; set; } = new List<string>();

        public void Success(IEnumerable<string> messages = null)
        {
            responseCode = HttpStatusCode.OK;

            AddResponseMessage(messages);
        }

        public void Failure(string message, HttpStatusCode statusCode)
        {
            responseCode = statusCode;

            List<string> responseMessages = this.responseMessages.ToList() ?? new List<string>();
            responseMessages.Add(message);

            this.responseMessages = responseMessages;
        }

        public void Failure(IEnumerable<string> messages, HttpStatusCode statusCode)
        {
            responseCode = statusCode;
            AddResponseMessage(messages);
        }

        private void AddResponseMessage(IEnumerable<string> messages)
        {
            List<string> responseMessages = this.responseMessages.ToList() ?? new List<string>();
            responseMessages.AddRange(messages ?? new List<string>());

            this.responseMessages = responseMessages;
        }
    }
}
