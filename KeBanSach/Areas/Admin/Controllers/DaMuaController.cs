using KeBanSach.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeBanSach.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class DaMuaController: Controller
    {
        private ISellCanvas _sellcanvas;
        public DaMuaController(ISellCanvas daMua)
        {
            _sellcanvas = daMua;
        }
        public IActionResult History()
        {
            return View(_sellcanvas.GetAll().OrderByDescending(u=>u.Time));
        }
    }
}
