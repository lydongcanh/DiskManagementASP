using Ehr.Data;
using Ehr.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Ehr.APIController
{
    [AllowAnonymous]
    public class AccountController : ApiController
    {
        private EhrDbContext db = new EhrDbContext();

        [Authentication.BasicAuthentication]
        public HttpResponseMessage  Login(LoginViewModel loginViewModel)
        {
            User user = db.Users.Where(c => c.Username == loginViewModel.Username && c.Password == loginViewModel.Password).FirstOrDefault();
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, user, Configuration.Formatters.JsonFormatter);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, user, Configuration.Formatters.JsonFormatter); 
            }
        }

    }
}
