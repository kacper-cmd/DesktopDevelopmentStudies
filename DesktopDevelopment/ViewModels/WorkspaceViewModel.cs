﻿using DesktopDevelopment.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopDevelopment.ViewModels
{
    // Each tab view model will extends this class
    public class WorkspaceViewModel:BaseViewModel
    {
        #region FieldsAndProperties
        //each tab has display name and close command
        public string DisplayName { get; set; } //this name of the tab
        private BaseCommand _CloseCommand; //this is command to close a tab
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                    _CloseCommand = new BaseCommand(() => this.OnRequestClose()); //this command calls method to close a tab
                return _CloseCommand;
            }
        }
        #endregion

        #region Constructors

        public WorkspaceViewModel(string displayName)
        {
            DisplayName = displayName;
        }

        #endregion

        #region RequestClose [event]
        public event EventHandler RequestClose;
        protected void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion 

    }
}

//private void OnWorkspaceRequestClose(object sender, EventArgs e)
//{
//    WorkspaceViewModel workspace = sender as WorkspaceViewModel;
//if (workspace != null)
//{
//    this.Workspaces.Remove(workspace);
//}
//}
