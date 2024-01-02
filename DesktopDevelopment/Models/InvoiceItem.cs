using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DesktopDevelopment.Models;

public partial class InvoiceItem
{
    [Key]
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public int ProductId { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal BasePricePerUnit { get; set; }

    public double TaxRate { get; set; }

    public double Quantity { get; set; }

    public double Discount { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("InvoiceId")]
    [InverseProperty("InvoiceItems")]
    public virtual Invoice Invoice { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("InvoiceItems")]
    public virtual Product Product { get; set; } = null!;
}
