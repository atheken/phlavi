using System;
using System.Collections.Generic;
using System.Net;
using ServiceStack.Text;
using CsvHelper;
using Ionic.Zip;
using System.Linq;
using System.IO;

namespace ApiStarter
{
	public class ValuationLoader
	{
		public IEnumerable<T> LoadInformation<T> (String zipLocation, String csvFileName) where T:class,new()
		{
			var retval = Enumerable.Empty<T> ();
			try {
				using (var wc = new WebClient ()) {
					var bytes = wc.OpenRead (zipLocation).ReadFully ();
					using (var ms = new MemoryStream(bytes)) {
						ms.Position = 0;
						using (var zs = ZipFile.Read(ms)) {
							foreach (var item in zs.Entries) {
								if (item.FileName == csvFileName) {
									//this is a zip, so we need to do something slightly special to make this work..
									var csvReader = new CsvReader (new StreamReader (item.OpenReader ()));
									csvReader.Configuration.IsStrictMode = false;
									retval = csvReader.GetRecords<T> ().ToArray ();
								}
							}
						}
					}
				}
			} catch (Exception ex) {
				Console.Write (ex.Message);
			}
			return retval;
		}
	}
}

