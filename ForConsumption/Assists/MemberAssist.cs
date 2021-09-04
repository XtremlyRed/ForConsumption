using System;
using System.Threading.Tasks;

using ForConsumption.Common.Common;
using ForConsumption.ViewModels;

using Plugins.ToolKits;

namespace ForConsumption.Assists
{
    public static class MemberAssist
    {
        public static Task<bool> AutoLoginTask;


        public static void SaveLoginInfo()
        {
            MemberViewModel.Instance.Member.Password = MemberViewModel.Instance.Password;

            string[] loginInfo = new[]
            {
                MemberViewModel.Instance.LoginName,
                MemberViewModel.Instance.Password,
            };


            string memberJson = JsonMapper.Serialize(loginInfo);


            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string path = System.IO.Path.Combine(appdata, "login.cfg");

            System.IO.File.WriteAllText(path, memberJson, System.Text.Encoding.UTF8);

        }

        public static void AutoLogin()
        {
            AutoLoginTask = Task.Factory.StartNew(() =>
           {
               if (ReadLoginInfo() == false)
               {
                   return false;
               }

               if (MemberViewModel.Instance.LoginName.IsNullOrEmpty())
               {
                   return false;
               }
               if (MemberViewModel.Instance.Password.IsNullOrEmpty())
               {
                   return false;
               }

               bool result = MemberViewModel.Instance.Login().GetAwaiter().GetResult();

               return result;
           }, System.Threading.CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);



        }

        public static bool ReadLoginInfo()
        {
            try
            {

                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                string path = System.IO.Path.Combine(appdata, "login.cfg");

                if (!System.IO.File.Exists(path))
                {
                    return false;
                }

                string memberJson = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);

                if (memberJson.IsNullOrEmpty())
                {
                    return false;
                }

                string[] member = JsonMapper.Deserialize<string[]>(memberJson);

                if (member is null || member.Length != 2)
                {
                    return false;
                }

                MemberViewModel.Instance.LoginName = member[0];
                MemberViewModel.Instance.Password = member[1];
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
