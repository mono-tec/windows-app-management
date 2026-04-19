namespace TaskWorkerSample.Common;

/// <summary>
/// Mutex を使用した多重起動制御を補助します。
/// </summary>
public static class MutexHelper
{
    /// <summary>
    /// 指定キーで Mutex を生成し、初回起動かどうかを返します。
    /// </summary>
    /// <param name="mutexKey">Mutex キーです。</param>
    /// <param name="mutex">生成された Mutex を返します。</param>
    /// <returns>初回起動の場合は true、それ以外は false を返します。</returns>
    public static bool TryAcquire(string mutexKey, out Mutex? mutex)
    {
        mutex = null;

        try
        {
            mutex = new Mutex(initiallyOwned: true, name: mutexKey, createdNew: out bool createdNew);
            return createdNew;
        }
        catch
        {
            mutex?.Dispose();
            mutex = null;
            return false;
        }
    }
}