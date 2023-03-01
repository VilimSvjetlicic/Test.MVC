using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MonoTest.MVC.Models
{
    public class VehicleModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }

        [ForeignKey("VehicleMake")]
        public int MakeId { get; set; }
        public VehicleMake VehicleMake { get; set; }
    }
}