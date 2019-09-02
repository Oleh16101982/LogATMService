using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Dispatcher;
using System.Timers;
using Org.Mentalis.Files;
using System.IO;
using System.Threading;

using Logging;

namespace LogATMServer
{
	public partial class LogATM_Server : ServiceBase
	{
		public ServiceHost srv = new ServiceHost(typeof(GetDate));
		public static String cLogName = "ServerForLogATM";
		public static String cEvtResourceFileName = Environment.CommandLine.Remove(Environment.CommandLine.Length - 5, 5).Remove(0, 1) + "_resEvt.dll";
		public static String cFileLogDir = Path.GetDirectoryName(Environment.CommandLine.Remove(Environment.CommandLine.Length - 1, 1).Remove(0, 1)) + @"\Log";
		public static String cIniFileName = Environment.CommandLine.Remove(Environment.CommandLine.Length - 4, 4).Remove(0, 1) + "ini";

		public static Logging.fWriteEventLog fEvtLog = null;
		public Logging.fWriteFileLog fFileLog = null;
		public clMainThread MainThread = null;
		public Thread fMainThread = null;

		public StartProcedure StProc = new StartProcedure();
		public LogATM_Server()
		{
			InitializeComponent();
		}
		public System.Timers.Timer Tim1 = new System.Timers.Timer();

		protected override void OnStart(string[] args)
		{
			Boolean IsErrorStartProcedure = false;
			Boolean IsErrorCreateLogging = false;
			try
			{
				StProc.Start();
			}
			catch (LogATMServer.ServiceExceptions.EIniFileException e)
			{
				fEvtLogMain.WriteEntry("EIniFileException\nIniFile Error. " + e.IniFileMessage + "\n" + "Code - " + Convert.ToString(e.IniFileCode), EventLogEntryType.Error);
				IsErrorStartProcedure = true;
				this.Stop();
			}
			catch (Exception e)
			{
				fEvtLogMain.WriteEntry("Exception\nIniFile Error. " + e.Message + "\n" + "Source - " + e.Source, EventLogEntryType.Error);
				IsErrorStartProcedure = true;
				this.Stop();
			}
			if (!IsErrorStartProcedure)
			{

				fEvtLogMain.WriteEntry("Host opened");
				srv.Open();
				fEvtLogMain.WriteEntry("Host started");
				Tim1.Interval = 1850;
				Tim1.Elapsed += new ElapsedEventHandler(Tim1_Tick);
				Tim1.Enabled = true;

				try
				{
					fCreateLogging();
					fLogging("Logging of this service started successfully");
				}
				catch (LogATMServer.ServiceExceptions.ECreateEventLoggingException e)
				{
					fEvtLogMain.WriteEntry("ECreateEventLoggingException\nCreate Event Logging Error. " + e.EventMessage + "\n" + "Code - " + Convert.ToString(e.EventCode), EventLogEntryType.Error);
					IsErrorCreateLogging = true;
					this.Stop();
				}
				catch (LogATMServer.ServiceExceptions.ECreateFileLoggingException e)
				{
					fEvtLogMain.WriteEntry("ECreateFileLoggingException\nCreate File Logging Error. " + e.FileMessage + "\n" + "Code - " + Convert.ToString(e.FileCode), EventLogEntryType.Error);
					IsErrorCreateLogging = true;
					this.Stop();
				}
				catch (Exception e)
				{
					fEvtLogMain.WriteEntry("Exception\ncREATElOGGING Error. " + e.Message + "\n" + "Source - " + e.Source, EventLogEntryType.Error);
					IsErrorCreateLogging = true;
					this.Stop();
				}
				if (!IsErrorCreateLogging)
				{
					try
					{
						MainThread = new clMainThread(StProc.sIniMain.isEvtLog, StProc.sIniMain.isFileLog);
					}
					catch (ServiceExceptions.EMainThreadCreateException e)
					{
						fEvtLogMain.WriteEntry("EMainThreadCreateException.\n. Message - " + e.MainThreadCreateMessage + "\nCode - " + e.MainThreadCreateCode, EventLogEntryType.Error);

						this.Stop();
					}
					catch (Exception e)
					{
						fEvtLogMain.WriteEntry("Exception.\n. Message - " + e.Message + "\nSource - " + e.Source, EventLogEntryType.Error);

						this.Stop();
					}
					try
					{
						fMainThread = new Thread(MainThread.Start);
						fMainThread.Name = "MainThread";
						fMainThread.Start();
					}
					catch (Exception e)
					{
						fEvtLogMain.WriteEntry("Exception.\nError strated MainThread.\n" + e.Message + "\n" + e.Source);
					}

				}

			}
		}

