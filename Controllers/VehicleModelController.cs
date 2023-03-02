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
using MonoTest.MVC.AutoMapper;
using MonoTest.MVC.Models.ViewModels;
using System.Data.SqlClient;
using System.Security.Cryptography;
using PagedList;

namespace MonoTest.MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private VehicleContext db = new VehicleContext();
        private Mapper mapper = AutoMapperConfig.GetMapper();

        private List<VehicleModelVM> MapVehicleModels(List<VehicleModel> models)
        {
            List<VehicleModelVM> modelsVM = new List<VehicleModelVM>();
            foreach (VehicleModel model in models)
            {
                modelsVM.Add(MapVehicleModel(model));
            }
            return modelsVM;
        }

        private VehicleModelVM MapVehicleModel(VehicleModel model)
        {
            return mapper.Map<VehicleModelVM>(model);
        }

        // GET: VehicleModel
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSortModel = sortOrder;
            ViewBag.NameSortParmModel = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AbrvSortParmModel = (sortOrder == "abrv") ? "abrv_desc" : "abrv";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilterModel = searchString;

            var models = db.VehicleModels.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                models = models.Where(m => m.Name.Contains(searchString)
                                       || m.Abrv.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    models = models.OrderByDescending(m => m.Name);
                    break;
                case "abrv":
                    models = models.OrderBy(m => m.Abrv);
                    break;
                case "abrv_desc":
                    models = models.OrderByDescending(m => m.Abrv);
                    break;
                default:
                    models = models.OrderBy(m => m.Name);
                    break;
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var vehicleModels = MapVehicleModels(models.ToList()).ToPagedList(pageNumber,pageSize);

            return View(vehicleModels);
            
        }

        // GET: VehicleModel/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = await db.VehicleModels.FindAsync(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.VehicleMakeName = (await db.VehicleMakes.FindAsync(vehicleModel.MakeId)).Name;
            return View(MapVehicleModel(vehicleModel));
        }

        // GET: VehicleModel/Create
        public ActionResult Create()
        {
            ViewBag.MakeId = new SelectList(db.VehicleMakes, "Id", "Name");
            return View();
        }

        // POST: VehicleModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Abrv,MakeId")] VehicleModel vehicleModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.VehicleModels.Add(vehicleModel);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.ToString());
            }
            

            ViewBag.MakeId = new SelectList(db.VehicleMakes, "Id", "Name", vehicleModel.MakeId);
            return View(MapVehicleModel(vehicleModel));
        }

        // GET: VehicleModel/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = await db.VehicleModels.FindAsync(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.MakeId = new SelectList(db.VehicleMakes, "Id", "Name", vehicleModel.MakeId);
            return View(MapVehicleModel(vehicleModel));
        }

        // POST: VehicleModel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Abrv,MakeId")] VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicleModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MakeId = new SelectList(db.VehicleMakes, "Id", "Name", vehicleModel.MakeId);
            return View(MapVehicleModel(vehicleModel));
        }

        // GET: VehicleModel/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = await db.VehicleModels.FindAsync(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            return View(MapVehicleModel(vehicleModel));
        }

        // POST: VehicleModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                VehicleModel vehicleModel = await db.VehicleModels.FindAsync(id);
                db.VehicleModels.Remove(vehicleModel);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, ex.ToString());
            }
            return RedirectToAction("Index");
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
