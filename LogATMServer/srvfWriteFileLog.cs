using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Logging
{
	public class fWriteFileLog
	{
		private Object thisLock = new Object();
		protected EventLog EvtLogMain = new EventLog("Application", ".", "From LogATMServer");
		protected String fDirName = "";
		protected String fFullName = "";
		public fWriteFileLog()
		{
			fDefFileName();
		}
		public fWriteFileLog(String FileName)
		{
			fDefFileName(FileName);
		}
		public fWriteFileLog(String FileName, String DirName)
		{
			fDefFileName(FileName, DirName); 
		}
		public fWriteFileLog(String FileName, String DirName, int Number)
		{
			try
			{
				fDefFileName(FileName, DirName, Number);
			}
			catch (LogATMServer.ServiceExceptions.EErrorCreateLogFileException e)
			{
				throw new LogATMServer.ServiceExceptions.EErrorCreateLogFileException(e.ErrorLogFileMessage, 0x00);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}

		protected void fDefFileName(String FileName = "", String DirName = "", int Number = 0)
		{
			String PartOfName = "";
			if (DirName.Length != 0)
			{
				PartOfName = DirName + @"\" + fDatePart();
				fDirName = DirName;
			}
			else
			{
				PartOfName = "LOG" + @"\" + fDatePart();
				fDirName = "LOG" + @"\";
			}
			if (FileName.Length != 0)
			{
				PartOfName = PartOfName + "_" + FileName;
			}
			if (Number != 0)
			{
				PartOfName = PartOfName + "_" + Convert.ToString(Number).PadLeft(3, '0');
			}
			fFullName = PartOfName + ".log";

			if (!Directory.Exists(fDirName))
			{
				try
				{
					Directory.CreateDirectory(fDirName);
				}
				catch (NotSupportedException e)
				{
					throw new LogATMServer.ServiceExceptions.EErrorCreateLogFileException("NotSupportedException.\n" + e.Message + "\n" + e.Source, 0x01);
				}
				catch (PathTooLongException e)
				{
					throw new LogATMServer.ServiceExceptions.EErrorCreateLogFileException("PathTooLongException.\n" + e.Message + "\n" + e.Source, 0x02);
				}
				catch (DirectoryNotFoundException e)
				{
					throw new LogATMServer.ServiceExceptions.EErrorCreateLogFileException("DirectoryNotFoundException.\n" + e.Message + "\n" + e.Source, 0x03);
				}
				catch (IOException e)
				{
					throw new LogATMServer.ServiceExceptions.EErrorCreateLogFileException("IOException.\n" + e.Message + "\n" + e.Source, 0x04);
				}
				catch (UnauthorizedAccessException e)
				{
					throw new LogATMServer.ServiceExceptions.EErrorCreateLogFileException("UnauthorizedAccessException.\n" + e.Message + "\n" + e.Source, 0x05);
				}
				catch (ArgumentNullException e)
				{
					throw new LogATMServer.ServiceExceptions.EErrorCreateLogFileException("ArgumentNullException.\n" + e.Message + "\n" + e.Source, 0x06);
				}
				catch (ArgumentException e)
				{
					throw new LogATMServer.ServiceExceptions.EErrorCreateLogFileException("ArgumentException.\n" + e.Message + "\n" + e.Source, 0x07);
				}
				catch (Exception e)
				{
					throw new LogATMServer.ServiceExceptions.EErrorCreateLogFileException("Exception.\n" + e.Message + "\n" + e.Source, 0x08);
				}
			}
		}

		protected String fDatePart()
		{
		String RetVal = "";
		RetVal = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2 , '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
		return RetVal;
		}

		public void WriteLog(String TextMsg)
		{
			
			String ansiTextMsg;
			ansiTextMsg = TextMsg;
			lock (thisLock)
			{

				try
				{
					//				FileStream fLog = new FileStream(fFullName, FileMode.OpenOrCreate, FileAccess.Write);

					FileInfo fFileInfo = new FileInfo(fFullName);
					using (FileStream fLog = fFileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
					{
						StreamWriter swfLog = new StreamWriter(fLog);
						fLog.Seek(0, SeekOrigin.End);
						swfLog.Write(DateTime.Now.Day.ToString().PadLeft(2, '0') + "." + DateTime.Now.Month.ToString().PadLeft(2, '0') + "." + DateTime.Now.Year.ToString() + " "
												+ DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Millisecond.ToString().PadLeft(3, '0') + "\t");
						swfLog.WriteLine(TextMsg);
						swfLog.Flush();
					}
					//				fLog.Close();
				}
				catch (Exception e)
				{
					EvtLogMain.WriteEntry("Exception in writetolog.\n Message - " + e.Message + "\nSource - " + e.Source);
					throw new LogATMServer.ServiceExceptions.EWriteToLogFileException(e.Message + "\n" + e.Source, 0x01);
				}
			}

		}

	}
}
