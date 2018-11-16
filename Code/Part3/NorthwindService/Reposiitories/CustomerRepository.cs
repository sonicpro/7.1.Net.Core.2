using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Packt.CS7;

namespace NorthwindService.Reposiitories
{
	public class CustomerRepository : ICustomerRepository
	{
		private static ConcurrentDictionary<string, Customer> customersCache;

		private Northwind db;

		public CustomerRepository(Northwind db)
		{
			this.db = db;
			if (customersCache == null)
			{
				customersCache == new ConcurrentDictionary<string, Customer>(
					db.Customers.ToDictionary(c => c.CustomerID)));
			}
		}

		public Task<Customer> CreateAsync(Customer c)
		{
			throw new System.NotImplementedException();
		}

		public Task<bool> DeleteAsync(string id)
		{
			throw new System.NotImplementedException();
		}

		public Task<IEnumerable<Customer>> RetrieveAllAsync()
		{
			throw new System.NotImplementedException();
		}

		public Task<Customer> RetrieveAsync(string id)
		{
			throw new System.NotImplementedException();
		}

		public Task<Customer> UpdateAsync(string id, Customer c)
		{
			throw new System.NotImplementedException();
		}
	}
}
