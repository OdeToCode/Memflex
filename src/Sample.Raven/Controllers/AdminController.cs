using System.Web.Mvc;
using LogMeIn.Raven.Filters;

namespace LogMeIn.Raven.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        [FlexAuthorize(Roles="admin")]
        public ActionResult Index()
        {
            return Content("You can see me!");
        }

    }
}
