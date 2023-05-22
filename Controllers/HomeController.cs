using MCF_FE_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MCF_FE_MVC.Controllers
{
    public class HomeController : Controller
    {
        string BaseURI = "http://localhost:5001/";

        public async Task<IActionResult> Index()
        {
            List<MCFWebAPI.Models.BPKB> datas = new List<MCFWebAPI.Models.BPKB>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseURI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = await client.GetAsync("mcf/bpkb/list");
                    if (Res.IsSuccessStatusCode)
                    {
                        var Result = Res.Content.ReadAsStringAsync().Result;
                        datas = JsonConvert.DeserializeObject<List<MCFWebAPI.Models.BPKB>>(Result);
                    }

                }
            }
            catch (Exception ex)
            {               
            }

            return View(datas);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetBPKB(string agreement_no)
        //{
        //    MCFWebAPI.Models.BPKB _bpkb= new MCFWebAPI.Models.BPKB();
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(BaseURI);
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage Res = await client.GetAsync("mcf/bpkb/" + agreement_no);
        //        if (Res.IsSuccessStatusCode)
        //        {
        //            var Result = Res.Content.ReadAsStringAsync().Result;
        //            _bpkb = JsonConvert.DeserializeObject<MCFWebAPI.Models.BPKB>(Result);

        //        }
        //    }
        //    return View(_bpkb);

        //}
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<MCFWebAPI.Models.Location> locs = new List<MCFWebAPI.Models.Location>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage ResLoc = await client.GetAsync("mcf/location/list");
                if (ResLoc.IsSuccessStatusCode)
                {
                    var Result = ResLoc.Content.ReadAsStringAsync().Result;
                    locs = JsonConvert.DeserializeObject<List<MCFWebAPI.Models.Location>>(Result);
                    ViewBag.Locations = locs.Select(i => new SelectListItem
                    {
                        Value = i.locationId,
                        Text = i.locationName
                    }).ToList();
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MCFWebAPI.Models.BPKB obj)
        {

            MCFWebAPI.Models.Location locs = new MCFWebAPI.Models.Location();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage searchLoc = await client.GetAsync("mcf/location/"+obj.LocationId);
                if (searchLoc.IsSuccessStatusCode)
                {
                    var _loc = searchLoc.Content.ReadAsStringAsync().Result;
                    locs = JsonConvert.DeserializeObject<MCFWebAPI.Models.Location>(_loc);
                    obj.Location=locs;
                    HttpResponseMessage Res = client.PostAsJsonAsync(BaseURI + "mcf/bpkb/add", obj).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        ViewBag.msg = "New Data Submitted";
                        ModelState.Clear();
                    }
                    else ViewBag.msg = "Submit Data Failed";
                }



            }
            return View();

        }
      
        public IActionResult Privacy()
        {
            return View();
        }

    }
}