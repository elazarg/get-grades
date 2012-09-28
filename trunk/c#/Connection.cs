using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.ComponentModel;
using System.Windows.Forms;

namespace getGradesForms
{
    [Serializable]
    public class ConnectionException : System.ApplicationException { }

    [Serializable]
    public class ConnectionError : ConnectionException { }

    [Serializable]
    public class BadHtmlFormat : ConnectionException { }

    [Serializable]
    public class DimaErorr: ConnectionException { }

    class Connection : IDisposable
    {

 
    }

}
