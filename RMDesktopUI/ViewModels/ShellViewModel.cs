using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.EventModels;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SimpleContainer _container;
        private SalesViewModel _salesVM;
        private IEventAggregator _events;


        public ShellViewModel(SimpleContainer container, SalesViewModel salesVM, 
            IEventAggregator events)
        {
            _container = container;
            _events = events;
            _salesVM = salesVM;

            _events.Subscribe(this);
            
            ActivateItem(_container.GetInstance<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
        }
    }
}
