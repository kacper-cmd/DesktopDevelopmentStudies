using CommunityToolkit.Mvvm.Messaging;
using DesktopDevelopment.Helpers;
using DesktopDevelopment.ViewModels.Many;
using DesktopDevelopment.ViewModels.Single;
using DesktopDevelopment.ViewResources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace DesktopDevelopment.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region TopAndSideMenuCommand

        public ICommand OpenAddressViewCommand { get ; }
        public ICommand OpenCustomerViewCommand { get ; }
        public ICommand OpenCustomersViewCommand { get ; }
        public ICommand OpenInvoiceViewCommand { get ; }

        #endregion


        public MainWindowViewModel()
        {
            Workspaces = new();
            Workspaces.CollectionChanged += OnWorkspacesChanged;
            OpenAddressViewCommand = new BaseCommand(() => CreateView(new AddressViewModel()));
            OpenCustomerViewCommand = new BaseCommand(() => CreateView(new CustomerViewModel()));
            OpenCustomersViewCommand = new BaseCommand(() => CreateView(new CustomersViewModel()));
            OpenInvoiceViewCommand = new BaseCommand(() => CreateView(new InvoiceViewModel()));
            Commands = new(CreateCommands());
            //pass messages between classes
            WeakReferenceMessenger.Default.Register<OpenViewMessage>(this, (recipent,message) =>
            {
               if(message !=null)
                {
                    //if (message.Type == typeof(CustomerViewModel))
                    //{
                    //    CreateView(new CustomerViewModel());
                    //}
                    CreateView(message.WorkspaceViewModel);

                }
            });
        }
        //on watch wpisz tam Model.CustomerId = 1
        #region Buttons in side bar

        public ReadOnlyCollection<CommandViewModel> Commands { get; set; }

        private List<CommandViewModel> CreateCommands()
        {
            return new()
            {
                new (GlobalResources.Address, OpenAddressViewCommand),
                new (GlobalResources.Customer, OpenCustomerViewCommand),
                new (GlobalResources.Customers, OpenCustomersViewCommand),
                new (GlobalResources.Invoice, OpenInvoiceViewCommand)
            };
        }

        #endregion

        #region Tabs

        public ObservableCollection<WorkspaceViewModel> Workspaces { get; set; }
        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += onWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= onWorkspaceRequestClose;
        }

        private void onWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel? workspace = sender as WorkspaceViewModel;
            if (workspace != null)
            {
                Workspaces.Remove(workspace);
            }
        }

        #endregion

        #region Helepers

        private void CreateView(WorkspaceViewModel workspace)
        {
            Workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        private void CreateListView<WorkspaceViewModelType>() where WorkspaceViewModelType : WorkspaceViewModel, new()
        {
            WorkspaceViewModel? workspace = Workspaces.FirstOrDefault(vm => vm is WorkspaceViewModelType);
            if (workspace == null)
            {
                workspace = new WorkspaceViewModelType();
                Workspaces.Add(workspace);
            }
            SetActiveWorkspace(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        #endregion
    }
}
