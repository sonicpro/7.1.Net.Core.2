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
				customersCache = new ConcurrentDictionary<string, Customer>(
					db.Customers.ToDictionary(c => c.CustomerID));
			}
		}

		#region Interface implementation

		public async Task<Customer> CreateAsync(Customer c)
		{
			c.CustomerID = c.CustomerID.ToUpper();

			EntityEntry<Customer> added = await db.Customers.AddAsync(c);

			int affected = await db.SaveChangesAsync();

			// Synchronise the successful Add operation into the cache.
			if (affected == 1)
			{
				// The customer might have been updated since the SaveChangesAsync() call above,
				// so use AddOrUpdate() providing the correct "Update value factory" implementation for our use case,
				// namely pass-through whatever the customer value is for the moment that the update is called
				// for not changing the cached value by the update.
				return customersCache.AddOrUpdate(c.CustomerID, c, (key, currentValue) => currentValue);
			}
			else // Concurrency conflict in the DB, we must retry the save operation.
			{
				return null;
			}
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

		#endregion
	}
}
