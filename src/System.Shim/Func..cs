namespace System
{
    #if NETFRAMEWORK && !NET40_OR_NEWER
    public delegate TResult Func<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 args, T3 arg3, T4 arg4, T5 arg5);
    #endif
}
