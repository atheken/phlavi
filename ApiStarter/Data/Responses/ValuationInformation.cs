using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiStarter.Data.Responses
{
	public class ValuationInformation
	{
		public PropertyDetails PropertyDetails { get; set; }
		public IEnumerable<ValueDetails> Valuations { get; set; }
	}
}
