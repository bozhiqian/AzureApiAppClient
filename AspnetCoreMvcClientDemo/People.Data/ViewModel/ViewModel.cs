using System;
using System.Net;

namespace People.Data.ViewModel
{
    public class ViewModel
    {
        public Exception Exception { get; set; }

        public HttpStatusCode  StatusCode { get; set; }
    }
}
