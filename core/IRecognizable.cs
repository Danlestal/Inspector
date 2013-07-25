
namespace AudioScriptInspector.Core
{
    /// <summary>
    /// This interface will enable access to a named collection. It is used on the AudioScriptInspectorControl and it is a nive way to isolate the RACCOON dependencies on the InspectorCore project.
    /// </summary>
    public interface IRecognizable
    {
        /// <summary>
        /// Returns the key of the object collection.
        /// </summary>
        /// <returns></returns>
        string GetKey();
        /// <summary>
        /// It will return the value of a parameter.
        /// </summary>
        /// <param name="key">name of the parameter</param>
        /// <returns></returns>
        string GetParameter(string key);
        /// <summary>
        /// It will return all the parameter keys. It works the same way as a Dictionary.Keys.
        /// </summary>
        /// <returns></returns>
        string[] GetParameterKeys();
        /// <summary>
        /// It will return all the parameter values. It works the same way as a Dictionary.Values.
        /// </summary>
        /// <returns></returns>
        string[] GetParameterValues();

    }
}
