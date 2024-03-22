using Microsoft.AspNetCore.Mvc;

namespace SimpleBlogSystem.Controllers
{
    public class WebSocketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
