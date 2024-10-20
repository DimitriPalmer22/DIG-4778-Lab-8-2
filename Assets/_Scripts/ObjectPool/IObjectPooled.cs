public interface IObjectPooled
{
    /// <summary>
    /// Called when the object is retrieved from the pool.
    /// </summary>
    public void OnPoolGet();

    /// <summary>
    /// Called when the object is released back to the pool.
    /// </summary>
    public void OnPoolRelease();

    /// <summary>
    /// Called when the object is destroyed by the pool.
    /// </summary>
    public void OnPoolDestroy();

    /// <summary>
    /// Called when the object is created by the pool.
    /// </summary>
    public void OnPoolCreate();
}