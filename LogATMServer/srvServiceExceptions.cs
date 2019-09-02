using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogATMServer
{
	public static class ServiceExceptions
	{

		public class EIniFileException : ApplicationException
		{
			protected String eMessage;
			protected int eCode;
			public String IniFileMessage { get { return eMessage; } }
			public int IniFileCode { get { return eCode; } }
			public EIniFileException(String SourceEMessage, int SourceECode)
			{
				eMessage = SourceEMessage;
				eCode = SourceECode;
			}
		}

		public class ECreateEventLoggingException : Exception
		{
			protected String eMessage;
			protected int eCode;
			public String EventMessage { get { return eMessage; } }
			public int EventCode { get { return eCode; } }
			public ECreateEventLoggingException(String SourceEMessage, int SourceECode)
			{
				eMessage = SourceEMessage;
				eCode = SourceECode;
			}
		}

		public class ECreateFileLoggingException : Exception
		{
			protected String eMessage;
			protected int eCode;
			public String FileMessage { get { return eMessage; } }
			public int FileCode { get { return eCode; } }
			public ECreateFileLoggingException(String SourceEMessage, int SourceECode)
			{
				eMessage = SourceEMessage;
				eCode = SourceECode;
			}
		}

		public class EMsgFileNotFoundException : Exception
		{
			protected String eMessage;
			protected int eCode;
			public String MsgFileNotFoundMessage { get { return eMessage; } }
			public int MsgFileNotFoundCode { get { return eCode; } }
			public EMsgFileNotFoundException(String SourceEMessage, int SourceECode)
			{
				eMessage = SourceEMessage;
				eCode = SourceECode;
			}
		}

		public class ENeedServiceRestartException : Exception
		{
			protected String eMessage;
			protected int eCode;
			public String NeedRestartMessage { get { return eMessage; } }
			public int NeedRestartCode { get { return eCode; } }
			public ENeedServiceRestartException(String SourceEMessage, int SourceECode)
			{
				eMessage = SourceEMessage;
				eCode = SourceECode;
			}
		}

		public class EErrorCreateLogFileException : Exception
		{
			protected String eMessage;
			protected int eCode;
			public String ErrorLogFileMessage { get { return eMessage; } }
			public int ErrorLogFileCode { get { return eCode; } }
			public EErrorCreateLogFileException(String SourceEMessage, int SourceECode)
			{
				eMessage = SourceEMessage;
				eCode = SourceECode;
			}
		}

		public class EMainThreadCreateException : Exception
		{
			protected String eMessage;
			protected int eCode;
			public String MainThreadCreateMessage { get { return eMessage; } }
			public int MainThreadCreateCode { get { return eCode; } }
			public EMainThreadCreateException(String SourceEMessage, int SourceECode)
			{
				eMessage = SourceEMessage;
				eCode = SourceECode;
			}
		}

		public class EWriteToLogFileException : Exception
		{
			protected String eMessage;
			protected int eCode;
			public String WriteToLogFileMessage { get { return eMessage; } }
			public int WriteToLogFileCode { get { return eCode; } }
			public EWriteToLogFileException(String SourceEMessage, int SourceECode)
			{
				eMessage = SourceEMessage;
				eCode = SourceECode;
			}
		}

	}
}
