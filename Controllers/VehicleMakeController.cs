using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MonoTest.MVC.DAL;
using MonoTest.MVC.Models;
using AutoMapper;
using MonoTest.MVC.Models.ViewModels;
using MonoTest.MVC.AutoMapper;
using static System.Net.WebRequestMethods;
using PagedList;

namespace MonoTest.MVC.Controllers
{
    public class VehicleMakeController : Controller
    {
        private VehicleContext db = new VehicleContext();
        private Mapper mapper = AutoMapperConfig.GetMapper();

        private List<VehicleMakeVM> MapVehicleMakes(List<VehicleMake> makes)
        {
            List<VehicleMakeVM> makesVM = new List<VehicleMakeVM>();
            foreach (VehicleMake make in makes)
            {
                makesVM.Add(MapVehicleMake(make));
            }
            return makesVM;
        }

        private VehicleMakeVM MapVehicleMake(VehicleMake make) {
            return mapper.Map<VehicleMakeVM>(make);
        }

        // GET: VehicleMake
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AbrvSortParm = (sortOrder == "abrv") ? "abrv_desc" : "abrv";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var makes=db.VehicleMakes.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                makes = makes.Where(s => s.Name.Contains(searchString)
                                       || s.Abrv.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    makes = makes.OrderByDescending(m => m.Name);
                    break;
                case "abrv":
                    makes = makes.OrderBy(m => m.Abrv);
                    break;
                case "abrv_desc":
                    makes = makes.OrderByDescending(m => m.Abrv);
                    break;
                default:
                    makes = makes.OrderBy(m => m.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            var vehicleMakes=MapVehicleMakes(makes.ToList()).ToPagedList(pageNumber, pageSize);

            return View(vehicleMakes);
            //return View(makes.ToPagedList(pageNumber, pageSize));
        }

        // GET: VehicleMake/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleMake vehicleMake = await db.VehicleMakes.FindAsync(id);
            if (vehicleMake == null)
            {
                return HttpNotFound();
            }

            return View(MapVehicleMake(vehicleMake));
        }

        // GET: VehicleMake/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleMake/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Abrv")] VehicleMake vehicleMake)
        {
            try 
            {
                if (ModelState.IsValid)
                {
                    db.VehicleMakes.Add(vehicleMake);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

            }catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotModified, ex.ToString());
            }

            return View(MapVehicleMake(vehicleMake));
        }

        // GET: VehicleMake/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleMake vehicleMake = await db.VehicleMakes.FindAsync(id);
            if (vehicleMake == null)
            {
                return HttpNotFound();
            }
            return View(MapVehicleMake(vehicleMake));
        }

        // POST: VehicleMake/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Abrv")] VehicleMake vehicleMake)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    db.Entry(vehicleMake).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch(Exception ex) 
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.ToString());
                }               
            }
            return View(MapVehicleMake(vehicleMake));
        }

        // GET: VehicleMake/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleMake vehicleMake = await db.VehicleMakes.FindAsync(id);
            if (vehicleMake == null)
            {
                return HttpNotFound();
            }
            return View(MapVehicleMake(vehicleMake));
        }

        // POST: VehicleMake/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                VehicleMake vehicleMake = await db.VehicleMakes.FindAsync(id);
                db.VehicleMakes.Remove(vehicleMake);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }catch(Exception ex) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, ex.ToString());
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
