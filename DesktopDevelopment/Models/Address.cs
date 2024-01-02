using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DesktopDevelopment.Models;

public partial class Address
{
    [Key]
    public int Id { get; set; }

    [StringLength(128)]
    public string? Street { get; set; }

    [StringLength(16)]
    public string? HouseNumber { get; set; }

    [StringLength(16)]
    public string? FlatNumber { get; set; }

    [StringLength(32)]
    public string? PostalCode { get; set; }

    [StringLength(128)]
    public string? PostalCity { get; set; }

    [StringLength(128)]
    public string? CountyOrRegion { get; set; }

    public int? CountryId { get; set; }

    [StringLength(16)]
    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditDateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteDateTime { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("Addresses")]
    public virtual Country? Country { get; set; }

    [InverseProperty("Address")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
