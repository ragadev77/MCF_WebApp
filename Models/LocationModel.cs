using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCF_FE_MVC.Models
{
    public class LocationModel
    {
        public string locationId { get; set; }

        public List<SelectListItem>? LocationList { get; set; }
    }
}