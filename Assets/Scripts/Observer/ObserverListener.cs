using System.Collections.Generic;

public interface ObserverListener<T>
{
	public void deregisterBroadcaster();
	public void registerBroadcaster(ObserverBroadcaster<T> b);
	public void onObserverStateChange(T state, T prevState);
}
