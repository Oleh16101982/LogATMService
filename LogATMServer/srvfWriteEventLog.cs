using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
// using LogATMClient;

namespace Logging
{
	public class fWriteEventLog
	{

		protected String fLocalMachineName = ".";
		protected String LogName;
		protected String SrcName;

		protected EventLog fAppEvt = new EventLog("Application", ".", "ServiceLogATM");
		protected EventLog fEvt;

		public fWriteEventLog()
		{
			
		}
		public fWriteEventLog(String fLogName, String fSource, String fMsgFile)
		{
			if (!EventLog.SourceExists(fSource))
			{
				fAppEvt.WriteEntry("Source- " + fSource + " not exists");
				if (!System.IO.File.Exists(fMsgFile))
				{
					fAppEvt.WriteEntry("Message file - " + fMsgFile + " not exists");
					throw new LogATMServer.ServiceExceptions.EMsgFileNotFoundException("Message file for EventLog - " + fMsgFile + " not found", 0x01);
				}
				EventSourceCreationData mySourceData = new EventSourceCreationData(fSource, fLogName);
				mySourceData.LogName = fLogName;
				mySourceData.Source = fSource;
				mySourceData.CategoryCount = 32;
				mySourceData.CategoryResourceFile = fMsgFile;
				mySourceData.MessageResourceFile = fMsgFile;
				mySourceData.ParameterResourceFile = fMsgFile;
				mySourceData.MachineName = fLocalMachineName;
				EventLog fEvtCreate = new EventLog();
				EventLog.CreateEventSource(mySourceData);
				fAppEvt.WriteEntry("Source Created." + fLogName + ". " + fSource + ". " + fMsgFile);
				throw new LogATMServer.ServiceExceptions.ENeedServiceRestartException("Restart service neededd.", 0x01);
			}
			LogName = fLogName;
			SrcName = fSource;		
		}

		private long fInstance;
		private int fCategory;
		private EventLogEntryType fEntryType;
		private String fMsg = "";
	
		public long Instance	{get	{return fInstance;}	set	{fInstance = value;}}
		public int Category		{get	{return fCategory;}	set	{fCategory = value;}}
		public EventLogEntryType EntryType { get { return fEntryType; } set { fEntryType = value; } }
		public String Msg	{get { return fMsg; }	set { fMsg = value; }}
		
		public void WriteToLog(String WriteMessage = "")
		{
			EventInstance fEvtIns = new EventInstance(fInstance, fCategory, fEntryType);
			fEvt = new EventLog(LogName, ".", SrcName);
			fEvt.WriteEvent(fEvtIns, WriteMessage);
		}
	}
}
