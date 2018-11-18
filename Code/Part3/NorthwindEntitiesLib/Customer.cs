using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Packt.CS7
{
	public class Customer
	{
		public string CustomerID { get; set; }

		public string CompanyName { get; set; }

		public string ContactName { get; set; }

		public string ContactTitle { get; set; }

		public string Address { get; set; }

		public string City { get; set; }

		public string Region { get; set; }

		public string PostalCode { get; set; }

		public string Country { get; set; }

		public string Phone { get; set; }

		public string Fax { get; set; }

		// Defines a navigation property for related rows.
		public ICollection<Order> Orders { get; set; }

		// override object.Equals
		public override bool Equals(object obj)
		{
			//       
			// See the full list of guidelines at
			//   http://go.microsoft.com/fwlink/?LinkID=85237  
			// and also the guidance for operator== at
			//   http://go.microsoft.com/fwlink/?LinkId=85238
			//

			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			var other = (Customer)obj;
			return CustomerID == other.CustomerID &&
				CompanyName == other.CompanyName &&
				ContactName == other.ContactName &&
				ContactTitle == other.ContactTitle &&
				Address == other.Address &&
				City == other.City &&
				Region == other.Region &&
				PostalCode == other.PostalCode &&
				Country == other.Country &&
				Phone == other.Phone &&
				Fax == other.Fax;
		}

		// override object.GetHashCode
		public override int GetHashCode()
		{
			int index = 3;
			string CustomerIDPadded = CustomerID + (CustomerID.Length > 3 ? "" : string.Join("", Enumerable.Repeat("A", 4 - CustomerID.Length).ToArray()));
			return Encoding.ASCII.GetBytes(CustomerIDPadded).Take(4).Aggregate(0, (accum, current) => accum + (current << (8 * index--)));
		}
	}
}
