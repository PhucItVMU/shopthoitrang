﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BanHangThoiTrangMVC.Models.EF
{
    [Table("tb_ProductCategory")]
    public class ProductCategory: CommonAbstract
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [Required]
        [StringLength(150)]
        public string? Alias { get; set; }
        public string? Description { get; set; }
        [StringLength(250)]
        public List<string>? Icon { get; set; }
        [StringLength(250)]
        public string? SeoTitle { get; set; }
        [StringLength(500)]
        public string? SeoDescription { get; set; }
        [StringLength(250)]
        public string? SeoKeywords { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}