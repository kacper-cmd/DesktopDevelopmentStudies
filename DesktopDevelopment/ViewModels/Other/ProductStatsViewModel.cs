using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DesktopDevelopment.BusinessLogic;
using DesktopDevelopment.Helpers;
using DesktopDevelopment.Models.ViewModels;
using DesktopDevelopment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DesktopDevelopment.ViewModels.Other
{
    public class ProductStatsViewModel : BaseDBViewModel
    {
        #region Fields&Properties 
        public List<GenericComboBoxVM<int>> Products { get; set; }
        private GenericComboBoxVM<int>? _SelectedProduct;
        public GenericComboBoxVM<int>? SelectedProduct
        {
            get => _SelectedProduct;
            set
            {
                if (_SelectedProduct != value)
                {
                    _SelectedProduct = value;
                    OnPropertyChanged(() => SelectedProduct);
                }
            }
        }
        private DateTime _StartDate;
        public DateTime StartDate
        {
            get => _StartDate;
            set
            {
                if (_StartDate != value)
                {
                    _StartDate = value;
                    OnPropertyChanged(() => StartDate);
                }
            }
        }
        private DateTime _EndDate;
        public DateTime EndDate
        {
            get => _EndDate;
            set
            {
                if (_EndDate != value)
                {
                    _EndDate = value;
                    OnPropertyChanged(() => EndDate);
                }
            }
        }
        private decimal _TotalBasePrice;
        public decimal TotalBasePrice
        {
            get => _TotalBasePrice;
            set
            {
                if (_TotalBasePrice != value)
                {
                    _TotalBasePrice = value;
                    OnPropertyChanged(() => TotalBasePrice);
                }
            }
        }
        private decimal _TotalTaxAmount;
        public decimal TotalTaxAmount
        {
            get => _TotalTaxAmount;
            set
            {
                if (_TotalTaxAmount != value)
                {
                    _TotalTaxAmount = value;
                    OnPropertyChanged(() => TotalTaxAmount);
                }
            }
        }
        private decimal _TotalTaxedPrice;
        public decimal TotalTaxedPrice
        {
            get => _TotalTaxedPrice;
            set
            {
                if (_TotalTaxedPrice != value)
                {
                    _TotalTaxedPrice = value;
                    OnPropertyChanged(() => TotalTaxedPrice);
                }
            }
        }
        public ICommand CalculateCommand { get; set; } //ctl k s surround
        #endregion

        #region Constructors
        public ProductStatsViewModel() : base("Product Stats")
        {
            CalculateCommand = new BaseCommand(() => Calculate());
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Products = Database.Products.Where(item => item.IsActive).
                Select(item => new GenericComboBoxVM<int>(item.Title, item.Id)).ToList();

        } 
        #endregion
        #region Methods
        private void Calculate()
        {
            if (SelectedProduct != null )
            {
                ProductStatsBM productStatsBM = new();
                //TotalBasePrice = productStatsBM.CalculateTotalBasePrice(SelectedProduct?.Key ?? 1, StartDate, EndDate);
                //TotalTaxAmount = productStatsBM.CalculateTotalTaxAmount(SelectedProduct?.Key ?? 1, StartDate, EndDate);
                //TotalTaxedPrice = TotalBasePrice + TotalTaxAmount;
                decimal totalBasePrice = 0, totalTaxAmount =0 , totalTaxedPrice = 0;
                productStatsBM.CalculateAllStats(SelectedProduct?.Key ?? 1, StartDate, EndDate, ref totalBasePrice, ref totalTaxAmount, ref totalTaxedPrice);//or use out
                TotalTaxedPrice = totalTaxedPrice;
                TotalTaxAmount = totalTaxAmount;
                TotalBasePrice = totalBasePrice;//params ref out
            }
        }
        #endregion
    }
}



//messanger w maui i jak viewmodele wiele 
//funcja kalkulujaca
//xapytaj sie w constructorze base z parametrem 

//zmiana ilosci na magazynie

