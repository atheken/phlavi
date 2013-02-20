using System;
using System.Collections.Generic;

namespace ApiStarter
{
	public class PropertyDetails
	{
		public string Acct_Num { get; set; }
		public string Address { get; set; }
		public string Unit { get; set; }
		public bool Homestead_Ex { get; set; }
		public string Prop_Cat { get; set; }
		public string Prop_Type { get; set; }
		public string Num_Stor{ get; set; }
		public double Latitude{ get; set; }
		public double Longitude{ get; set; }
		public IDictionary<int,ValueDetails> Valuations { get; set; }
	}

	public class ValueDetails
	{
		public int Mktval { get; set; }
		public int LandVal { get; set; }
		public int ImpVal { get; set; }
		public int Abat_Ex { get; set; }
	}
}

