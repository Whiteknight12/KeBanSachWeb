using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models;
using KeBanSach.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace KeBanSach.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISanPham _sanpham;
        private IDanhMuc _danhmuc;
        private SignInManager<IdentityUser> _signinManager;
        private IDaMua _damua;
        private IDanhGia _danhGia;
        private UserManager<IdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, 
            ISanPham sanPham, 
            IDanhMuc danhMuc, 
            SignInManager<IdentityUser> signInManager,
            IDaMua daMua,
            IDanhGia danhGia,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _sanpham = sanPham;
            _danhmuc = danhMuc;
            _signinManager = signInManager;
            _damua = daMua;
            _danhGia = danhGia;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_sanpham.GetAll().ToList());
        }
        [HttpPost]
        public IActionResult Index(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var listsanpham=_sanpham.GetList(u=>u.Name.Contains(searchString)).ToList();
                TempData["searchsuccess"] = $"Hiển thị kết quả tìm kiếm cho {searchString}";
                return View(listsanpham);
            }
            return View(_sanpham.GetAll().ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Detail(int? sanphamid)
        {
            SanPham obj = _sanpham.Get(u => u.SanPhamId == sanphamid);
            obj.DanhMuc = _danhmuc.Get(u => u.DanhMucId == obj.DanhMucId);
            bool HaveBought = false;
            if (_signinManager.IsSignedIn(User))
            {
                var user=await _userManager.GetUserAsync(User);
                var damualist = _damua.GetList(u => u.UserId == user.Id);
                foreach (var entity in damualist)
                {
                    if (entity.SanPhamId == sanphamid) HaveBought = true;
                }
                var true_user=(CustomedUser)user;
                ViewBag.UserAvatar = true_user.UserImgUrl;
                ViewBag.UserId=true_user.Id;
            }
            ViewBag.HaveBought= HaveBought;
            Dictionary<CustomedUser, DanhGia> Comments = new Dictionary<CustomedUser, DanhGia>();
            var listdanhgia=_danhGia.GetList(u=>u.SanPhamId==sanphamid);
            foreach (var danhgia in listdanhgia)
            {
                var user = await _userManager.FindByIdAsync(danhgia.UserId);
                var true_user = (CustomedUser)user;
                Comments[true_user] = danhgia;
            }
            ViewBag.Comments=Comments;
            return View(obj);
        }
        [HttpPost]
        public IActionResult Detail(int SanPhamId, string UserId, string DanhGia)
        {
            DanhGia danhgia=new DanhGia();
            danhgia.UserId=UserId;
            danhgia.SanPhamId=SanPhamId;
            danhgia.NoiDung = DanhGia;
            _danhGia.Add(danhgia);
            _danhGia.Save();
            TempData[("success")] = "Đăng bình luận thành công!";
            return RedirectToAction("Detail", new { sanphamid = SanPhamId });
        }
        public IActionResult All()
        {
            return View(_sanpham.GetAll().ToList());
        }
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Comment(int SanPhamId, ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);
            DanhGia danhgia= new DanhGia();
            danhgia.SanPhamId = SanPhamId;
            danhgia.UserId = user.Id;
            _danhGia.Add(danhgia);
            _danhGia.Save();
            return RedirectToAction("Detail", SanPhamId);
        }
        public IActionResult CategoryFilter(int DanhMucId)
        {
            var listsanpham=_sanpham.GetList(u=>u.DanhMucId == DanhMucId);
            var danhmuc=_danhmuc.Get(u=>u.DanhMucId==DanhMucId);
            ViewBag.danhmuc = danhmuc;  
            return View(listsanpham);
        }
    }
}
