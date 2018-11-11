using System;
using System.Collections.Generic;

namespace Packt.CS7
{
	public class Order
	{
		public int OrderID { get; set; }

		// Represents a foreign key constraint.
		public string CustomerID { get; set; }
		public Customer Customer { get; set; }

		// Represents a foreign key constraint.
		public int EmployeeID { get; set; }
		public Employee Employee { get; set; }

		public DateTime? OrderDate { get; set; }

		public DateTime? RequiredDate { get; set; }

		public DateTime? ShippedDate { get; set; }

		// Represents a foreign key constraint.
		public int ShipVia { get; set; }
		public Shipper Shipper { get; set; }

		public decimal? Freight { get; set; } = 0;

		// Defines a navigation property for related rows.
		public ICollection<OrderDetail> OrderDetails { get; set; }
	}
}