//koszyk -> trans
//desktop - jak klikne na row to ponize rozwiniete szzcegoly

//jak zlecenie i elementy zlecenie to zeby produkt utworzony mial infora

//dynamiczne dodawanie menu maui

//<usercontrol/Datacontext> tp na poczatku viewmodel ten wpf z comentami edit delete i te commedni zbindowac w datagrid template column
//relativesource wpf datacontext



//do zakladek methode wywolujaca 
//w zakladnce user control  wnetrzne
//relaycommand zamiasta command
//behiden code rzytiwac na viewmodel 
//itemscontrol binding colecji i tam baton bindablelayout
//<ItemsControl ItemsSource>
//<ItemControl.ItemsPanel>
//>ItemsPanelTemplete>
//<UniformGifd columns="2>
//<itemsCIntril.ItemTemplete>
//<DateTemolete>
//<StackPanel>
//<TexBox Text=Binding FIrsName
//UniformGRid rows  -> stackpanel
//https://learn.microsoft.com/pl-pl/dotnet/desktop/winforms/controls/how-to-add-and-remove-tabs-with-the-windows-forms-tabcontrol?view=netframeworkdesktop-4.8
//tabs cotrol w 1
//            SearchCommand = new RelayCommand(async () => await OnSearchCommodityCommand());




//WAZNE
//public class MainWindowViewModel : BaseViewModel
//{
//    private UserControl _currentView;
//    public UserControl CurrentView
//    {
//        get => _currentView;
//        set
//        {
//            if (_currentView != value)
//            {
//                _currentView = value;
//                OnPropertyChanged(() => CurrentView);
//            }
//        }
//    }
//    public ICommand  OpenHomeViewCommand { get; }
//    public ICommand OpenLikedSongsViewCommand { get; }
//    public MainWindowViewModel()
//    {
//        CurrentView = new HomeView(new HomeViewModel());//view to strona
//        OpenHomeViewCommand = new BaseCommand(() => SetHomeView(new HomeViewModel()));
//        OpenLikedSongsViewCommand = new BaseCommand(() => SetLikedSongsView(new LikedSongsViewModel()));
//        WeakReferenceMessenger.Default.Register<OpenViewMessage>(this, (recipient, message) => OpenView(message));
//    }
//    public void OpenView(OpenViewMessage message)
//    {
//        if(message.WorkspaceViewModel.GetType() == typeof(HomeViewModel))
//        {
//            SetHomeView(message.WorkspaceViewModel);
//        }
//        else if (message.WorkspaceViewModel.GetType() == typeof(LikedSongsViewModel))
//        {
//            SetLikedSongsView(message.WorkspaceViewModel);
//        }
//        //if (message != null)
//        //{
//        //    if (message.Type == typeof(HomeViewModel))
//        //    {
//        //        SetHomeView(new HomeViewModel());
//        //    }
//        //    else if (message.Type == typeof(LikedSongsViewModel))
//        //    {
//        //        SetLikedSongsView(new LikedSongsViewModel());
//        //    }
//        //}
//    }
//    private void SetHomeView(WorkspaceViewModel workspaceViewModel)
//    {
//        CurrentView = new HomeView(workspaceViewModel);
//    }
//    private void SetLikedSongsView(WorkspaceViewModel workspaceViewModel)
//    {
//        CurrentView = new LikedSongsView(workspaceViewModel);
//    }
//    //SideMenuView
//}

//jak register to send
//inny viewmodel
//    public ICommand OpenSOcCommand { get; set; }
//construktiir 
//        OpenSOcCommand = new BaseCommand(() => OpenSOc());
//    private void OpenSOc()
//{
//        WeakReferenceMessenger.Default.Send(new OpenViewMessage(new SOcViewModel()));
//    }

//public ovveride void Edit(){
//WeakReferenceMessenger.Default.Send(new EdiitorMessenger<Pilot>(1, SelectedItem));//EdiitorMessenger to moja lklasa dla messengera do wysylki