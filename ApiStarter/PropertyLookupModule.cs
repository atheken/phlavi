using System;
using System.Web;
using Nancy;
using System.Linq;
using ApiStarter;

namespace AviApi
{
	/// <summary>
	/// Nancy module for routing property loookups
	/// </summary>
	public class PropertyLookupModule : NancyModule
	{
		private ValuationRepository _valuations;

		public PropertyLookupModule (ValuationRepository repo)
		{
			_valuations = repo;

			Get ["/"] = parameters => {
				return "Hi, Check out the /account/ API!";
			};

			Get ["/account/{accountNumber}"] = parameters =>
			{
				return Response.AsJson (new {valuations=_valuations.GetPropertyDetailsByAccountNumber (parameters.accountNumber)});
			};

			Get ["/account/change/{accountNumber}"] = parameters =>
			{
				return Response.AsJson (new { valuation_difference = 
					_valuations.GetValuesDifferenceByAccountNumber (parameters.accountNumber)
				});
			};
            
			// Routes for Address lookup
			Get ["/address/{address}"] = parameters =>
			{
				return Response.AsJson (new {valuations=_valuations.GetPropertyDetailsByAddress (parameters.address)});
			};

			Get ["/address/change/{address}"] = parameters =>
			{
				return Response.AsJson (new { valuation_difference = 
					_valuations.GetValuesDifferenceByAddress (parameters.address)
				});
			};
		}
	}
}