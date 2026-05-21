using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAgent.Learning.Wpf.ViewModels
{

    public class ViewModelBase : ObservableObject // ObservableObject 是 CommunityToolkit.Mvvm 提供的基类，它帮你实现了 WPF 数据绑定最核心的接口 INotifyPropertyChanged
    {
        #region 无ObservableObject的写法

        private string _message1;

        public string Message1
        {
            get => _message1;
            set
            {
                _message1 = value;
                OnPropertyChanged(nameof(Message1));
            }
        }

        #endregion

        #region 有ObservableObject的写法

        public string Message2
        {
            get => field; // C# 13 field 关键字 或用 setField
            set => SetProperty(ref field, value);
        }
        #endregion
    }
}
