using CommunityToolkit.Mvvm.Messaging;
using DesktopDevelopment.Helpers;
using DesktopDevelopment.Models;
using DesktopDevelopment.Models.Contexts;
using DesktopDevelopment.Models.ViewModels;
using DesktopDevelopment.ViewModels.Single;
using DesktopDevelopment.ViewResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopDevelopment.ViewModels.Many
{
    public class CustomersViewModel : BaseManyViewModel<Customer,CustomersViewModel>
    {
        //private ObservableCollection<Customer> _Models;
        //public ObservableCollection<Customer> Models 
        //{
        //    get => _Models;
        //    set
        //    {
        //        if (_Models != value)
        //        {
        //            _Models = value;
        //            OnPropertyChanged(() => Models);
        //        }
        //    }
        //}

        //private Customer? _SelectedItem;
        //public Customer? SelectedItem 
        //{
        //    get => _SelectedItem;
        //    set
        //    {
        //        if (_SelectedItem != value)
        //        {
        //            _SelectedItem = value;
        //            OnPropertyChanged(() => SelectedItem);
        //        }
        //    }
        //}
        //public ICommand RefreshCommand { get; set; }
        //public ICommand DeleteCommand { get; set; }
        //public ICommand AddNewCommand { get; set; }
        public CustomersViewModel() : base(GlobalResources.Customers)
        {
            //Refresh();
            //RefreshCommand = new BaseCommand(() => Refresh());
            //DeleteCommand = new BaseCommand(() => DeleteFromDatabase());
            //AddNewCommand = new BaseCommand(() => AddNew());
            //WeakReferenceMessenger.Default.Register<RefreshMessage<CustomerViewModel>>(this, (recipent, message) => Refresh());//send viewnidek from one class to 
        }

        public override IQueryable<Customer> GetModels()
        {
            return new DatabaseContext().Customers.Include(item => item.Address)
                                                  .ThenInclude(item => item.Country)
                                                  .Where(item => item.IsActive);
        }

        //private void Refresh()
        //{
        //    Models = new ObservableCollection<Customer>(GetModels());

        //}

        public override void DeleteFromDatabase()
        {
            if(SelectedItem != null)
            {
                Customer? model = Database.Customers.FirstOrDefault(item => item.IsActive && item.Id == SelectedItem.Id);
                if(model != null)
                {
                    model.IsActive = false;
                    Database.SaveChanges();
                }
               // Models.Remove(SelectedItem);
            }
        }
        //zaimplementuj dodatkowewyszykiwanie virtualna bazowa//na kazdy viewmodel nowy context bazy danych
        public override void SelectModel()
        {
            if (SelectedItem != null)
            {
                WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage(new CustomerViewModel(SelectedItem.Id)));

            }
        }

        protected override List<GenericComboBoxVM<string>> GetSearchColumns()
        {
            return new()
           {
               new("Code",nameof(Customer.Code)),//Code
               new("Title",nameof(Customer.Title)),//use resouce file
               
           };
           
        }

        protected override IQueryable<Customer> Search(IQueryable<Customer> models)
        {
            switch (SearchColumn)
            {
                case nameof(Customer.Code):
                  return models = models.Where(item => item.Code.Contains(SearchInput));
                case nameof(Customer.Title):
                  return models = models.Where(item => item.Title.Contains(SearchInput));
                default:
                    return models;
                  
            }
        }

        protected override IQueryable<Customer> Sort(IQueryable<Customer> models)
        {
            switch (SortColumn)
            {
                case nameof(Customer.Code):
                   return SortDescending ? models.OrderByDescending(item => item.Code) : models.OrderBy(item => item.Code);
                case nameof(Customer.Title):
                    return SortDescending ? models.OrderByDescending(item => item.Title) : models.OrderBy(item => item.Title);
                default:
                    return models;
            }
        }
        //private void AddNew()
        //{
        //    //WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage(typeof(CustomerViewModel)));
        //    WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage(new CustomerViewModel()));
        //}
    }
}

