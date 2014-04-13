<Query Kind="Program">
  <Reference Relative="Adaptive.ReactiveTrader.Client.Domain\bin\Debug\Adaptive.ReactiveTrader.Client.Domain.dll">&lt;MyDocuments&gt;\git\ReactiveTrader\src\Adaptive.ReactiveTrader.Client.Domain\bin\Debug\Adaptive.ReactiveTrader.Client.Domain.dll</Reference>
  <Reference Relative="Adaptive.ReactiveTrader.Client.Domain\bin\Debug\Adaptive.ReactiveTrader.Shared.dll">&lt;MyDocuments&gt;\git\ReactiveTrader\src\Adaptive.ReactiveTrader.Client.Domain\bin\Debug\Adaptive.ReactiveTrader.Shared.dll</Reference>
  <Reference Relative="Adaptive.ReactiveTrader.Client.Domain\bin\Debug\log4net.dll">&lt;MyDocuments&gt;\git\ReactiveTrader\src\Adaptive.ReactiveTrader.Client.Domain\bin\Debug\log4net.dll</Reference>
  <Reference Relative="Adaptive.ReactiveTrader.Client.Domain\bin\Debug\Microsoft.AspNet.SignalR.Client.dll">&lt;MyDocuments&gt;\git\ReactiveTrader\src\Adaptive.ReactiveTrader.Client.Domain\bin\Debug\Microsoft.AspNet.SignalR.Client.dll</Reference>
  <Reference Relative="Adaptive.ReactiveTrader.Client.Domain\bin\Debug\Newtonsoft.Json.dll">&lt;MyDocuments&gt;\git\ReactiveTrader\src\Adaptive.ReactiveTrader.Client.Domain\bin\Debug\Newtonsoft.Json.dll</Reference>
  <Reference Relative="Adaptive.ReactiveTrader.Client.Domain\bin\Debug\System.Reactive.Core.dll">&lt;MyDocuments&gt;\git\ReactiveTrader\src\Adaptive.ReactiveTrader.Client.Domain\bin\Debug\System.Reactive.Core.dll</Reference>
  <Reference Relative="Adaptive.ReactiveTrader.Client.Domain\bin\Debug\System.Reactive.Interfaces.dll">&lt;MyDocuments&gt;\git\ReactiveTrader\src\Adaptive.ReactiveTrader.Client.Domain\bin\Debug\System.Reactive.Interfaces.dll</Reference>
  <Reference Relative="Adaptive.ReactiveTrader.Client.Domain\bin\Debug\System.Reactive.Linq.dll">&lt;MyDocuments&gt;\git\ReactiveTrader\src\Adaptive.ReactiveTrader.Client.Domain\bin\Debug\System.Reactive.Linq.dll</Reference>
  <Reference Relative="Adaptive.ReactiveTrader.Client.Domain\bin\Debug\System.Reactive.PlatformServices.dll">&lt;MyDocuments&gt;\git\ReactiveTrader\src\Adaptive.ReactiveTrader.Client.Domain\bin\Debug\System.Reactive.PlatformServices.dll</Reference>
  <Namespace>Adaptive.ReactiveTrader.Client.Domain</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>Adaptive.ReactiveTrader.Client.Domain.Models</Namespace>
</Query>

void Main()
{
	var x = from dir in new [] {"Adaptive.ReactiveTrader.Client", "Adaptive.ReactiveTrader.Client.Domain", "Adaptive.ReactiveTrader.Shared" }
	        from file in Directory.EnumerateFiles(Path.Combine(Environment.CurrentDirectory, dir), "*.cs", SearchOption.AllDirectories)
			select file;
			
	x.Dump();
	
}