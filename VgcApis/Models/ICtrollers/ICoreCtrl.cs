namespace VgcApis.Models.IControllers
{
    public interface ICoreCtrl
    {

        string GetConfig();
        double GetIndex();
        string GetName();
        string GetStatus(); // speed test result e.g. 2000ms
        string GetTitle(); // 1.[serv1] vmess@1.2.3.4
        string GetUid(); // 16 hex chars

        bool IsCoreRunning();
        bool IsSelected();
        bool IsUntrack();

        void SetIsSelected(bool selected);
        void SetMark(string mark);

        void RestartCore();
        void RunSpeedTest();
        void StopCore();

        void ChangeConfig(string config);
        void ChangeInboundIpAndPort(string ip, int port);
        void ChangeInboundMode(int type); // 0 config 1 http 2 socks

        // for ProxySetter and Statistics 
        bool IsSuitableToBeUsedAsSysProxy(bool isGlobal, out bool isSocks, out int port);
        Datas.StatsSample TakeStatisticsSample();

    }
}
