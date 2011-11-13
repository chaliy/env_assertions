namespace EnvAssertions.Servers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Management;
    using EnvAssertions.Utilities;    

    public class Server
    {        
        private readonly AdvancedServer advanced;
        private readonly ServerDisks disks;

        public Server(string name)
        {                        
            advanced = new AdvancedServer(name);
            disks = new ServerDisks(this);
        }

        public AdvancedServer Advanced
        {
            get
            {
                return advanced;
            }
        }

        public ServerDisks Disks
        {
            get
            {
                return disks;
            }
        }
               
        public static Server Connect(string ipOrName)
        {
            return new Server(ipOrName);
        }
    }

    public class ServerDisks
    {
        private readonly Server server;

        internal ServerDisks(Server server)
        {
            this.server = server;
        }

        public ServerLogicalDisk this[string letter]
        {
            get
            {
                return new ServerLogicalDisk(server, letter);
            }
        }
    }

    public class AdvancedServer
    {
        private readonly string serverName;
        private readonly AdvancedServerWmi wmi; 

        internal AdvancedServer(string serverName)
	    {
            this.serverName = serverName;
            wmi = new AdvancedServerWmi(serverName);

	    }

        public string ServerName
        {
            get
            {
                return serverName;
            }
        }

        public AdvancedServerWmi Wmi
        {
            get
            {
                return wmi;
            }
        }

    }

    public class AdvancedServerWmi
    {
        private readonly string serverName;

        internal AdvancedServerWmi(string serverName)
        {
            this.serverName = serverName;
        }

        public IEnumerable<dynamic> Query(string query)
        {
            return Wmi.SearchDefaultPath(serverName, query);
        }      
    }

    public class ServerLogicalDisk
    {
        private readonly Server server;
        private readonly string driveLetter;

        internal ServerLogicalDisk(Server server, string driveLetter)
        {
            this.server = server;
            this.driveLetter = driveLetter.EndsWith(":") ? driveLetter : driveLetter + ":";
        }        

        private dynamic QueryDiskInfo() 
        {
            return server.Advanced.Wmi
                .Query("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 3")
                .FirstOrDefault(x => x.DeviceID == driveLetter);
        }

        public FileSize FreeSpace
        {
            get
            {
                dynamic info = QueryDiskInfo();
                if (info != null)
                {
                    return info.FreeSpace;
                }
                return FileSize.Empty;
            }
        }

        public string DriveLetter
        {
            get
            {
                return driveLetter;
            }
        }
    }
}
