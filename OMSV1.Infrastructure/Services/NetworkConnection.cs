using System;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
namespace OMSV1.Infrastructure.Services;



public class NetworkConnection : IDisposable
{
    private string _networkPath;
    private string _username;
    private string _password;

    [DllImport("mpr.dll")]
    private static extern int WNetAddConnection2(ref NETRESOURCE netResource, string password, string username, int dwFlags);

    [DllImport("mpr.dll")]
    private static extern int WNetCancelConnection2(string name, int dwFlags, bool force);

    [StructLayout(LayoutKind.Sequential)]
    private class NETRESOURCE
    {
        public int dwScope = 0;
        public int dwType = 1;
        public int dwDisplayType = 3;
        public int dwUsage = 1;
        public string lpLocalName = null;
        public string lpRemoteName;
        public string lpComment = null;
        public string lpProvider = null;
    }

    public NetworkConnection(string networkPath, NetworkCredential credentials)
    {
        _networkPath = networkPath;
        _username = credentials.UserName;
        _password = credentials.Password;
    }

    public void Connect()
    {
        var netResource = new NETRESOURCE
        {
            lpRemoteName = _networkPath
        };

        int result = WNetAddConnection2(ref netResource, _password, _username, 0);
        if (result != 0)
        {
            throw new Exception($"Failed to connect to network path: {result}");
        }
    }

    public void Disconnect()
    {
        WNetCancelConnection2(_networkPath, 0, true);
    }

    public void Dispose()
    {
        Disconnect();
    }
}

