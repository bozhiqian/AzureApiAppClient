using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace People.Data.ViewModel
{
    public class ViewModel<T>
    {
        public Exception Exception { get; set; }

        public HttpStatusCode  StatusCode { get; set; }
    }
}
