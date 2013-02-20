using System;
using System.Linq;
using System.Collections.Generic;


namespace ApiStarter
{
	///<summary>Encapsulates the valuation lookups.</summary>
	public class ValuationRepository
	{
		private static IQueryable<PropertyDetails> _queryable;

		public ValuationRepository ()
		{
			_queryable = _queryable ?? LoadValuations ();
		}

		///<summary>Allows for testing... (Queryable could be loaded via DB, or in memory..)</summary>
		public ValuationRepository (IQueryable<PropertyDetails> valuations)
		{
			_queryable = valuations;
		}

		public PropertyDetails GetPropertyDetailsByAccountNumber (string accountNumber)
		{
			return _queryable.FirstOrDefault (k => k.Acct_Num.ToLower ().Trim () == accountNumber.ToLower ().Trim ());
		}

		public PropertyDetails GetPropertyDetailsByAddress (string address)
		{
			return _queryable.FirstOrDefault (k => k.Address.ToLower ().Trim () 
				== address.ToLower ().Trim ());
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
			ValueDetails retval = null;

			if (details != null) {
				//grab the values in reverse order.
				var values = details.Valuations.OrderByDescending (k => k.Key)
					.Select (k => k.Value).ToArray ();
				if (values.Any ()) {
					retval = values.First ();
					if (values.Length > 1) {
						var previousValue = values [1];
						retval = new ValueDetails{
							Abat_Ex = retval.Abat_Ex - previousValue.Abat_Ex,
							ImpVal = retval.ImpVal - previousValue.ImpVal,
							LandVal = retval.LandVal - previousValue.LandVal,
							Mktval = previousValue.Mktval
						};
					}
				}
			}

			return retval;
		}

		private IQueryable<PropertyDetails> LoadValuations ()
		{
			Console.WriteLine ("Loading valuations from OpenDataPhilly.org");
			var loader = new ValuationLoader ();

			var data2014 = loader.LoadInformation<Valuation2014> (
				"http://www.phila.gov/OPA/Documents/Tax%20Year%202014%20Data%20Set.zip",
			    "AVI_2014_Dataset_XY.txt");

			var rowlookup = 

				data2014.AsParallel ().Select (row => 
				#region Map 2014 values to simple object.
				new PropertyDetails (){
					Acct_Num = row.Acct_Num,
					Address = row.Address,
					Homestead_Ex = row.Homestd_Ex,
					Latitude = row.Latitude,
					Longitude = row.Longitude,
					Num_Stor = row.Num_Stor,
					Prop_Cat = row.Prop_Cat,
					Prop_Type =row.Prop_Type,
					Unit = row.Unit,
					Valuations = new Dictionary<int, ValueDetails>(){ {2014 ,
							new ValueDetails{ 
								Abat_Ex = row.Abat_Ex_14, 
								ImpVal = row.ImpVal_14, 
								Mktval = row.Mktval_14, 
								LandVal = row.LandVal_14
							}
						}
					}
				}
				#endregion
			).ToDictionary (k => k.Acct_Num);

			Console.WriteLine ("Loaded 2014 data.");

			var data2013 = loader.LoadInformation<Valuation2013> (
				"http://www.phila.gov/OPA/Documents/Tax%20Year%202013%20Data%20Set.zip",
			    "AVI_2013_Dataset_XY.txt");

			Console.WriteLine ("Loaded 2013 data.");
			Console.WriteLine ("Starting 'app join' of data.");

			var contained = data2013.ToLookup (k => rowlookup.ContainsKey (k.Acct_Num));

			contained [true].AsParallel ().ForAll (row => rowlookup [row.Acct_Num]
				.Valuations.Add (2013, new ValueDetails{
					Abat_Ex = row.Abat_Ex_13, 
					ImpVal = row.ImpVal_13, 
					Mktval = row.Mktval_13, 
					LandVal = row.LandVal_13
			}));

			var outputRows = contained [false].AsParallel ().Select (row => 
				#region map the rest of the 2013 data.
				new PropertyDetails (){
					Acct_Num = row.Acct_Num,
					Address = row.Address,
					Homestead_Ex = row.Homestd_Ex,
					Latitude = row.Latitude,
					Longitude = row.Longitude,
					Num_Stor = row.Num_Stor,
					Prop_Cat = row.Prop_Cat,
					Prop_Type =row.Prop_Type,
					Unit = row.Unit,
					Valuations = new Dictionary<int, ValueDetails>(){ {2014 ,
							new ValueDetails{ 
								Abat_Ex = row.Abat_Ex_13, 
								ImpVal = row.ImpVal_13, 
								Mktval = row.Mktval_13, 
								LandVal = row.LandVal_13
							}
						}
					}
				}
				#endregion
			).ToList ();

			//combine the rows.
			outputRows.AddRange (rowlookup.Values);

			Console.WriteLine ("Finished loading valuations from OpenDataPhilly.org");

			return outputRows.ToArray ().AsQueryable ();
		}
	}
}

