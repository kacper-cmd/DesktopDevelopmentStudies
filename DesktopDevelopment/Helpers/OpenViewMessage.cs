using DesktopDevelopment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDevelopment.Helpers
{
    public class OpenViewMessage
    {
        public WorkspaceViewModel WorkspaceViewModel { get; set; }
        //bypopublic Type Type { get; set; }
        public OpenViewMessage(WorkspaceViewModel workspaceViewModel)
        {
            WorkspaceViewModel = workspaceViewModel;
        }
    }
}
//one message with generic < kidofmesage, messageitself>