		protected override void OnStop()
		{
			if (!(fMainThread == null))
			{
				MainThread.NeedAbort();
			}
			fEvtLogMain.WriteEntry("LogATMServer service is stopped", EventLogEntryType.Warning);

			Tim1.Elapsed -= Tim1_Tick;
			Tim1.Enabled = false;

			fEvtLog.WriteToLog("Host closing");
			srv.Close();
			fEvtLog.WriteToLog("host closed");
		}

		#region
		protected void fCreateLogging()
		{
			if (StProc.sIniMain.isEvtLog)
			{
				try
				{
					fCreateEventLogging();
				}
				catch (ServiceExceptions.ECreateEventLoggingException e)
				{
					throw new ServiceExceptions.ECreateEventLoggingException(e.EventMessage, e.EventCode);
				}
				catch (Exception e)
				{
					throw new ServiceExceptions.ECreateEventLoggingException("Exception. Error create event logging\nMessage - " + e.Message, 0x01);
				}
			}

			if (StProc.sIniMain.isFileLog)
			{
				try
				{
					fCreateFileLogging();
				}
				catch (ServiceExceptions.EErrorCreateLogFileException e)
				{
					throw new ServiceExceptions.EErrorCreateLogFileException(e.ErrorLogFileMessage, e.ErrorLogFileCode);
				}
				catch (Exception e)
				{
					throw new ServiceExceptions.ECreateEventLoggingException("Error create file logging.\n" + e.Message, 0x01);
				}
			}
		}

		protected void fCreateEventLogging()
		{
			try
			{
				fEvtLog = new Logging.fWriteEventLog(cLogName, "MainService", cEvtResourceFileName);
			}
			catch (ServiceExceptions.EMsgFileNotFoundException e)
			{
				throw new ServiceExceptions.ECreateEventLoggingException("Message file - " + cEvtResourceFileName + " not found\n" + e.MsgFileNotFoundMessage + "\nCode - " + Convert.ToString(e.MsgFileNotFoundCode), 0x01);
			}
			catch (ServiceExceptions.ENeedServiceRestartException e)
			{
				throw new ServiceExceptions.ECreateEventLoggingException("Service need to restart.\nMessage - " + e.NeedRestartMessage + "\nCode - " + Convert.ToString(e.NeedRestartCode), 0x01);
			}
			catch (Exception e)
			{
				throw new Exception("Exception in fCreateEventLogging.\n" + e.Message);
			}
		}

		protected void fCreateFileLogging()
		{
			try
			{
				fFileLog = new fWriteFileLog("Main_Service", Path.GetDirectoryName(Environment.CommandLine.Remove(Environment.CommandLine.Length - 1, 1).Remove(0, 1)) + @"\Log", 0);
			}
			catch (ServiceExceptions.EErrorCreateLogFileException e)
			{
				throw new ServiceExceptions.EErrorCreateLogFileException(e.ErrorLogFileMessage, e.ErrorLogFileCode);
			}
			catch (Exception e)
			{
				throw new Exception("Exception in fCreateEventLogging.\n" + e.Message);
			}

		}
		#endregion

		protected void fLogging(String Msg, int Category = 0x01, EventLogEntryType EntryType = EventLogEntryType.Information, int Instance = 0x01)
		{
			if (!(fFileLog == null)) { fFileLog.WriteLog(Msg); }
			if (!(fEvtLog == null)) { fEvtLog.Category = Category; fEvtLog.EntryType = EntryType; fEvtLog.Instance = Instance; fEvtLog.WriteToLog(Msg); }
		}

		private void Tim1_Tick(object sender, EventArgs e)
		{
			fEvtLogMain.WriteEntry("Timer tick");
		}



	}
}
