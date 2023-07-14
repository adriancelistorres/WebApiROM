using Microsoft.AspNetCore.Mvc;
using WebApiRestNetCore.Models;

namespace WebApiRestNetCore.Controllers
{
    public class AperturaPDVController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerData(List<AperturaPDV> data)
        {
            // Tu lógica para procesar los datos recibidos

            return Ok(new { success = true });
        }
    }
}
