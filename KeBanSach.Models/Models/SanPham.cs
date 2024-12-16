using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeBanSach.Models
{
    public class SanPham
    {
        [Key]
        public int SanPhamId { get; set; }
        [Required]
        [Display(Name="Ten San Pham")]
        public string Name { get; set; }
        [Display(Name = "Tac Gia")]
        public string Author { get; set; }
        [ValidateNever]
        [Display(Name="Danh Muc")]
        public int DanhMucId { get; set; }
        [ValidateNever]
        [ForeignKey("DanhMucId")]
        public DanhMuc DanhMuc {  get; set; }
        [Display(Name="Anh San Pham")]
        [ValidateNever]
        public string AnhSanPham { get; set; }
        [Range(1, 100)]
        [Display(Name="Gia San Pham")]
        public int Price { get; set; }
        [ValidateNever]
        [Display(Name="Mo Ta")]
        public string Description { get; set; }
    }
}
