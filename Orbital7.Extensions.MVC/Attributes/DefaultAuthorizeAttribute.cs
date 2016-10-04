using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Orbital7.Extensions.Attributes
{
    public class DefaultAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var action = filterContext.ActionDescriptor;
            if (action.IsDefined(typeof(OverrideAuthorizeAttribute), true)) return;

            base.OnAuthorization(filterContext);
        }
    }
}
