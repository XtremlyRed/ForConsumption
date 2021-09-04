namespace ForConsumption.Common.Services
{
    public interface IVersionManager
    {
        /// <summary>
        /// 版本号
        /// </summary>
        int VersionCode { get; }

        int GetNewestVersion();


        /// <summary>
        /// 更新
        /// </summary>
        void Update(string apkurl);
    }
}
