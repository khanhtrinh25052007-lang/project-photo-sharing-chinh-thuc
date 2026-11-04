using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoSocialDB.Controllers
{
    public class PhotosController : Controller
    {
        private PhotoSocialDB db =
            new PhotoSocialDB();
        // GET: Photos
        public ActionResult Index()
        {
            return View();
        }
        // POST: Photos/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(
            HttpPostedFileBase image,
            string caption)
        {
            // Kiểm tra người dùng đã đăng nhập
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Kiểm tra file ảnh
            if (image == null || image.ContentLength == 0)
            {
                ViewBag.Error = "Vui lòng chọn ảnh!";
                return View();
            }

            // Chỉ cho phép ảnh
            string extension =
                Path.GetExtension(image.FileName).ToLower();

            string[] allowedExtensions =
            {
                ".jpg", ".jpeg", ".png", ".gif"
            };

            if (!allowedExtensions.Contains(extension))
            {
                ViewBag.Error = "File không đúng định dạng ảnh!";
                return View();
            }

            // Tạo tên file mới
            string fileName = Guid.NewGuid().ToString()
                              + extension;

            // Đường dẫn lưu ảnh
            string folderPath = Server.MapPath(
                "~/Content/Uploads/");

            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Lưu file ảnh
            string fullPath = Path.Combine(
                folderPath, fileName);

            image.SaveAs(fullPath);

            // Lưu đường dẫn vào database
            int userId = Convert.ToInt32(
                Session["UserId"]);

            Photo photo = new Photo();

            photo.UserId = userId;
            photo.ImagePath = "/Content/Uploads/"
                            + fileName;
            photo.Caption = caption;
            photo.CreatedAt = DateTime.Now;

            db.Photos.Add(photo);
            db.SaveChanges();

            return RedirectToAction("Index", "Store");
        }
    }
}
