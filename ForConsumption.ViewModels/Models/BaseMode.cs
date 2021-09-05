using System;

using ForConsumption.Common.Common;
using ForConsumption.Common.Services;

using Plugins.ToolKits;
using Plugins.ToolKits.EasyHttp;

namespace ForConsumption.ViewModels.Models
{
    public abstract class BaseMode
    {
        private readonly IRestClient client = Injecter.Resolve<IRestClient>();
        protected IRestRequest CreateRequest(object? target = null)
        {
            PostContext context = PostContext.Create(target, MemberViewModel.Instance.Member?.ID ?? 0);

            context.SignToken = context.Sign();


            IRestRequest? request = client.Create()
                .AddHeader("Authorization", $"Bearer {PostContext.CurrentToken}")
                .UseDate(DateTime.Now)
                .UseMethod(Method.POST)
                .AddParameter(context);
            return request;
        }
    }
}
