using System;
using Nancy.Hosting.Self;
using Nancy.TinyIoc;
using System.Linq;

namespace ApiStarter
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			try {
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
