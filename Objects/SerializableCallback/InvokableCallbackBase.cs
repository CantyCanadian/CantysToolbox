
// Part of SerializableCallback by Siccity on Github : https://github.com/Siccity/SerializableCallback

public abstract class InvokableCallbackBase<TReturn>
{
	public abstract TReturn Invoke(params object[] args);
}