using Microsoft.AspNetCore.Mvc;

namespace Blog.web.Controllers
{
    public class ErrorPageController : Controller
    {
        public IActionResult Error1(int code)
        {


            return View();
        }
    }
}
