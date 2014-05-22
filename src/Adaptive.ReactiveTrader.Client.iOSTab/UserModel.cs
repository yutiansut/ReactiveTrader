using System;

namespace Adaptive.ReactiveTrader.Client.iOSTab
{
	public class UserModel : NotifyingModel<UserModel>
	{
		static Boolean _oneTouchTradingEnabled = true;

		public Boolean GetOneTouchTradingEnabled()
		{
			return (_oneTouchTradingEnabled);
		}

		public void SetOneTouchTradingEnabled(Boolean newValue)
		{
			if (_oneTouchTradingEnabled != newValue) {
				_oneTouchTradingEnabled = newValue;
				NotifyOnChanged (this);
			}
		}
			
		public UserModel ()
		{
		}
	}
}

