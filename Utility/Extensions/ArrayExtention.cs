public static class ArrayExtention
{
    /// <summary>
    /// Applies a passed function to every member of the array.
    /// </summary>
    /// <param name="action">Function with a single Item argument and with an Item return type.</param>
    public static void DoOnAll<I>(this I[] target, System.Func<I, I> action)
    {
        for (int i = 0; i < target.Length; i++)
        {
            target[i] = action.Invoke(target[i]);
        }
    }
}
