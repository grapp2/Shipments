using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipments.Model
{
    internal class LotUI
    {
        public LotUI(ShipmentUI parent, Lot lot)
        {
            Parent = parent;
            Lot = lot;
        }
        public ShipmentUI Parent { get; set; }
        public Lot Lot { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsEnabled { get; set; }
    }
}
