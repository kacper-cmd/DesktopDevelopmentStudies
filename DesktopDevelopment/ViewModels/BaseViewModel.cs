using DesktopDevelopment.Views.Single;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls.Primitives;

namespace DesktopDevelopment.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        #region Propertychanged
        protected void OnPropertyChanged<T>(Expression<Func<T>> action)
        {
            var propertyName = GetPropertyName(action);
            OnPropertyChanged(propertyName);
        }

        private static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            var propertyName = expression.Member.Name;
            return propertyName;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)

            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
//WAZNE ZAKLADKI DO HURT
//stworz custom control do widoku bazowego
//https://wpf-tutorial.com/misc-controls/the-expander-control/ expander to rozwijane menu
// utworz usercontrol View->single-> TabsView.xaml
//< TabControl >
//< TabItem Header = "Zakladka 1" />
// < local:InvoiceView /> WYWSoluje rozne widokoi
//</ TabItem >
//< TabItem Header = "Zakladak1" >
//< local:CustomerView />
//</ TabItem >
//</ TabControl >
//</ UserControl >
//potem tutu ustowrz te zakladke methode daj
//potem methode do otwierania zakladek
//private void OpenTabs()
//{
//    Window window = new Window();
//    window.Content = new TabsView();
//    window.Title = "Tabs";
//    TabsViewModel vw = new TabsViewModel();
//    window.DataContext = vw;
//    window.Owner = Application.Current.MainWindow;
//    window.ShowDialog();
//    vm.RequestClose += delegate (object? sender, EventArgs e)
//    {
//        window.Close();
//        Debug.WriteLine("Close");
//    };
//}
//zamiast grida
//< ItemsControl Grid.Row = "1" ItemsSource = "{Binding Models}" HorizontalAlignemnt="Stretch">
//< ItemsControl.ItemsPanel >
//< ItemsPanelTemplate >
//< UniformGrid Rows = "1" ></ UniformGrid >
//</ ItemsPanelTemplate >
//</ ItemsControl.ItemsPanel >
//< ItemsControl.ItemTemplate >
//< DataTemplate >
//< StackPanel Margin = "5" >
//< TextBox Text"{Binding FirsName}"></TextBox>
//<Button Content="Usun"/>
//</StackPanel>
//</DataTemplate>
//</ItemsControl.ItemTemplate>
//</ItemsControl>

//datagridtemplateColumn.CellTemplate


//wpf expander ; group box ;html panel; treeview
//ContentControl
//CUrentView wpf https://stackoverflow.com/questions/75273937/how-do-i-go-to-another-view-from-a-view-in-mvvm-wpf
//https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern
//https://www.codeproject.com/Articles/165368/WPF-MVVM-Quick-Start-Tutorial