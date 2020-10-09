using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MvcApplication1.Infrastructure
{

    interface ICustomPrincipal : IPrincipal
    {
        string accessToken { get; set; }
        int id { get; set; }
        string email { get; set; }
    }
    public class CustomPrincipal : ICustomPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public CustomPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email);
        }

        public string accessToken { get; set; }
        public int id { get; set; }
        public string email { get; set; }
    }

    [Serializable]
    public class CustomPrincipalSerializeModel
    {
        public string accessToken { get; set; }
        public int id { get; set; }
        public string email { get; set; }
    }


}