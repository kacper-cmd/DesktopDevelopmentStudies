using CommunityToolkit.Mvvm.Messaging;
using DesktopDevelopment.Helpers;
using DesktopDevelopment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDevelopment.ViewModels.Many
{
    public class SelectCustomersViewModel : CustomersViewModel
    {
        public object Recipient { get; set; }
        public SelectCustomersViewModel(object recipient) : base()
        {
            Recipient = recipient;//wiemy kto chcial zebysmy mu wyslali
        }
        public override void SelectModel()
        {
            if (SelectedItem != null)
            {
                WeakReferenceMessenger.Default.Send(new SelectModelMessage<Customer>(Recipient, SelectedItem));
                //w ktory obiekcie ma byc odebrana i co chcemy wyslac
                OnRequestClose();//opali wszystkie metody ktore sa podpiete pod requestclose i zamknie okno
            }
        }
        //or ovveride properties
    }
}
