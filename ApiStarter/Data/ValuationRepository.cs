using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;


namespace ApiStarter
{
	///<summary>Encapsulates the valuation lookups.</summary>
	public class ValuationRepository
	{
		private Func<IDbConnection> _connectionProvider;
		
		public ValuationRepository ()
		{
		}

		///<summary>Allows for testing... (Queryable could be loaded via DB, or in memory..)</summary>
		public ValuationRepository (Func<IDbConnection> connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}

		public PropertyDetails GetPropertyDetailsByAccountNumber (string accountNumber)
		{
			throw new NotImplementedException();
			//return _queryable.FirstOrDefault (k => k.AccountNumber.ToLower ().Trim () == accountNumber.ToLower ().Trim ());
		}

		public PropertyDetails GetPropertyDetailsByAddress (string address)
		{
			throw new NotImplementedException();
			//return _queryable.FirstOrDefault (k => k.Address.ToLower ().Trim () 
			//	== address.ToLower ().Trim ());
		}

		public ValueDetails GetValuesDifferenceByAccountNumber (string accountNumber)
		{
			var details = GetPropertyDetailsByAccountNumber (accountNumber);
			return ValuationDifferences (details);
		}

		public ValueDetails GetValuesDifferenceByAddress (string address)
		{
			var details = GetPropertyDetailsByAddress (address);
			return ValuationDifferences (details);
		}

		private ValueDetails ValuationDifferences (PropertyDetails details)
		{
			throw new NotImplementedException();
		}

	}
}

