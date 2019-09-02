using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Mentalis.Files;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using Logging;

namespace LogATMServer
{
	public class StartProcedure
	{
		
		fWriteFileLog fFileLog = new fWriteFileLog("StartProcedure",  Path.GetDirectoryName(Environment.CommandLine.Remove(Environment.CommandLine.Length - 1, 1).Remove(0, 1)) + @"\Log", 0);
		public struct structIniMain
		{
			public String EvtLog;
			public String FileLog;
			public String sDaysSaveFileLog;
			public Int16 DaysSaveFileLog;
			public bool isEvtLog;
			public bool isFileLog;
		}
		public structIniMain sIniMain = new structIniMain();
//		public static String sIniFileName = Environment.CommandLine.Remove(Environment.CommandLine.Length - 4, 4).Remove(0, 1) + "ini";

		public void Start()
		{
			ReadIniMain();
		}
		
		public void ReadIniMain()
		{
			fFileLog.WriteLog("ReadIniMain Started");
//			if (!File.Exists(sIniFileName))
			if (!File.Exists(LogATM_Server.cIniFileName))
			{
				throw new ServiceExceptions.EIniFileException("Ini file not found - " + LogATM_Server.cIniFileName, 0x01);
			}
			IniReader IniMain;
			try
			{
				IniMain = new IniReader(LogATM_Server.cIniFileName);
			}
			catch (Exception e)
			{
				throw new ServiceExceptions.EIniFileException("Error create IniReader class. \n" + e.Message + "\nSource - " + e.Source, 0x02);
			}
			ArrayList AllSections = new ArrayList();
			AllSections = IniMain.GetSectionNames();
			Boolean IsSectionPresent;
			IsSectionPresent = false;

			if (AllSections.Count > 0)
			{
				foreach (String tmpSectionName in AllSections)
				{
					fFileLog.WriteLog("IniFileName - " + IniMain.Filename + ". SectionName - " + tmpSectionName);
					if (tmpSectionName == "Main")
					{
						IsSectionPresent = true;
						break;
					}
				}
			}
			if (!IsSectionPresent)
			{
				throw new ServiceExceptions.EIniFileException("Section Main does not Exists in IniFile. \n" + LogATM_Server.cIniFileName, 0x03);
			}
			sIniMain.EvtLog = IniMain.ReadString("Main", "IsEvtLog");
			
			if (sIniMain.EvtLog.Length == 0)
			{
				throw new ServiceExceptions.EIniFileException("Missing or not defined key IsEvtLog.\n" + LogATM_Server.cIniFileName, 0x04);
			}
			if (sIniMain.EvtLog.Remove(1).Contains("Y"))	{sIniMain.isEvtLog = true;}			else {sIniMain.isEvtLog=false;}
			
			sIniMain.FileLog = IniMain.ReadString("Main", "IsFileLog");
			if (sIniMain.FileLog.Length == 0)
			{
				throw new ServiceExceptions.EIniFileException("Missing or not defined key IsFileLog.\n" + LogATM_Server.cIniFileName, 0x05);
			}
			if (sIniMain.FileLog.Remove(1).Contains("Y")) { sIniMain.isFileLog = true; }	else { sIniMain.isFileLog = false; }
			sIniMain.sDaysSaveFileLog = IniMain.ReadString("Main", "DaysSaveFileLog");
			if (!Int16.TryParse(sIniMain.sDaysSaveFileLog, out sIniMain.DaysSaveFileLog))
			{
				throw new ServiceExceptions.EIniFileException("Missing or error defined key DaysSaveCopy.\n" + LogATM_Server.cIniFileName, 0x05);
			}

		}

			
		}
	}

