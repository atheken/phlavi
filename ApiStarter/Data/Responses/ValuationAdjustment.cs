using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiStarter.Data.Responses
{
	public class ValuationAdjustment
	{
		public PropertyDetails PropertyDetails { get; set; }
		public int LandValueChange { get; set; }
		public int MarketValueChange { get; set; }
		public int ImprovementsValueChange { get; set; }
		public int AbatementExemptionChange { get; set; }
		public int StartYear { get; set; }
		public int EndYear { get; set; }
	}
}
