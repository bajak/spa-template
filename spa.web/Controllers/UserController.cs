using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using spa.model;
using WebMatrix.WebData;

namespace spa.web.Controllers
{
    public class UserController : ApiController
    {
        private IUow _uow { get; set; }

        public UserController(IUow uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [AllowAnonymous]
        public void Login([FromBody] dynamic form)
        {
            if (WebSecurity.IsAuthenticated)
                return;
            var username = form["username"].Value;
            var password = form["password"].Value;
            if (!WebSecurity.UserExists(username))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized, 1));
            }
            if (!WebSecurity.Login(username, password))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized, 2));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public void Create([FromBody] dynamic form)
        {
            if (WebSecurity.IsAuthenticated)
                return;
            var username = form["username"].Value;
            var password = form["password"].Value;
            if (WebSecurity.UserExists(username))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized, 3));
            }
            var user = WebSecurity.CreateUserAndAccount(username, password, new { LastActivity = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)});
            WebSecurity.Login(username, password);
        }

        [HttpGet]
        [AllowAnonymous]
        public void Check()
        {
            if (WebSecurity.IsAuthenticated)
                return;
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized, 4));
        }

        [HttpPost]
        [Authorize]
        public void Logout()
        {
            WebSecurity.Logout();
        }
    }
}

