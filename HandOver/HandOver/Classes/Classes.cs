using HandOver.Singleton;

namespace HandOver.Classes
{
    public class Config
    {
        public string status { get; set; }
        public string reason { get; set; }
        public string path { get; set; } = DefaultValues.path;
        public string offline_pass { get; set; } = DefaultValues.offline_pass;
        public string start_path { get; set; } = DefaultValues.start_path;
        public int refreshInterval { get; set; } = int.Parse(DefaultValues.refreshIntervalDefault);
        public string sn { get; set; } = DefaultValues.sn;
        public bool loading { get; set; } = true;
        public bool notLoading { get; set; } = false;
    }

    public class RemoteJson
    {
        public string status { get; set; }
        public string reason { get; set; }
        public string path { get; set; }
        public string offline_pass { get; set; }
        public string start_path { get; set; }
    }
}
