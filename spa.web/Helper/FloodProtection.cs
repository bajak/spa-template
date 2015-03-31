using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace spa.web
{
    public class FloodProtection
    {
        public ApiController Controller { get; set; }
        private readonly Dictionary<string, DateTime> _storage = new Dictionary<string,DateTime>();
        private readonly int _minutes;
        private readonly int _maxCount;

        public FloodProtection(int minutes, int maxCount = 500) 
        {
            _minutes = minutes;
            _maxCount = maxCount;
        }

        public void Check() {
            if (Controller == null)
                throw new ArgumentNullException("Controller");
            var clientIP = HttpContext.Current.Request.UserHostAddress;
            if (clientIP == null)
                throw new HttpResponseException(Controller.Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Cant not proceed request. IP Address does not exists."));
            if (_storage.ContainsKey(clientIP))
            {
                var clientDate = _storage[clientIP];
                if (clientDate.AddMinutes(_minutes) > DateTime.Now)
                    throw new HttpResponseException(Controller.Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Cant not proceed request. Flood protection enabled."));
                _storage.Remove(clientIP);
            }
            if (_storage.Count > _maxCount)
                _storage.Clear();
            _storage.Add(clientIP, DateTime.Now);
        }
    }
}