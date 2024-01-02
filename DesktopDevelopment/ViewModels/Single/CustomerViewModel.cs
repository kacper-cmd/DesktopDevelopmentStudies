using CommunityToolkit.Mvvm.Messaging;
using DesktopDevelopment.Helpers;
using DesktopDevelopment.Helpers.Validators;
using DesktopDevelopment.Models;
using DesktopDevelopment.Models.Contexts;
using DesktopDevelopment.Models.ViewModels;
using DesktopDevelopment.ViewResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using StringValidator = DesktopDevelopment.Helpers.Validators.StringValidator;

namespace DesktopDevelopment.ViewModels.Single
{
    public class CustomerViewModel : BaseSingleViewModel<Customer>
    {
        #region FieldsAndProperties
        public List<ComboBoxVM> CustomerStatuses { get; set; }
        public List<ComboBoxVM> CustomerCategories { get; set; }
        public List<ComboBoxVM> Countries { get; set; }
        public string? Code
        {
            get => Model.Code;
            set
            {
                if (Code != value)
                {
                    Model.Code = value;
                    OnPropertyChanged(() => Code);
                }
            }
        }
        public string? Nip
        {
            get => Model.Nip;
            set
            {
                if (Nip != value)
                {
                    Model.Nip = value;
                    OnPropertyChanged(() => Nip);
                }
            }
        }
        public string? Title
        {
            get => Model.Title;
            set
            {
                if (Title != value)
                {
                    Model.Title = value;
                    OnPropertyChanged(() => Title);
                }
            }
        }
        public int CustomerStatusId
        {
            get => Model.CustomerStatusId;
            set
            {
                if (CustomerStatusId != value)
                {
                    Model.CustomerStatusId = value;
                    OnPropertyChanged(() => CustomerStatusId);
                }
            }
        }
        public int CustomerCategoryId
        {
            get => Model.CustomerCategoryId;
            set
            {
                if (CustomerCategoryId != value)
                {
                    Model.CustomerCategoryId = value;
                    OnPropertyChanged(() => CustomerCategoryId);
                }
            }
        }
        public string? Street
        {
            get => Model.Address.Street;
            set
            {
                if (Street != value)
                {
                    Model.Address.Street = value;
                    OnPropertyChanged(() => Street);
                }
            }
        }
        public string? PostalCode
        {
            get => Model.Address.PostalCode;
            set
            {
                if (PostalCode != value)
                {
                    Model.Address.PostalCode = value;
                    OnPropertyChanged(() => PostalCode);
                }
            }
        }
        public int? CountryId
        {
            get => Model.Address.CountryId;
            set
            {
                if (CountryId != value)
                {
                    Model.Address.CountryId = value;
                    OnPropertyChanged(() => CountryId);
                }
            }
        }
        public string? HouseNumber
        {
            get => Model.Address.HouseNumber;
            set
            {
                if (HouseNumber != value)
                {
                    Model.Address.HouseNumber = value;
                    OnPropertyChanged(() => HouseNumber);
                }
            }
        }
        public string? PostalCity
        {
            get => Model.Address.PostalCity;
            set
            {
                if (PostalCity != value)
                {
                    Model.Address.PostalCity = value;
                    OnPropertyChanged(() => PostalCity);
                }
            }
        }
        public string? PhoneNumber
        {
            get => Model.Address.PhoneNumber;
            set
            {
                if (PhoneNumber != value)
                {
                    Model.Address.PhoneNumber = value;
                    OnPropertyChanged(() => PhoneNumber);
                }
            }
        }
        public string? FlatNumber
        {
            get => Model.Address.FlatNumber;
            set
            {
                if (FlatNumber != value)
                {
                    Model.Address.FlatNumber = value;
                    OnPropertyChanged(() => FlatNumber);
                }
            }
        }
        public string? CountyOrRegion
        {
            get => Model.Address.CountyOrRegion;
            set
            {
                if (CountyOrRegion != value)
                {
                    Model.Address.CountyOrRegion = value;
                    OnPropertyChanged(() => CountyOrRegion);
                }
            }
        } 
        #endregion


        public CustomerViewModel() : base(GlobalResources.Customer)
        {
            //  Model = InitializeModel();
            Initialize();

        }
         

        public CustomerViewModel(int id) : base(id, GlobalResources.Customer)
        {
            Initialize();
        }
        private void Initialize()
        {
            CustomerCategories = Database.CustomerCategories.Where(item => item.IsActive).Select(item => new ComboBoxVM()
            {
                Id = item.Id,
                Title = item.Title
            }
           ).ToList();

            CustomerStatuses = Database.CustomerStatuses.Where(item => item.IsActive).Select(item => new ComboBoxVM()
            {
                Id = item.Id,
                Title = item.Title
            }
            ).ToList();

            Countries = Database.Countries.Where(item => item.IsActive).Select(item => new ComboBoxVM()
            {
                Id = item.Id,
                Title = item.Title
            }
            ).ToList();

            WeakReferenceMessenger.Default.Register<string>(this, (recipent, message) =>
            {
                Debug.WriteLine($"Message: {message}");
                Debug.WriteLine($"Recipent: {recipent}");
            });
        }
        protected override Customer InitializeModel()
        {
            return new Customer()
            {
                //select first element of collection rto have default value
              //  CustomerCategoryId = CustomerCategories.First().Id,
                Address = new()
                {
                    CreationDateTime = DateTime.Now,
                },
                IsActive = true,
                CreationDateTime = DateTime.Now
            };
        }

        protected override DbSet<Customer> GetDbTable()
        {
            return Database.Customers;
        }
        //napisz methodainicjalizujacaedycje
        protected override Customer? GetModelFromDatabase(int id)
        {
            return GetDbTable()
                .Include(item => item.Address)//ewentualnie adres
                .First(item => item.Id ==id);
        }

 

        protected override string? ValidateProperty(string propertyName)
        {
            switch(propertyName)
            {
                case nameof(Code):
                    if (!Code.IsNullOrEmpty())
                    {
                        if (!StringValidator.ContainsOnlyNumbers(Code))
                        {
                            return "Code can cointain only numbers";
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return "Code cannot be empty";
                    }
                case nameof(CustomerCategoryId):
                    if (CustomerCategoryId == 0)
                    {
                        return "Customer category is not set";
                    }
                    break;
                  
                 case nameof(CustomerStatusId):
                    if (CustomerStatusId == 0)
                    {
                        return "Customer status is not set";
                    }
                    break;

              
            }
            return null;
        }
    }
}
//ctr m+ o