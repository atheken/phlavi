using System;
using Nancy.Hosting.Self;
using Nancy.TinyIoc;
using System.Linq;
using ServiceStack.OrmLite;
using ApiStarter.Data;
using System.IO;

namespace ApiStarter
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//this is a global 'setup' option, it can be set on a per connection basis, but this is simpler.
			OrmLiteConfig.DialectProvider = SqliteDialect.Provider;

			try {
				var dbName = "./valuations.db";
				if (!File.Exists(dbName))
				{
					var fileCreator = new DbFileCreator();
					var tempName = dbName + Guid.NewGuid().ToString("n");
					fileCreator.CreateDb(tempName);
					//so, basicaly, this is an "atomic" move, once this is moved,
					//"valuations.db" will be complete. 
					//This guards against a "failed" bootstrapped db getting used.
					File.Move(tempName, dbName);
				}

				var uri = "http://localhost:9999";
				if (args.Length == 1) {
					uri = args [0];
				}

				var nancy = new NancyHost (new Uri (uri));

				nancy.Start ();
				Console.WriteLine ("Nancy's jammin' on '" + uri + "'. Press <enter> to stop the server.");
				Console.ReadLine ();
				nancy.Stop ();

			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
				Console.WriteLine (ex.StackTrace);
			}
		}
	}
}
