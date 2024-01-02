using CommunityToolkit.Mvvm.Messaging;
using DesktopDevelopment.Helpers;
using DesktopDevelopment.Models;
using DesktopDevelopment.Models.ViewModels;
using DesktopDevelopment.ViewModels.Many;
using DesktopDevelopment.Views.Many;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DesktopDevelopment.ViewModels.Single
{
    public class InvoiceViewModel : BaseSingleViewModel<Invoice>
    {
        #region Fields&Properties
        public List<ComboBoxVM> Products { get; set; }
        public int InvoiceItemProductId
        {
            get => SelectedInvoiceItem.ProductId;
            set
            {
                if (InvoiceItemProductId != value)
                {
                    SelectedInvoiceItem.ProductId = value;
                    OnPropertyChanged(() => InvoiceItemProductId);
                }
            }
        }
        public decimal BasePricePerUnit
        {
            get => SelectedInvoiceItem.BasePricePerUnit;
            set
            {
                if (BasePricePerUnit != value)
                {
                    SelectedInvoiceItem.BasePricePerUnit = value;
                    OnPropertyChanged(() => BasePricePerUnit);
                }
            }
        }
        public double TaxRate
        {
            get => SelectedInvoiceItem.TaxRate;
            set
            {
                if (TaxRate != value)
                {
                    SelectedInvoiceItem.TaxRate = value;
                    OnPropertyChanged(() => TaxRate);
                }
            }
        }
        public double Quantity
        {
            get => SelectedInvoiceItem.Quantity;
            set
            {
                if (Quantity != value)
                {
                    SelectedInvoiceItem.Quantity = value;
                    OnPropertyChanged(() => Quantity);
                }
            }
        }
        public double Discount
        {
            get => SelectedInvoiceItem.Discount;
            set
            {
                if (Discount != value)
                {
                    SelectedInvoiceItem.Discount = value;
                    OnPropertyChanged(() => Discount);
                }
            }
        }

        public int PaymentMethod //PaymentMethodId
        {
            get => Model.PaymentMethod;
            set
            {
                if (PaymentMethod != value)
                {
                    Model.PaymentMethod = value;
                    OnPropertyChanged(() => PaymentMethod);
                }
            }
        }
        public bool IsPaid
        {
            get => Model.IsPaid;
            set
            {
                if (IsPaid != value)
                {
                    Model.IsPaid = value;
                    OnPropertyChanged(() => IsPaid);
                }
            }
        }
        public string Number
        {
            get => Model.Number;
            set
            {
                if (Number != value)
                {
                    Model.Number = value;
                    OnPropertyChanged(() => Number);
                }
            }
        }
        public DateTime DateOfIssue
        {
            get => Model.DateOfIssue;
            set
            {
                if (DateOfIssue != value)
                {
                    Model.DateOfIssue = value;
                    OnPropertyChanged(() => DateOfIssue);
                }
            }
        }
        public DateTime PaymentDate
        {
            get => Model.PaymentDate;
            set
            {
                if (PaymentDate != value)
                {
                    Model.PaymentDate = value;
                    OnPropertyChanged(() => PaymentDate);
                }
            }
        }
        public ICommand SelectCustomerCommand { get; set; }
        public ICommand AddNewInvoiceItemCommand { get; set; }
        public ICommand DeleteInvoiceItemCommand { get; set; }
        private string _SelectedCustomerData;
        public string SelectedCustomerData
        {
            get => _SelectedCustomerData;
            set
            {
                if (_SelectedCustomerData != value)
                {
                    _SelectedCustomerData = value;
                    OnPropertyChanged(() => SelectedCustomerData);
                }
            }
        }

        private ObservableCollection<InvoiceItem> _InvoiceItems;//powinien byc vm
        public ObservableCollection<InvoiceItem> InvoiceItems
        {
            get => _InvoiceItems;
            set
            {
                if (_InvoiceItems != value)
                {
                    _InvoiceItems = value;
                    OnPropertyChanged(() => InvoiceItems);
                }
            }
        }
        private InvoiceItem _SelectedInvoiceItem;
        public InvoiceItem SelectedInvoiceItem        //clasa bazowa napisz BaseSingleMany zeby nie popwarzac
        {
            get => _SelectedInvoiceItem;
            set
            {
                if (_SelectedInvoiceItem != value)
                {
                    _SelectedInvoiceItem = value;
                    OnPropertyChanged(() => SelectedInvoiceItem);
                }
            }
        }

        public List<ComboBoxVM> PaymentMethods { get; set; }
        #endregion

        public InvoiceViewModel() : base("Invoice")//parameterless bo uzywam w viedoku
        {
            PaymentMethods = Database.PaymentMethods.Where(x => x.IsActive).Select(x => new ComboBoxVM()
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();
            _SelectedCustomerData = "No customer selected";//use resources
            SelectCustomerCommand = new BaseCommand(() => SelectCustomer());
            AddNewInvoiceItemCommand = new BaseCommand(() => AddInvoiceItem());
            DeleteInvoiceItemCommand = new BaseCommand(() => DeleteInvoiceItem());
            WeakReferenceMessenger.Default.Register<SelectModelMessage<Customer>>(this, (recipent, message) => ReceiveCustomer(message));//nasluchuje wiadowmosc z selectCustomerviewmodel//message, recepient
            //this tu chce odebrac wiadomosc
            _InvoiceItems = new ObservableCollection<InvoiceItem>();
            _SelectedInvoiceItem = new InvoiceItem();
            Products = Database.Products.Where(x => x.IsActive).Select(x => new ComboBoxVM()
            {
                Id = x.Id,
                Title = x.Title
            }).ToList();
            //Database.Products.Add(new Product()
            //{
            //    Title = "test",
            //    IsActive = true,
            //    BasePricePerUnit = 10,
            //    ProfitMargin = 10,
            //    SalesTaxRate = 10,
            //    PurchaseTaxRate = 10,
            //    CreationDateTime = DateTime.Now
            //});
            //Database.SaveChanges();
        }
        private void ReceiveCustomer(SelectModelMessage<Customer> message)//, object recipent
        {
            if (this == message.Recipient)//w jednym oknie zostanie wybrany klient
            {
                Model.CustomerId = message.Message.Id; //model to tu faktura
                SelectedCustomerData = $"{message.Message.Title}";
            }

        }
        protected override DbSet<Invoice> GetDbTable() => Database.Invoices;

        private void SelectCustomer()
        {
            Window window = new Window();
            window.Title = "Select customer";
            window.Content = new CustomersView();
            SelectCustomersViewModel selectCustomersViewModel = new Many.SelectCustomersViewModel(this);
            window.DataContext = selectCustomersViewModel;//     CustomersViewModel       window.Show(); independent window
            window.Owner = App.Current.MainWindow;
            //selectCustomersViewModel.RequestClose += SelectCustomersViewModel_RequestClose;//podpinam wydarzenie
            selectCustomersViewModel.RequestClose += delegate (object? sender, EventArgs eventArgs)
            {
                //Debug.WriteLine("test");
                window.Close();  //selectCustomersViewModel.RequestClose += (sender, eventArgs) => window.Close();//2 SPOSOB
                //Debug.WriteLine("Closing");
            };
            window.ShowDialog();//window.DialogResult poczytaj

        }
        private void AddInvoiceItem()
        {
            InvoiceItem invoiceItem = new InvoiceItem()
            {
                ProductId = InvoiceItemProductId,
                BasePricePerUnit = BasePricePerUnit,//form invoice item on right side from vm
                TaxRate = TaxRate,
                Quantity = Quantity,
                Discount = Discount,
                IsActive = true,
                CreationDateTime = DateTime.Now
            };
            Model.InvoiceItems.Add(invoiceItem);//add to db
            InvoiceItems.Add(invoiceItem);//display items on view observable collection itemsource on datagrid local collection
        }
        private void DeleteInvoiceItem()
        {
            Model.InvoiceItems.Remove(SelectedInvoiceItem);
            InvoiceItems.Remove(SelectedInvoiceItem);
        }

        //private void SelectCustomersViewModel_RequestClose(object? sender, EventArgs e)
        //{
        //    throw new NotImplementedException();//1 SPOSOB    
        //}

        protected override Invoice InitializeModel()
        {
            return new Invoice()
            {
                DateOfIssue = DateTime.Now,
                CreationDateTime = DateTime.Now,
                IsActive = true,
                PaymentDate = DateTime.Now.AddDays(14),
                IsPaid = false,

            };
        }

        protected override Invoice? GetModelFromDatabase(int id)
        {
            throw new NotImplementedException();
        }

        protected override string? ValidateProperty(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
//ctrl m+o