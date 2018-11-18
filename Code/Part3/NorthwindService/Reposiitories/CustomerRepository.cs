using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Packt.CS7;

namespace NorthwindService.Repositories
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
			c.CustomerID = c.CustomerID.ToUpperInvariant();

			await db.Customers.AddAsync(c);

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
			else // The Add operation is unsucessful, show "The chosen customerID is already exist" message to the user.
			{
				return null;
			}
		}

		public async Task<bool> DeleteAsync(string id)
		{
			return await Task.Run(() =>
			{
				id = id.ToUpperInvariant();

				Customer c = db.Customers.Find(id);
				db.Customers.Remove(c);
				int affected = db.SaveChanges();
				if (affected == 1)
				{
					return Task.Run(() =>
					{
						Customer removed;
						return customersCache.TryRemove(id, out removed);
					});
				}
				else
				{
					return Task.Run(() => false);
				}
			});
		}

		public async Task<IEnumerable<Customer>> RetrieveAllAsync()
		{
			return await Task.Run(() => customersCache.Values);
		}

		public Task<Customer> RetrieveAsync(string id)
		{
			return Task.Run(() =>
			{
				Customer c;
				customersCache.TryGetValue(id.ToUpperInvariant(), out c);
				return c;
			});
		}

		public async Task<Customer> UpdateAsync(string id, Customer c)
		{
			id = id.ToUpperInvariant();
			c.CustomerID = c.CustomerID.ToUpperInvariant();

			db.Customers.Update(c);
			int affected = await db.SaveChangesAsync();

			if (affected == 1)
			{
				Customer old;
				if (customersCache.TryGetValue(id, out old) && customersCache.TryUpdate(id, c, old))
				{
					return c;
				}
				else
				{
					return null; // Display "The data has been already modified" message to the user.
				}
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
