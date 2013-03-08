using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiStarter.Data
{
	/// <summary>
	/// Constructs a sqlite from the path.
	/// </summary>
	public class DbFileCreator
	{
		public void CreateDb(string dbPath)
		{
			Console.WriteLine("Loading valuations from OpenDataPhilly.org");
			var loader = new ValuationLoader();

			#region load 2014 data
			var data2014 = loader.LoadInformation<Valuation2014>(
				"http://www.phila.gov/OPA/Documents/Tax%20Year%202014%20Data%20Set.zip",
				"AVI_2014_Dataset_XY.txt").ToArray();

			using (var db = dbPath.OpenDbConnection())
			{
				//use a transaction because it's WAY faster than single inserts.
				using (var transaction = db.BeginTransaction())
				{
					db.DropAndCreateTable<PropertyDetails>();
					db.ExecuteSql("CREATE INDEX PropertyDetailsAccountNumber ON PropertyDetails(AccountNumber);");
					db.InsertAll(
					data2014.Select(row => new PropertyDetails()
					{
						AccountNumber = row.Acct_Num,
						Address = row.Address,
						Homestead_Ex = row.Homestd_Ex,
						Latitude = row.Latitude,
						Longitude = row.Longitude,
						Num_Stor = row.Num_Stor,
						Prop_Cat = row.Prop_Cat,
						Prop_Type = row.Prop_Type,
						Unit = row.Unit
					}));

					db.DropAndCreateTable<ValueDetails>();
					db.ExecuteSql("CREATE INDEX ValueDetailsAccountNumber ON ValueDetails(AccountNumber);");
					db.InsertAll(data2014.Select(row => new ValueDetails
					{
						AccountNumber = row.Acct_Num,
						Year = 2014,
						Abat_Ex = row.Abat_Ex_14,
						ImpVal = row.ImpVal_14,
						Mktval = row.Mktval_14,
						LandVal = row.LandVal_14
					}));
					transaction.Commit();
				}
			}
			#endregion

			#region load 2013 data

			var currentAccts = new HashSet<string>(data2014.Select(k => k.Acct_Num));
			//release the data2014 for GC.
			data2014 = null;

			var data2013 = loader.LoadInformation<Valuation2013>(
				"http://www.phila.gov/OPA/Documents/Tax%20Year%202013%20Data%20Set.zip",
				"AVI_2013_Dataset_XY.txt").ToArray();

			Console.WriteLine("Downloaded and parsed the 2013 data.");

			using (var db = dbPath.OpenDbConnection())
			{
				using (var transaction = db.BeginTransaction())
				{
					db.InsertAll(data2013.Select(row => new ValueDetails
					{
						AccountNumber = row.Acct_Num,
						Year = 2013,
						Abat_Ex = row.Abat_Ex_13,
						ImpVal = row.ImpVal_13,
						Mktval = row.Mktval_13,
						LandVal = row.LandVal_13
					}));

					db.InsertAll(data2013
						.Where(k => !currentAccts.Contains(k.Acct_Num))
						.Select(row => new PropertyDetails()
							{
								AccountNumber = row.Acct_Num,
								Address = row.Address,
								Homestead_Ex = row.Homestd_Ex,
								Latitude = row.Latitude,
								Longitude = row.Longitude,
								Num_Stor = row.Num_Stor,
								Prop_Cat = row.Prop_Cat,
								Prop_Type = row.Prop_Type,
								Unit = row.Unit
							}));
					transaction.Commit();
				}
			}
			#endregion

			using (var db = dbPath.OpenDbConnection())
			{
				db.ExecuteSql("VACUUM;");
			}

			Console.WriteLine("Finished loading valuations from OpenDataPhilly.org");
		}
	}
}
