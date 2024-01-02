using DesktopDevelopment.Models;
using DesktopDevelopment.Models.Contexts;
using DesktopDevelopment.ViewResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDevelopment.ViewModels
{
    public class AddressViewModel : WorkspaceViewModel
    {
        public AddressViewModel() : base(GlobalResources.Address)
        {
            //DatabaseTest();
        }

        public void DatabaseTest()
        {
            Debug.WriteLine("Hello World");
            DatabaseContext databaseContext = new DatabaseContext();

            Country? country;

            country = databaseContext.Countries.FirstOrDefault(country => country.Title == "Poland");

            if (country == null)
            {
                databaseContext.Countries.Add(new Country()
                {
                    Title = "Poland",
                    CreationDateTime = DateTime.Now,
                    IsActive = true
                }
                );
            }

            if(!databaseContext.Countries.Any(country => country.Title == "Egypt"))
            {
                databaseContext.Countries.Add(new Country()
                {
                    Title = "Egypt",
                    CreationDateTime = DateTime.Now,
                    IsActive = true
                }
                ) ;
            }

            databaseContext.SaveChanges();

            databaseContext = new DatabaseContext();

            foreach(Country item in databaseContext.Countries.Where(country => country.IsActive))
            {
                Debug.WriteLine($"Id: {item.Id} Country: {item.Title}");
            }

            databaseContext = new DatabaseContext();

            country = databaseContext.Countries.FirstOrDefault(country => country.Title == "Poland");

            if(country != null)
            {
                databaseContext.Countries.Remove(country);
                Debug.WriteLine("Poland deleted");
            }
            databaseContext.SaveChanges();

            databaseContext = new DatabaseContext();

            foreach (Country item in databaseContext.Countries.Where(country => country.IsActive))
            {
                Debug.WriteLine($"Id: {item.Id} Country: {item.Title}");
            }

            databaseContext = new DatabaseContext();

            Address address = new Address()
            {
                CountryId = 4,
                IsActive = false,
                CreationDateTime = DateTime.Now,
            };

            databaseContext.Addresses.Add(address);

            country = databaseContext.Countries.FirstOrDefault(country => country.Title == "Egypt");

            address = new Address()
            {
                Country = country,
                IsActive = false,
                CreationDateTime = DateTime.Now,
            };
            databaseContext.Addresses.Add(address);
            databaseContext.SaveChanges();

            databaseContext = new DatabaseContext();

            foreach(Address item in databaseContext.Addresses)
            {
                Debug.WriteLine($"id: {item.Id} CountryID {item.CountryId} Creation Datetime: {item.CreationDateTime}");
            }

            foreach (Address item in databaseContext.Addresses.Include(item => item.Country))
            {
                Debug.WriteLine($"id: {item.Id} Country {item.Country.Title} Creation Datetime: {item.CreationDateTime}");
            }

            //databaseContext.Customers.Include(item => item.Address).ThenInclude(item => item.Country);

            DbSet<Country> dbSet = databaseContext.Countries;
            IQueryable<Country> queryable = dbSet.Where(item => item.IsActive);
            IQueryable<AddressVM> selectedQueryable = queryable.Select(item => new
            AddressVM()
            {
                Title = item.Title,
                Id = item.Id
            });
            selectedQueryable = selectedQueryable.OrderBy(item => item.Title);
            List<AddressVM> list = selectedQueryable.ToList();
        }
        

    }
}
