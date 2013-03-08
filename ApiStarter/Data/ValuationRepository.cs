using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;
using ApiStarter.Data.Responses;

namespace ApiStarter
{
	///<summary>Encapsulates the valuation lookups.</summary>
	public class ValuationRepository
	{
		private Func<IDbConnection> _connectionProvider;

		public ValuationRepository()
		{
			_connectionProvider = () => "./valuations.db".OpenDbConnection();
		}

		///<summary>
		///Passing in the Func allows for testing... (As the connection could be to an in-memory sqlite db if needed.)
		///</summary>
		public ValuationRepository (Func<IDbConnection> connectionProvider)
		{
			_connectionProvider = connectionProvider;
		}

		public ValuationInformation GetPropertyDetailsByAccountNumber (string accountNumber)
		{
			return GetDetailsBySingleParameter("AccountNumber", accountNumber);
		}

		public ValuationInformation GetPropertyDetailsByAddress(string address)
		{
			//TODO: this should normalize the address before proceeding.
			return GetDetailsBySingleParameter("Address", address);
		}

		public ValuationAdjustment GetValuesDifferenceByAccountNumber(string accountNumber)
		{
			var details = GetPropertyDetailsByAccountNumber (accountNumber);
			return ValuationsForProperty (details);
		}

		public ValuationAdjustment GetValuesDifferenceByAddress (string address)
		{
			var details = GetPropertyDetailsByAddress (address);
			return ValuationsForProperty (details);
		}

		private ValuationInformation GetDetailsBySingleParameter(string fieldName, string parameter)
		{
			ValuationInformation retval = null;
			try
			{
				using (var db = _connectionProvider())
				{
					var details = db.Select<PropertyDetails>(fieldName + " = {0}", parameter).FirstOrDefault();
					if (details != null)
					{
						retval = new ValuationInformation
						{
							PropertyDetails = details,
							Valuations = db.Select<ValueDetails>("AccountNumber = {0}", details.AccountNumber).ToArray()
						};
					}
				}
			}
			catch (Exception ex)
			{

			}
			return retval;
		}

		private ValuationAdjustment ValuationsForProperty(ValuationInformation details)
		{
			ValuationAdjustment retval = null;
			if (details != null)
			{
				var adjustment = new ValuationAdjustment
				{
					PropertyDetails = details.PropertyDetails
				};

				// Since I am lazy and don't want to see how many years of data are here
				// I sort descending, take two, and then reverse the sort.
				var values = details.Valuations.OrderByDescending(k => k.Year)
					.Take(2).Reverse().ToArray();
				
				adjustment.StartYear = values.First().Year;
				adjustment.EndYear = values.Last().Year;

				//only if there's more than one year's values is there
				//a difference, otherwise, all differences are 0.
				if (values.Length == 2)
				{
					var first = values.First();
					var last = values.Last();

					adjustment.ImprovementsValueChange = last.ImprovementsValue - first.ImprovementsValue;
					adjustment.LandValueChange = last.LandValue - first.LandValue;
					adjustment.MarketValueChange = last.MarketValue - first.MarketValue;
					adjustment.AbatementExemptionChange = last.AbatementExemption - first.AbatementExemption;
				}
				
			}
			return retval;
		}

	}
}

