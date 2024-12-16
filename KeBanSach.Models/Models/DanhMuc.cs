using System.ComponentModel.DataAnnotations;

namespace KeBanSach.Models
{
    public class DanhMuc
    {
        [Key]
        public int DanhMucId { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name="Ten Danh Muc")]
        public string Name { get; set; }
    }
}
