using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDevelopment.Models.ViewModels
{
    public class GenericComboBoxVM<KeyType>
    {
        /// <summary>
        /// Displayed to the user
        /// </summary>
        public string Value { get; set; }
        public KeyType Key { get; set; }
        public GenericComboBoxVM(string value, KeyType key)
        {
            Value = value;
            Key = key;
        }
    }
}
