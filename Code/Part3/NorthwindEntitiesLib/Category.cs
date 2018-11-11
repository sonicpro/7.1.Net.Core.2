using System.Collections.Generic;

namespace Packt.CS7
{
	public class Category
	{
		public int CategoryID { get; set; }

		public string CategoryName { get; set; }

		public string Description { get; set; }

		// Defines a navigation property for related rows.
		public ICollection<Product> Products { get; set; }
	}
}
