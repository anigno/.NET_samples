using System;
using System.IO;

namespace AnignoraCommunication.NetworkDiscovery
{
	
	/// <summary>
	/// Type of share
	/// </summary>
	[Flags]
	public enum ShareType
	{
		/// <summary>Disk share</summary>
		Disk		= 0,
		/// <summary>Printer share</summary>
		Printer		= 1,
		/// <summary>Device share</summary>
		Device		= 2,
		/// <summary>IPC share</summary>
		Ipc			= 3,
		/// <summary>Special share</summary>
		Special		= -2147483648, // 0x80000000,
	}
	
	
	/// <summary>
	/// Information about a local share
	/// </summary>
	public class Share
	{
		#region (------  Fields  ------)
		private readonly string m_netName;
		private readonly string m_path;
		private readonly string m_remark;
		private readonly string m_server;
		private readonly ShareType m_shareType;
		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)
	    /// <summary>
	    /// Constructor
	    /// </summary>
	    public Share(string p_server, string p_netName, string p_path, ShareType p_shareType, string p_remark) 
		{
			if (ShareType.Special == p_shareType && "IPC$" == p_netName)
			{
				p_shareType |= ShareType.Ipc;
			}
			
			m_server = p_server;
			m_netName = p_netName;
			m_path = p_path;
			m_shareType = p_shareType;
			m_remark = p_remark;
		}
		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)
		/// <summary>
		/// Returns true if this is a file system share
		/// </summary>
		public bool IsFileSystem 
		{
			get 
			{
				// Shared device
				if (0 != (m_shareType & ShareType.Device)) return false;
				// IPC share
				if (0 != (m_shareType & ShareType.Ipc)) return false;
				// Shared printer
				if (0 != (m_shareType & ShareType.Printer)) return false;
				
				// Standard disk share
				if (0 == (m_shareType & ShareType.Special)) return true;
				
				// Special disk share (e.g. C$)
                if (ShareType.Special == m_shareType && !string.IsNullOrEmpty(m_netName)) return true;
			    return false;
			}
		}

		/// <summary>
		/// Share name
		/// </summary>
		public string NetName 
		{
			get { return m_netName; }
		}

		/// <summary>
		/// Local path
		/// </summary>
		public string Path 
		{
			get { return m_path; }
		}

		/// <summary>
		/// Comment
		/// </summary>
		public string Remark 
		{
			get { return m_remark; }
		}

		/// <summary>
		/// Get the root of a disk-based share
		/// </summary>
		public DirectoryInfo DirectoryInfoRoot 
		{
			get
			{
			    if (IsFileSystem)
				{
				    if (string.IsNullOrEmpty(m_server))
						if (string.IsNullOrEmpty(m_path))
							return new DirectoryInfo(ToString());
						else
							return new DirectoryInfo(m_path);
				    return new DirectoryInfo(ToString());
				}
			    return null;
			}
		}

		/// <summary>
		/// The name of the computer that this share belongs to
		/// </summary>
		public string Server 
		{
			get { return m_server; }
		}

		/// <summary>
		/// Share type
		/// </summary>
		public ShareType ShareType 
		{
			get { return m_shareType; }
		}
		#endregion (------  Properties  ------)

		#region (------  Public Methods  ------)
	    /// <summary>
		/// Returns true if this share matches the local path
		/// </summary>
		/// <param name="p_path"></param>
		/// <returns></returns>
		public bool MatchesPath(string p_path) 
		{
			if (!IsFileSystem) return false;
			if (string.IsNullOrEmpty(p_path)) return true;
			
			return p_path.ToLower().StartsWith(m_path.ToLower());
		}

		/// <summary>
		/// Returns the path to this share
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
		    if (string.IsNullOrEmpty(m_server))
			{
				return string.Format(@"\\{0}\{1}", Environment.MachineName, m_netName);
			}
		    return string.Format(@"\\{0}\{1}", m_server, m_netName);
		}
		#endregion (------  Public Methods  ------)
	}
	
}