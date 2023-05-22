using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MCF_FE_MVC.Models
{
    public class Location
    {
        [Key]
        public string? locationId { get; set; }
        public string? locationName { get; set; }

    }
}
