using Plugins.ToolKits.MVVM;

namespace ForConsumption.ViewModels
{

    public abstract class BaseViewModel<TViewModel> : ViewModelBase<TViewModel> where TViewModel : ViewModelBase, new()
    {
        public virtual string Title { get; }
    }

    public abstract class BaseViewModel : ViewModelBase
    {
        public virtual string Title { get; }
    }
}
