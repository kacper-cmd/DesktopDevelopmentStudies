using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DesktopDevelopment.Models;

public partial class Invoice
{
    [Key]
    public int Id { get; set; }

    public int CustomerId { get; set; }

    [StringLength(16)]
    public string Number { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateOfIssue { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime PaymentDate { get; set; }

    public int PaymentMethod { get; set; }

    public bool IsPaid { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Invoices")]
    public virtual Customer Customer { get; set; } = null!;

    [InverseProperty("Invoice")]
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    [ForeignKey("PaymentMethod")]
    [InverseProperty("Invoices")]
    public virtual PaymentMethod PaymentMethodNavigation { get; set; } = null!;
}
