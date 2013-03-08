using System;
using System.Collections.Generic;
using ServiceStack.OrmLite;
using ServiceStack.DataAnnotations;

namespace ApiStarter
{
	public class PropertyDetails
	{
		[PrimaryKey]
		public string AccountNumber { get; set; }
		public string Address { get; set; }
		public string Unit { get; set; }
		public bool Homestead_Ex { get; set; }
		public string Prop_Cat { get; set; }
		public string Prop_Type { get; set; }
		public string Num_Stor{ get; set; }
		public double Latitude{ get; set; }
		public double Longitude{ get; set; }
		//public IDictionary<int,ValueDetails> Valuations { get; set; }
	}

	public class ValueDetails
	{
		[PrimaryKey]
		public string AccountNumber { get; set; }
		public int Mktval { get; set; }
		public int LandVal { get; set; }
		public int ImpVal { get; set; }
		public int Abat_Ex { get; set; }
		public int Year { get; set; }
	}
}

