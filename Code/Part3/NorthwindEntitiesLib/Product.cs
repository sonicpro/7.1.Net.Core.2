namespace Packt.CS7
{
	public class Product
	{
		public int ProductID { get; set; }

		public string ProductName { get; set; }

		// Represents a foreign key constraint.
		public int? SupplierID { get; set; }
		public Supplier Supplier { get; set; }

		// Represents a foreign key constraint.
		public int? CategoryID { get; set; }
		public Category Category { get; set; }

		public string QuantityPerUnit { get; set; }

		public decimal? UnitPrice { get; set; }

		public short? UnitsInStock { get; set; }

		public short? UnitsInOrder { get; set; }

		public short? ReorderLevel { get; set; }

		public bool Discontinued { get; set; } = false;
	}
}
