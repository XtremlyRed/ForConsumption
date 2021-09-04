using System;
using System.Threading.Tasks;

using Plugins.ToolKits.MVVM;

namespace ForConsumption.Assists
{
    public class MessageAssist : IMessageDialogService
    {
        public bool Confirm(string message, string title = null, params object[] paramaters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConfirmAsync(string message, string title = null, params object[] paramaters)
        {
            return AppShell.Current.DisplayAlert(title ?? "提示", message, "确认", "取消");
        }

        public bool Popup(object uIElement, string title = null, params object[] paramaters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PopupAsync(object uIElement, string title = null, params object[] paramaters)
        {
            throw new NotImplementedException();
        }

        public bool Show(string message, string title = null, params object[] paramaters)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ShowAsync(string message, string title = null, params object[] paramaters)
        {
            await AppShell.Current.DisplayAlert(title ?? "提示", message, "确认");
            return true;
        }
    }
}
