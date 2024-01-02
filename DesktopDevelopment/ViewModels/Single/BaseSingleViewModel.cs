using CommunityToolkit.Mvvm.Messaging;
using DesktopDevelopment.Helpers;
using DesktopDevelopment.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DesktopDevelopment.ViewModels.Single
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Model Type from database (from model layer) eg Invoice</typeparam>
    public abstract class BaseSingleViewModel<T> : BaseDBViewModel, IDataErrorInfo where T : class //:BaseEntity use basemodels jak codefirst
    {
        #region FieldsAndProperties
        public T Model { get; set; }
        public ICommand SaveAndCloseCommand { get; }

        public string Error => null;
        public string this[string columnName]
        {
            get
            {
               return ValidateProperty(columnName) ?? string.Empty;
            }
        }
        #endregion
        //JedenWieleViewModel (faktura i do elementy invoice)  ZRIB TO BAZOWY BaseOneManyViewModel

        #region Constructor
        public BaseSingleViewModel(string displayName) : base(displayName)
        {
            SaveAndCloseCommand = new BaseCommand(() => SaveAndClose());//commendt w constructorze
            Model = InitializeModel();
            GetDbTable().Add(Model);

        }
        public BaseSingleViewModel(int id, string displayName) : base(displayName)
        {
            SaveAndCloseCommand = new BaseCommand(() => SaveAndClose());//stwoz private InitializeCommands() i tam wywoluj method i
            ////zamiast inicjalizwoac model to pobierasz z bazy danych zeby miec swiezy
            Model = GetModelFromDatabase(id) ?? InitializeModel();//lub wyrzuc wyjatek zamiast InitializeModel
        }
        #endregion
        #region Methods
        private void SaveAndClose()
        {
            string? error = ValidateModel();
            if (error == null)
            {
                try
                {
                    Database.SaveChanges();
                    //  WeakReferenceMessenger.Default.Send<RefreshMessage<CustomerViewModel>>();
                    WeakReferenceMessenger.Default.Send<RefreshMessage<T>>();
                    OnRequestClose();
                }
                catch (SqlException ex)
                {

                }
                catch (DbUpdateException dbex)
                {

                }
            }
            else
            {
                MessageBox.Show(error);
            }
        }
       // protected abstract string? ValidateModel();
        protected abstract T InitializeModel();
        #endregion
        protected abstract DbSet<T> GetDbTable();
        protected abstract T? GetModelFromDatabase(int id);
        protected abstract string? ValidateProperty(string propertyName);
        protected virtual string? ValidateModel()
        {
            string output = string.Empty;
            //if(CustomerCategoryId == 0) //to bylo w customer viewmodel a tu byla public abstract
            //{
            //    output += "Custom category is not set";
            //}
            //if(CustomerStatusId == 0)
            //{
            //    output += "Custom category is not set";
            //}
            //if (!Code.IsNullOrEmpty())
            //{
            //    if (StringValidator.ContainsOnlyNumbers(Code))
            //    {
            //        output += "Code can cointain only numbers";
            //    }
            //}
            //iterate all propertly of my viewmodel to automate

            //foreach (var property in GetType().GetProperties())
            //{
            //    if (property.PropertyType == typeof(string))
            //    {
            //        string? error = ValidateProperty(property.Name);
            //        if (error != null)
            //        {
            //            output += error;
            //        }
            //    }
            //}
            //output += ValidateProperty(nameof(Code));
            //output += ValidateProperty(nameof(CustomerCategoryId));
            //output += ValidateProperty(nameof(CustomerStatusId));
            //return !output.IsNullOrEmpty() ? output : null;
            //bo uzywam reflection exclude property ktore nie chce walidowac
            IEnumerable<string?> enumerable = this.GetType().GetProperties().Select(item => ValidateProperty(item.Name)).Where(item => item != null);
            return string.Join(Environment.NewLine, enumerable);//"\n"

        }

        //Weak messanger class public class User { public const string Name = "User"; } klasa z const stringami
        //ItemCollection 
    }
}
