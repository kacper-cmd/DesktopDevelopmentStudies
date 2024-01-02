using CommunityToolkit.Mvvm.Messaging;
using DesktopDevelopment.Helpers;
using DesktopDevelopment.Models.Contexts;
using DesktopDevelopment.Models;
using DesktopDevelopment.ViewModels.Single;
using DesktopDevelopment.ViewResources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using DesktopDevelopment.Models.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace DesktopDevelopment.ViewModels.Many
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ModelType">typ modelu z bazy danych</typeparam>
    /// <typeparam name="NewViewModel">Viewmodel sluzacy do tworzenia nowego modelu</typeparam>
    public abstract class BaseManyViewModel<ModelType, NewViewModel> : BaseDBViewModel where ModelType : class where NewViewModel : WorkspaceViewModel, new()//new() konstruktor bezargumentowy
    {
        private ObservableCollection<ModelType> _Models;
        public ObservableCollection<ModelType> Models
        {
            get => _Models;
            set
            {
                if (_Models != value)
                {
                    _Models = value;
                    OnPropertyChanged(() => Models);
                }
            }
        }
        
        private ModelType? _SelectedItem;
        public ModelType? SelectedItem
        {
            get => _SelectedItem;
            set
            {
                if (_SelectedItem != value)
                {
                    _SelectedItem = value;
                    OnPropertyChanged(() => SelectedItem);
                }
               // SelectModel();
            }
        }
        private string? _SearchInput;
        public string? SearchInput
        {
            get => _SearchInput;
            set
            {
                if (_SearchInput != value)
                {
                    _SearchInput = value;
                    OnPropertyChanged(() => SearchInput);
                }
                SelectModel();
            }
        }
        public List<GenericComboBoxVM<string>> SearchAndOrderColumns{ get; set; }
        public string? SearchColumn { get; set; }
        public string? SortColumn { get; set; }
        public bool SortDescending { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddNewCommand { get; set; }
        public ICommand SelectCommand { get; set; }

        public BaseManyViewModel(string displayName) : base(displayName)
        {
            Refresh();
            RefreshCommand = new BaseCommand(() => Refresh());
            DeleteCommand = new BaseCommand(() => Delete());
            AddNewCommand = new BaseCommand(() => AddNew());
            SelectCommand = new BaseCommand(() => SelectModel());
            WeakReferenceMessenger.Default.Register<RefreshMessage<NewViewModel>>(this, (recipent, message) => Refresh());//send viewnidek from one class to 
            SearchAndOrderColumns = GetSearchColumns();
            SearchColumn = SearchAndOrderColumns.First().Key;
            SortColumn = SearchAndOrderColumns.First().Key;
            // WeakReferenceMessenger.Default.Register<RefreshMessage<CustomerViewModel>>(this, (recipent, message) => Refresh());//send viewnidek from one class to 
        }

        public abstract IQueryable<ModelType> GetModels();
        //{

        //    return new DatabaseContext().Customers.Include(item => item.Address)
        //                                          .ThenInclude(item => item.Country)
        //                                          .Where(item => item.IsActive)
        //                                          .ToList();
        //}

        private void Refresh()//GetSearchModels
        {
            // Models = new ObservableCollection<ModelType>(GetModels());
            IQueryable<ModelType> modelTypes = GetModels();
            if (!string.IsNullOrEmpty(SearchInput)) //or !SearchInput.IsNullOrEmpty()
            {
                modelTypes = Search(modelTypes);
            }
            if (!string.IsNullOrEmpty(SortColumn))
            {
                modelTypes = Sort(modelTypes);
            }
            Models =  new ObservableCollection<ModelType>(modelTypes);
        }

        private void Delete()
        {
            if (SelectedItem != null)
            {
                //ModelType? model = Database.Set<ModelType>().FirstOrDefault(item => item == SelectedItem);
                //zeby zrocic db seta i reflection
                //ModelType? model = Database.Customers.FirstOrDefault(item => item.IsActive && item.Id == SelectedItem.Id);
                //if (model != null)
                //{
                //    model.IsActive = false;
                //    Database.SaveChanges();
                //}
                DeleteFromDatabase();
                Models.Remove(SelectedItem);
            }
        }
        /// <summary>
        /// Znajdujemy model w bazie danych usuwa go i zapisuje zmiany
        /// </summary>
        /// 
        public abstract void DeleteFromDatabase();
        private void AddNew()
        {
            //WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage(typeof(CustomerViewModel)));
            WeakReferenceMessenger.Default.Send<OpenViewMessage>(new OpenViewMessage(new NewViewModel()));
        }
        protected abstract List<GenericComboBoxVM<string>> GetSearchColumns();
        protected abstract IQueryable<ModelType> Search(IQueryable<ModelType> models);
        protected abstract IQueryable<ModelType> Sort(IQueryable<ModelType> models);
        virtual protected void OnModelSelected() { }
        public abstract void SelectModel();
    }
}
