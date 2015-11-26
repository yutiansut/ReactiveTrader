using System;
using PropertyChanged;
using System.ComponentModel;

namespace Adaptive.ReactiveTrader.Client
{
    [ImplementPropertyChanged]

    public class TestClass : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public TestClass()
        {
        }

        public string Test { get; set; }
    }
}

