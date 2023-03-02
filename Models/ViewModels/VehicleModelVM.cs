using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MonoTest.MVC.Models.ViewModels
{
    public class VehicleModelVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }
        public int MakeId { get; set; }
        public VehicleMake VehicleMake { get; set; }
    }
}