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

namespace LogATMServer
{
	public class clMainThread
	{
		protected System.Timers.Timer Tim1 = new System.Timers.Timer();
		protected fWriteFileLog fFileLog = null;
		protected Boolean IsEvtLog;
		protected Boolean IsFileLog;

		volatile Boolean IsAbort;
		public void NeedAbort() {IsAbort = true;}
		public StartProcedure StProc = new StartProcedure();
//		public clThreadHost HostThread = null;
//		public Thread fHostThread = null;


		public clMainThread()
		{

		}
		public clMainThread(Boolean IsEvtLogging = false, Boolean IsFileLogging = false)
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
				fFileLog = new fWriteFileLog("MainThread", Path.GetDirectoryName(Environment.CommandLine.Remove(Environment.CommandLine.Length - 1, 1).Remove(0, 1)) + @"\Log", 0);
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

		protected void fLogging(String Msg, int Category = 0x01, EventLogEntryType EntryType = EventLogEntryType.Information, int Instance = 0x01)
		{
			if (!(fFileLog == null)) { fFileLog.WriteLog(Msg); }
			if (!(LogATM_Server.fEvtLog == null)) { LogATM_Server.fEvtLog.Category = Category; LogATM_Server.fEvtLog.EntryType = EntryType; LogATM_Server.fEvtLog.Instance = Instance; LogATM_Server.fEvtLog.WriteToLog(Msg); }
		}
		#endregion

		public void Start()
		{
			if (!Tim1.Enabled) { Tim1.Enabled = true; }
			CheckNeedAbort();
			StartHostThread();
			while (true)
			{
				CheckNeedAbort();
				System.Threading.Thread.Sleep(555);
				fLogging("After Threading Sleep(555)");
			}
		}

		void CheckNeedAbort()
		{
			if (IsAbort)
			{
/*
				if (!(fHostThread == null))
				{
					HostThread.NeedAbort();
				}
 */ 
				Thread.CurrentThread.Abort();
			}
		}

		public void StartHostThread()
		{
/*
			try
			{
				HostThread = new clThreadHost(IsEvtLog, IsFileLog);
			}
			catch (ServiceExceptions.EMainThreadCreateException e)
			{
				fLogging("EMainThreadCreateException.\n. Message - " + e.MainThreadCreateMessage + "\nCode - " + e.MainThreadCreateCode);
			}
			catch (Exception e)
			{
				fLogging("Exception.\n. Message - " + e.Message + "\nSource - " + e.Source);

//				this.Stop();
			}
 */ 
/*
			try
			{
				fHostThread = new Thread(HostThread.Start);
				fHostThread.Name = "HostThread";
				fHostThread.Start();
			}
			catch (Exception e)
			{
				fLogging("Exception.\nError strated MainThread.\n" + e.Message + "\n" + e.Source);
			}
*/
		}
	}
}
