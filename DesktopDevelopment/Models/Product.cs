using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DesktopDevelopment.Models;

public partial class Product
{
    [Key]
    public int Id { get; set; }

    [StringLength(128)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal BasePricePerUnit { get; set; }

    public double SalesTaxRate { get; set; }

    public double PurchaseTaxRate { get; set; }

    public double ProfitMargin { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}
