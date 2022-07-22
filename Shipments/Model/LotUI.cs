using Shipments.Utility;
using Shipments.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.Model
{
    internal class LotUI : ViewModelBase
    {
        public LotUI(ShipmentUI parent, Lot lot)
        {
            Parent = parent;
            Lot = lot;
        }
        public ShipmentUI Parent { get; set; }
        private Lot lot;
        public Lot Lot
        {
            get
            {
                return lot;
            }
            set { 
                lot = value;
                OnPropertyChanged();
            }
        }

        public void UpdateDescription()
        {
            Lot.Item.Description = Functions.GetInitials(Lot.Item.Name);
            foreach (var specification in Lot.Item.Specifications)
            {
                Lot.Item.Description += "-" + Functions.GetInitials(specification.Name) + specification.Value;
            }
        }
    }
}
