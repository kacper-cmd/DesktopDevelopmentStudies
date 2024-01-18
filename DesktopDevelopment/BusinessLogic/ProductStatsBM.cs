using DesktopDevelopment.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDevelopment.BusinessLogic
{
    public class ProductStatsBM
    {
        //create baseBussinesslogic class with database context
        public DatabaseContext Database { get; set; }
        public ProductStatsBM()
        {
            this.Database = new DatabaseContext();
        }
        //iquarable not use include bo buduje zapytanie nie musze includowac Invoice
        public decimal CalculateTotalBasePrice(int productId, DateTime startDate, DateTime endDate)
        {
            //alt enter i wrapping i 2 opcja
             return  Database.InvoiceItems.Where(item => item.IsActive
                                                && item.ProductId == productId
                                                && item.Invoice.DateOfIssue >= startDate
                                                && item.Invoice.DateOfIssue <= endDate).
                                                Sum(item => item.BasePricePerUnit * (decimal)item.Quantity);

        }
        public decimal CalculateTotalTaxAmount(int productId, DateTime startDate, DateTime endDate)
        {
            return Database.InvoiceItems.Where(item => item.IsActive
                                           && item.ProductId == productId
                                                 && item.Invoice.DateOfIssue >= startDate
                                                 && item.Invoice.DateOfIssue <= endDate).
                                                Sum(item => item.BasePricePerUnit * (decimal)item.Quantity * ((decimal)item.TaxRate/100));
        }
        public void CalculateAllStats(int productId, DateTime startDate, DateTime endDate, ref decimal totalBasePrice, ref decimal totalTaxAmount, ref decimal totalTaxedPrice)
        {
            totalBasePrice = CalculateTotalBasePrice(productId, startDate, endDate);
            totalTaxAmount = CalculateTotalTaxAmount(productId, startDate, endDate);
            totalTaxedPrice = totalBasePrice + totalTaxAmount;
        }

    }
}
