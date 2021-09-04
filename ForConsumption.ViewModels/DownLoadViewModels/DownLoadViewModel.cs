
using System.Threading.Tasks;

using ForConsumption.Common.Common;
using ForConsumption.ViewModels.Models;

namespace ForConsumption.ViewModels.DownLoadViewModels
{
    public class DownLoadViewModel : BaseViewModel<DownLoadViewModel>
    {

        private readonly DownLoadModel DownLoadModel = new DownLoadModel();

        public Task<JsonResult<int>> GetNewestVersion()
        {
            return DownLoadModel.GetNewestVersionAsync();
        }


    }
}
