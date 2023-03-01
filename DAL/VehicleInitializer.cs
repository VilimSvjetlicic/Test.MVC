using MonoTest.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonoTest.MVC.DAL
{
    public class VehicleInitializer:System.Data.Entity. DropCreateDatabaseIfModelChanges<VehicleContext>
    {
        protected override void Seed(VehicleContext context)
        {
            var makes=new List<VehicleMake>() 
            {
                new VehicleMake() {Id=1,Name = "Volkswagen", Abrv="VW"},
                new VehicleMake() {Id=2,Name = "Bayerische Motoren Werke", Abrv = "BMW"},
                new VehicleMake() {Id=3,Name = "The Ford Motor Company", Abrv = "Ford"},
                new VehicleMake() {Id=4,Name = "Auto Union Deutschland Ingolstadt", Abrv = "Audi"}
            };
            makes.ForEach(m => { context.VehicleMakes.Add(m); });
            context.SaveChanges();

            var models=new List<VehicleModel>() 
            {
                new VehicleModel { Id = 1, MakeId = 1, Name = "Golf", Abrv = "Golf" },
                new VehicleModel { Id = 2, MakeId = 1, Name = "Polo", Abrv = "Polo" },
                new VehicleModel { Id = 3, MakeId = 2, Name = "3 Series", Abrv = "3 Series" },
                new VehicleModel { Id = 4, MakeId = 2, Name = "X5", Abrv = "X5" },
                new VehicleModel { Id = 5, MakeId = 3, Name = "Mustang", Abrv = "Mustang" },
                new VehicleModel { Id = 6, MakeId = 3, Name = "Fiesta", Abrv = "Fiesta" },
                new VehicleModel { Id = 7, MakeId = 4, Name = "A4", Abrv = "A4" },
                new VehicleModel { Id = 8, MakeId = 4, Name = "Q5", Abrv = "Q5" }
            };
            models.ForEach(m => { context.VehicleModels.Add(m); });
            context.SaveChanges();

        }
    }
}