using System.Collections.Generic;

public interface ObserverBroadcaster<T>
{
	public void registerObserver(ObserverListener<T> observer);
	public void deregisterObserver(ObserverListener<T> observer);
	public void updateObserverState(T state);
}
