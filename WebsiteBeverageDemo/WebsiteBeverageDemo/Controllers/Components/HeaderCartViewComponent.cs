using Microsoft.AspNetCore.Mvc;
using WebsiteBeverageDemo.Extension;
using WebsiteBeverageDemo.ModelViews;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace WebsiteBeverageDemo.Controllers.Components
{
    public class HeaderCartViewComponent : ViewComponent

    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            return View(cart);
        }
    }
}
