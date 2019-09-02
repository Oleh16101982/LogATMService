using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.IO;
using Org.Mentalis.Files;
using Logging;


namespace LogATMClient
{
	public partial class ClientService : ServiceBase
	{
		class FromServerCallBack : LogATM.IGetDateCallback
		{
			public Boolean IsAccess;

			public void ServerQuery(Int64 QueryVal)
			{

			}
		}
		public ClientService()
		{
			InitializeComponent();
		}

		static InstanceContext InsCon = new InstanceContext(new FromServerCallBack());

		public LogATM.GetDateClient proxy = new LogATM.GetDateClient(InsCon);

		public Timer Tim1 = new Timer();
		public fWriteEventLog fEvtLog;
		public fWriteFileLog fFileLog;

		protected override void OnStart(string[] args)
		{
			fCreateLogging();
			Boolean RetResult = proxy.Registration("Asdf", "192.168.2.2");
			if (RetResult)
			{

			}
			else
			{

			}
			Tim1.Interval = 1500;
			Tim1.Elapsed += new ElapsedEventHandler(Tim1_Tick);
			Tim1.Enabled = true;			

		}

		protected override void OnStop()
		{
			Tim1.Elapsed -= Tim1_Tick;
			Tim1.Enabled = false;
			proxy.Close();
		}
		private void Tim1_Tick(object sender, EventArgs e)
		{
			try
			{
				DateTime retDate = proxy.CurrDate();
				fLogging("CurrDate - " + Convert.ToString(retDate));
			}
			catch (FaultException excFault)
			{
				fLogging("Fault Exception.\n" + excFault.Code.Name + "\n" + excFault.Message.ToString() + "\n" + excFault.GetType().ToString());
				if (proxy.State == CommunicationState.Faulted)
				{
					fLogging("CommunicationState Faulted\n Attepmt to Create new proxy channel");
					proxy = new LogATM.GetDateClient(InsCon);
				}
			}
			catch (CommunicationException excComm)
			{
				fLogging("Communication Error\n" + excComm.Message.ToString() + "\n" + excComm.GetType().ToString());
			}
			catch (Exception exc)
			{
				fLogging("Exception.\n" + exc.Message + "\n" + exc.GetType().ToString());
			}			
		}
	
		public void fCreateLogging()
		{
			fEvtLog = new fWriteEventLog("LogATMClient","LogATMClientMainService",Environment.CommandLine.Remove(Environment.CommandLine.Length - 5, 5).Remove(0, 1) + "_resEvt.dll");
			fFileLog = new fWriteFileLog("MainService", Path.GetDirectoryName(Environment.CommandLine.Remove(Environment.CommandLine.Length - 1, 1).Remove(0, 1)) + @"\Log", 0);
		}

		protected void fLogging(String Msg, int Category = 0x01, EventLogEntryType EntryType = EventLogEntryType.Information, int Instance = 0x01)
		{
			if (!(fFileLog == null)) { fFileLog.WriteLog(Msg); }
			if (!(fEvtLog == null)) { fEvtLog.Category = Category; fEvtLog.EntryType = EntryType; fEvtLog.Instance = Instance; fEvtLog.WriteToLog(Msg); }
		}
	}
}
