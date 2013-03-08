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
				string accountNumber = parameters.accountNumber.ToString();
				return Response.AsJson (_valuations.GetPropertyDetailsByAccountNumber (accountNumber));
			};

			Get ["/account/change/{accountNumber}"] = parameters =>
			{
				string accountNumber = parameters.accountNumber.ToString();
				return Response.AsJson ( _valuations.GetValuesDifferenceByAccountNumber (accountNumber));
			};
            
			// Routes for Address lookup
			Get ["/address/{address}"] = parameters =>
			{
				string address = parameters.address.ToString();
				return Response.AsJson (_valuations.GetPropertyDetailsByAddress (address));
			};

			Get ["/address/change/{address}"] = parameters =>
			{
				string address = parameters.address.ToString();
				return Response.AsJson (_valuations.GetValuesDifferenceByAddress (address));
			};
		}
	}
}