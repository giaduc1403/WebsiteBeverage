using Microsoft.AspNetCore.Mvc;
using WebsiteBeverageDemo.Extension;
using WebsiteBeverageDemo.ModelViews;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebsiteBeverageDemo.Controllers.Components
{
    public class NumberCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            return View(cart);
        }
    }
}
