using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.Models.Models
{
    public class SellCanvas
    {
        [Key]
        public int SellCanvasId { get; set; }
        [ValidateNever]
        public DanhMuc? DanhMuc { get; set; }
        [ValidateNever]
        public SanPham? SanPham { get; set; }
        [ValidateNever]
        public int spid { get; set; }
        [ValidateNever]
        public DateTime Time { get; set; }
        [ValidateNever]
        public int Number { get; set; }
        [ValidateNever]
        public int dmid { get; set; }
    }
}
