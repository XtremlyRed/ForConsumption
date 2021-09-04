using System;
using System.IO;
using System.Linq;

using Android.Content;
using Android.Content.PM;

using AndroidX.Core.Content;

using ForConsumption.Common.Services;

using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;

namespace ForConsumption.Droid
{
    public class VersionManager : IVersionManager
    {
        private readonly Context context;
        private readonly PackageManager packageManager;
        private readonly string packageName;

        public VersionManager()
        {
            context = Android.App.Application.Context;
            packageManager = context.PackageManager;
            packageName = context.PackageName;

            PackageInfoFlags flags = PackageInfoFlags.Activities;
            PackageInfo pinfo = packageManager.GetPackageInfo(packageName, flags);

            VersionCode = pinfo.VersionCode;
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public int VersionCode { get; }

        public int GetNewestVersion()
        {
            return 1;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update(string apkurl)
        {
            string apkPath = "";

            IDownloadManager downloadManager = CrossDownloadManager.Current;

            downloadManager.PathNameForDownloadedFile = new Func<IDownloadFile, string>(df =>
            {
                string fileName = Android.Net.Uri.Parse(df.Url).Path.Split('/').Last() + $"/{DateTime.Now.Ticks}.apk";
                // apk的保存位置
                string directory = Android.OS.Environment.DirectoryDownloads;
                apkPath = Path.Combine(context.GetExternalFilesDir(directory).AbsolutePath, fileName);
                return apkPath;
            });

            IDownloadFile file = downloadManager.CreateDownloadFile(apkurl);

            file.PropertyChanged += (sender, e) =>
            {
                // 当下载完成时进行安装
                bool isCompleted = ((IDownloadFile)sender).Status == DownloadFileStatus.COMPLETED;
                if (e.PropertyName == "Status" && isCompleted)
                {
                    InstallApk(apkPath);
                }
            };

            downloadManager.Start(file);
        }

        /// <summary>
        /// 安装App
        /// </summary>
        /// <param name="apkbytes"></param>
        private void InstallApk(string apkPath)
        {
            Intent intent = new Intent(Intent.ActionView);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);

            Java.IO.File file = new Java.IO.File(apkPath);
            string intentType = "application/vnd.android.package-archive";

            // 安卓手机系统版本小于7.0和大于7.0的不同的安装方式
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.N)
            {
                intent.SetDataAndType(Android.Net.Uri.FromFile(file), intentType);
            }
            else
            {
                Android.Net.Uri contentUri = FileProvider.GetUriForFile(context, $"{packageName}.fileprovider", file);
                intent.SetDataAndType(contentUri, intentType);
            }

            context.StartActivity(intent);
        }
    }
}