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
	}

	public class ValueDetails
	{
		[AutoIncrement]
		public int Id { get; set; }
		public string AccountNumber { get; set; }
		public int MarketValue { get; set; }
		public int LandValue { get; set; }
		public int ImprovementsValue { get; set; }
		public int AbatementExemption { get; set; }
		public int Year { get; set; }
	}
}

