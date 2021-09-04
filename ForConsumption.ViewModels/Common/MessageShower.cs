using System.Threading.Tasks;

using Plugins.ToolKits;
using Plugins.ToolKits.MVVM;

namespace ForConsumption.ViewModels
{
    public abstract class MessageShower
    {
        public static Task<bool> ConfirmAsync(string message, string? title = null)
        {
            IMessageDialogService? mesageService = Injecter.Resolve<IMessageDialogService>();
            return mesageService.ConfirmAsync(message, title);
        }
        public static Task<bool> ShowAsync(string message, string? title = null)
        {
            IMessageDialogService? mesageService = Injecter.Resolve<IMessageDialogService>();
            return mesageService.ShowAsync(message, title);
        }
    }
}
