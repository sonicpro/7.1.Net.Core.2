namespace Packt.CS7
{
	public class OrderDetail
	{
		// Represents a foreign key constraint.
		public int OrderID { get; set; }
		public Order Order { get; set; }

		// Represents a foreign key constraint.
		public int ProductID { get; set; }
		public Product Product { get; set; }

		public decimal UnitPrice { get; set; }

		public short Quantity { get; set; } = 1;

		public double Discount { get; set; } = 0D;
	}
}
