using ADMIN_ASPNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ADMIN_ASPNET.Controllers
{
    public class LoginController : Controller
    {
        private AdminRecEntities _context = new AdminRecEntities();
        // GET: Login
        public ActionResult Login()
        {
            ViewBag.Mensaje = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(Empleado user)
        {
            if (ModelState.IsValid)
            {
                var IsValidUser = false;
                var loginUser = _context.Empleadoes.FirstOrDefault(u => u.Legajo == user.Legajo);
                if (loginUser != null && user.Contraseña == loginUser.Contraseña)
                {
                    FormsAuthentication.SetAuthCookie($"{(RolesEmpleados)loginUser.Rol}:{loginUser.Legajo}", false);
                    FormsAuthentication.RedirectFromLoginPage($"{(RolesEmpleados)loginUser.Rol}:{loginUser.Legajo}", false);
                }
            }
            ModelState.AddModelError("InvalidLogin", "Contraseña o Usuario invalido");
            return View();
        }

        public ActionResult Salir()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}