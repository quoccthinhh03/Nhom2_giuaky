using Nhom2_giuaky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nhom2_giuaky.Controllers
{
    public class trangchuController : Controller
    {
        
        nhanviensqlDataContext db = new nhanviensqlDataContext();

        public ActionResult Index()
        {
            if (Session["MaKH"] == null)
            {
                return RedirectToAction("dangnhap", "trangchu");
            }

            var complaints = db.Complaints.OrderByDescending(c => c.complaintID).Take(5).ToList();
            ViewBag.Complaints = complaints;
            return View();
        }

        [HttpPost]
        public ActionResult Comment(string comment)
        {
            if (Session["MaKH"] != null)
            {
             
                var complaint = new Complaint
                {
                    employeeID = Session["MaKH"].ToString(),
                    noidungcomplaint = comment
                };
                db.Complaints.InsertOnSubmit(complaint);
                db.SubmitChanges();
            }
            return RedirectToAction("Index", "trangchu");
        }
        [HttpGet]
        public ActionResult dangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult dangnhap(string employeeID, string password)
        {
            if (!string.IsNullOrEmpty(employeeID) && !string.IsNullOrEmpty(password))
            {
                var user = db.Employees.SingleOrDefault(u => u.employeeID == employeeID && u.password == password);
                if (user != null)
                {
                 
                    Session["TaiKhoan"] = user;
                    Session["MaKH"] = user.employeeID;
                    return RedirectToAction("Index", "trangchu");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin đăng nhập.");
            }
            
            return View();
        }


        [HttpGet]
        public ActionResult dangky()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult dangky(Employee model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                 
                    db.Employees.InsertOnSubmit(model);
                    db.SubmitChanges();
                    return RedirectToAction("dangnhap", "trangchu");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi. Vui lòng thử lại sau.");
                }
            }

            return View(model);
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("dangnhap", "trangchu");
        }

    }
}