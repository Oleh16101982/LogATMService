using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Timers;
using System.IO;
using System.Threading;
using Logging;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace LogATMServer
{
	public class clThreadHost
	{
		protected System.Timers.Timer Tim1 = new System.Timers.Timer();
		protected fWriteFileLog fFileLog = null;
		protected Boolean IsEvtLog;
		protected Boolean IsFileLog;

		volatile Boolean IsAbort;
		public void NeedAbort() 
		{
			IsAbort = true;
		}
		public ServiceHost srv = new ServiceHost(typeof(GetDate));
		
		public clThreadHost()
		{

		}
		public clThreadHost(Boolean IsEvtLogging = false, Boolean IsFileLogging = false)
		{
			IsEvtLog = IsEvtLogging;
			IsFileLog = IsFileLogging;
			if (IsFileLogging)
			{
				try
				{
					fCreateFileLogging();
				}
				catch (ServiceExceptions.ECreateFileLoggingException e)
				{
					throw new ServiceExceptions.EMainThreadCreateException(e.FileMessage, e.FileCode);
				}
				catch (Exception e)
				{
					throw new ServiceExceptions.EMainThreadCreateException(e.Message , 0x00);
				}
			}
			Tim1.Interval = 1500;
			Tim1.Enabled = false;
			Tim1.Elapsed += new ElapsedEventHandler(OnTimer);
			fLogging("Class Main Thread created cuccessfully",Category: 0x02);
		}

		

		protected void OnTimer(object source, ElapsedEventArgs e)
		{
			fLogging("On Timer Tim1 EVENT");
		}
		#region region Logging
		protected void fCreateFileLogging()
		{
			try
			{
				fFileLog = new fWriteFileLog("HostThread", Path.GetDirectoryName(Environment.CommandLine.Remove(Environment.CommandLine.Length - 1, 1).Remove(0, 1)) + @"\Log", 0);
			}
			catch (ServiceExceptions.EErrorCreateLogFileException e)
			{
				throw new Exception("Error Create Log FIle\n" + e.ErrorLogFileMessage);
			}
			catch (Exception e)
			{
				throw new Exception("Exception in fCreateEventLogging.\n" + e.Message);
			}

		}

		protected void fLogging(String Msg, int Category = 0x01, EventLogEntryType EntryType = EventLogEntryType.Information, int Instance = 0x01)
		{
			if (!(fFileLog == null)) 
			{
				try
				{
					fFileLog.WriteLog(Msg);
				}
				catch (LogATMServer.ServiceExceptions.EWriteToLogFileException e)
				{
					{ LogATM_Server.fEvtLog.Category = Category; LogATM_Server.fEvtLog.EntryType = EventLogEntryType.Error; LogATM_Server.fEvtLog.Instance = Instance; LogATM_Server.fEvtLog.WriteToLog("Error write to file log\nMsg - " + e.WriteToLogFileMessage +"\nCode - " + Convert.ToString(e.WriteToLogFileCode)); }
				}
			}
			if (!(LogATM_Server.fEvtLog == null)) { LogATM_Server.fEvtLog.Category = Category; LogATM_Server.fEvtLog.EntryType = EntryType; LogATM_Server.fEvtLog.Instance = Instance; LogATM_Server.fEvtLog.WriteToLog(Msg); }
		}
		#endregion

		public void Start()
		{
			if (!Tim1.Enabled) { Tim1.Enabled = true; }
			CheckNeedAbort();
			fLogging("Host opened");
//			ServiceDiscoveryBehavior discoveryBehavior = new ServiceDiscoveryBehavior();
//			discoveryBehavior.AnnouncementEndpoints.Add(new UdpAnnouncementEndpoint());
//			srv.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
//			srv.AddServiceEndpoint(new UdpDiscoveryEndpoint());

/*
			NetTcpContextBinding srvBinding = new NetTcpContextBinding(SecurityMode.None, true);
			srvBinding.Name = "ServiceBinding";
			ServiceHost srv = new ServiceHost(typeof(GetDate), new Uri("net.tcp://192.168.0.51:8888/"));
			srv.AddServiceEndpoint(typeof(IGetDate), srvBinding, "LogATM/");
			ServiceMetadataBehavior mexBehavior = new ServiceMetadataBehavior();
			mexBehavior.HttpGetEnabled = false;
			srv.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
*/

			srv.Open();
			fLogging("Host started");
			while (true)
			{
				CheckNeedAbort();
				System.Threading.Thread.Sleep(777);
				fLogging("After Threading Sleep(777)");
			}
		}

		void CheckNeedAbort()
		{
			if (IsAbort)
			{
				fLogging("Host closing");
				srv.Close();
				fLogging("host closed");
				Thread.CurrentThread.Abort();
			}
		}

	}
}
