using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Dispatcher;
using System.Diagnostics;

namespace LogATMServer
{
	#region
	[ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IServerCallback))]
	public interface IGetDate
	{
		[OperationContract]
		DateTime CurrDate();
		[OperationContract]
		Boolean Registration(String CompName, String IPAddress);
	}

	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class GetDate : IGetDate
	{
		private EventLog EvtLogMain = new EventLog("Application", ".", "From LogATMServer");
		public Boolean AccessClient;
		public DateTime CurrDate()
		{
			OperationContext context = OperationContext.Current;
			MessageProperties msgprop = context.IncomingMessageProperties;
			RemoteEndpointMessageProperty ednmsgprop = msgprop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
			EvtLogMain.WriteEntry("Remote Address - " + ednmsgprop.Address + ". Port - " + Convert.ToString(ednmsgprop.Port));
			return DateTime.Now;
		}

		public Boolean Registration(String CompName, String IPaddress)
		{
			AccessClient = CheckRegistration(CompName, IPaddress);
			IServerCallback IsrvCB = OperationContext.Current.GetCallbackChannel<IServerCallback>();
			if (AccessClient)
			{
				IsrvCB.ServerQuery(222);
			}
			return AccessClient;
		}

		Boolean CheckRegistration(String CompName, String IPAddress)
		{
			AccessClient = true;
			return AccessClient;
		}

	}

	public interface IServerCallback
	{
		[OperationContract(IsOneWay = true)]
		void ServerQuery(Int64 QueryVal);
	}
	#endregion

	static class Server
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] 
			{ 
				new LogATM_Server() 
			};
			ServiceBase.Run(ServicesToRun);
		}
	}
}
