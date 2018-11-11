using System.Collections.Generic;

namespace Packt.CS7
{
	public class Shipper
	{
		public int ShipperID { get; set; }

		public string ShipperName { get; set; }

		public string Phone { get; set; }

		// // Defines a navigation property for related rows.
		public ICollection<Order> Orders { get; set; }
	}
}
