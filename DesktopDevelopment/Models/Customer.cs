using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DesktopDevelopment.Models;

public partial class Customer
{
    [Key]
    public int Id { get; set; }

    [StringLength(8)]
    public string? Code { get; set; }

    [Column("NIP")]
    [StringLength(10)]
    public string? Nip { get; set; }

    [StringLength(128)]
    public string Title { get; set; } = null!;

    public int CustomerStatusId { get; set; }

    public int CustomerCategoryId { get; set; }

    public int AddressId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Customers")]
    public virtual Address Address { get; set; } = null!;

    [ForeignKey("CustomerCategoryId")]
    [InverseProperty("Customers")]
    public virtual CustomerCategory CustomerCategory { get; set; } = null!;

    [ForeignKey("CustomerStatusId")]
    [InverseProperty("Customers")]
    public virtual CustomerStatus CustomerStatus { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
