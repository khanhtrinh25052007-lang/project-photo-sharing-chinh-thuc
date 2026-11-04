using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace project_photo_sharing.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa phiên đăng nhập
            return RedirectToAction("Index", "Home"); // Trả về trang chủ (hoặc đổi thành "Login" nếu muốn)
        }
    }
}