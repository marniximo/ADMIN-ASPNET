using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ADMIN_ASPNET.Models;
using Rotativa;

namespace ADMIN_ASPNET.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private AdminRecEntities db = new AdminRecEntities();

        // GET: Clientes
        public async Task<ActionResult> Index()
        {
            var clientes = db.Clientes.Include(c => c.Empresa).Include(c => c.Persona).OrderBy(c=>c.Persona.DNI);
            return View(await clientes.ToListAsync());
        }

        public async Task<ActionResult> Proyectos()
        {
            var clientes = db.Clientes.Include(c => c.Empresa).Include(c => c.Persona)
                .Where(c => c.Proyectos.Count > 0);
            return View(await clientes.ToListAsync());
        }

        public async Task<ActionResult> Ventas()
        {
            var clientes = db.Ventas
                .GroupBy(v => v.Cliente.ID)
                .Select(y => y.OrderByDescending(v => v.Fecha).FirstOrDefault())
                .Select(v => v.Cliente)
                .Include(c => c.Empresa)
                .Include(c => c.Persona);
            return View(await clientes.ToListAsync());
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            if (HttpContext.User.Identity.Name.Split(':')[0] == RolesEmpleados.EMPLEADO_VENTAS.ToString())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cliente cliente, string dniCuit, string AYNRazonSocial)
        {
            if (HttpContext.User.Identity.Name.Split(':')[0] == RolesEmpleados.EMPLEADO_VENTAS.ToString())
            {
                if (dniCuit.Length > 8)
                {
                    cliente.Empresa = new Empresa { CUIT = dniCuit, RazonSocial = AYNRazonSocial };
                }
                else
                {
                    cliente.Persona = new Persona { DNI = int.Parse(dniCuit), ApellidoYNombre = AYNRazonSocial };
                }
                db.Clientes.Add(cliente);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home", null);
        }

        // GET: Clientes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (HttpContext.User.Identity.Name.Split(':')[0] == RolesEmpleados.EMPLEADO_VENTAS.ToString())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Cliente cliente = await db.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    return HttpNotFound();
                }
                ViewBag.dniCuit = cliente.Persona != null ? cliente.Persona.DNI.ToString() : cliente.Empresa.CUIT;
                ViewBag.AYNRazonSocial = cliente.Persona != null ? cliente.Persona.ApellidoYNombre : cliente.Empresa.RazonSocial;
                return View(cliente);
            }
            return RedirectToAction("Index", "Home", null);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Cliente cliente, string dniCuit, string AYNRazonSocial)
        {
            if (HttpContext.User.Identity.Name.Split(':')[0] == RolesEmpleados.EMPLEADO_VENTAS.ToString())
            {
                if (dniCuit.Length > 8)
                {
                    var empresa = db.Empresas.Find(dniCuit);
                    empresa.RazonSocial = AYNRazonSocial;
                    db.Entry(empresa).State = EntityState.Modified;
                }
                else
                {
                    var persona = db.Personas.Find(int.Parse(dniCuit));
                    persona.ApellidoYNombre = AYNRazonSocial;
                    db.Entry(persona).State = EntityState.Modified;
                }
                db.Entry(cliente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home", null);
        }

        // GET: Clientes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (HttpContext.User.Identity.Name.Split(':')[0] == RolesEmpleados.EMPLEADO_VENTAS.ToString())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Cliente cliente = await db.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    return HttpNotFound();
                }
                return View(cliente);
            }
            return RedirectToAction("Index", "Home", null);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.User.Identity.Name.Split(':')[0] == RolesEmpleados.EMPLEADO_VENTAS.ToString())
            {
                Cliente cliente = db.Clientes
                    .Include(c => c.Persona)
                    .Include(c => c.Empresa)
                    .Include(c => c.Proyectos)
                    .Include(c => c.Proyectos.Select(p=>p.LineaVentas))
                    .Include(c => c.Ventas)
                    .Include(c => c.Facturas)
                    .First(c=> c.ID == id);
                db.Clientes.Remove(cliente);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home", null);
        }

        public ActionResult UltimaVenta(int ID) {
            var venta = db.Clientes
                .Include(c=>c.Ventas)
                .Include(c=>c.Ventas.Select(v=>v.LineaVentas))
                .Include(c=>c.Ventas.Select(v=>v.LineaVentas.Select(l=>l.Proyecto)))
                .FirstOrDefault(c => c.ID == ID)
                .Ventas
                .OrderByDescending(v => v.Fecha)
                .FirstOrDefault();
            return View(venta);
        }

        public ActionResult ListaProyectos(int ID) {
            var proyectos = db.Clientes.Find(ID).Proyectos;
            return View(proyectos);
        }

        public ActionResult PDF(int id) {
            var factura = db.Facturas.FirstOrDefault(f => f.Venta.ID == id);
            return new ViewAsPdf("Factura", factura);
        }

        public ActionResult Reporte() {
            if (HttpContext.User.Identity.Name.Split(':')[0] == RolesEmpleados.JEFE_VENTAS.ToString())
            {
                ViewBag.fechaInicio = DateTime.Today;
                ViewBag.fechaFin = DateTime.Today;
                return View(new List<Venta>());
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
        }

        public ActionResult Filtrar(string filtro)
        {
            if (string.IsNullOrEmpty(filtro)) {
                return RedirectToAction("Index");
            }
            var clientes = db.Clientes.Where(c=>
                c.Persona.DNI.ToString() == filtro ||
                c.Empresa.CUIT == filtro
            ).Include(c => c.Empresa).Include(c => c.Persona).ToList();
            return View("Index", clientes);
        }

        [HttpPost]
        public ActionResult Reporte(DateTime fechaInicio, DateTime fechaFin) {
            if (HttpContext.User.Identity.Name.Split(':')[0] == RolesEmpleados.JEFE_VENTAS.ToString())
            {
                var ventas = db.Ventas.Where(v => v.Fecha <= fechaFin && v.Fecha >= fechaInicio).ToList();
                ViewBag.fechaInicio = fechaInicio;
                ViewBag.fechaFin = fechaFin;
                return View(ventas);
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
        }

        public ActionResult ReporteVenta(DateTime fechaInicio, DateTime fechaFin)
        {
            if (HttpContext.User.Identity.Name.Split(':')[0] == RolesEmpleados.JEFE_VENTAS.ToString())
            {
                var ventas = db.Ventas.Where(v => v.Fecha <= fechaFin && v.Fecha >= fechaInicio).ToList();
                ViewBag.fechaInicio = fechaInicio;
                ViewBag.fechaFin = fechaFin;
                return new ViewAsPdf("ReporteVenta", ventas);
            }
            else
            {
                return RedirectToAction("Index", "Home", null);
            }
        }
    }
}
