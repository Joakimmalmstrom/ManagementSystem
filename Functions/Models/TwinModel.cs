using System;
using System.Collections.Generic;
using System.Text;

namespace Functions.Models
{
    public class TwinModel
    {
        public string deviceId { get; set; }
        public string etag { get; set; }
        public string deviceEtag { get; set; }
        public string status { get; set; }
        public DateTime statusUpdateTime { get; set; }
        public string connectionState { get; set; }
        public DateTime lastActivityTime { get; set; }
        public int cloudToDeviceMessageCount { get; set; }
        public string authenticationType { get; set; }
        public X509thumbprint x509Thumbprint { get; set; }
        public string modelId { get; set; }
        public int version { get; set; }
        public Properties properties { get; set; }
        public Capabilities capabilities { get; set; }
    }

    public class X509thumbprint
    {
        public object primaryThumbprint { get; set; }
        public object secondaryThumbprint { get; set; }
    }

    public class Properties
    {
        public Desired desired { get; set; }
        public Reported reported { get; set; }
    }

    public class Desired
    {
        public Metadata metadata { get; set; }
        public int version { get; set; }
    }

    public class Metadata
    {
        public DateTime lastUpdated { get; set; }
    }

    public class Reported
    {
        public Metadata1 metadata { get; set; }
        public int version { get; set; }
    }

    public class Metadata1
    {
        public DateTime lastUpdated { get; set; }
    }

    public class Capabilities
    {
        public bool iotEdge { get; set; }
    }
}
