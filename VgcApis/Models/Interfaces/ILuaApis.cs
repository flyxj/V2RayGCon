namespace VgcApis.Models.Interfaces
{
    public interface ILuaApis
    {
        /// <summary>
        /// Api:Print("hello",", ","world","!")
        /// </summary>
        /// <param name="contents">objects</param>
        void Print(params object[] contents);

        /// <summary>
        /// Api:Sleep(1000) // one second
        /// </summary>
        /// <param name="millisecond"></param>
        void Sleep(int millisecond);


        /// <summary>
        /// Api:UpdateServerListCache()
        /// </summary>
        /// <returns></returns>
        void UpdateServerListCache();

        string[] GetAllServerUid();
        string GetServerUidByName(string name);
        string GetServerNameByUid(string uid);
        string GetServerTitleByUid(string uid);

        void SelectServerByUid(string uid);
        void UnSelectServerByUid(string uid);
        void StartServerByUid(string uid);
        void StopServerByUid(string uid);
    }
}
