using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataLoadable
{
    /// <summary>
    /// Implementation of this interface should have this function. It should return an empty, new version of the child object.
    /// </summary>
    /// <returns>Empty version of the child class.</returns>
    IDataLoadable Clone();

    /// <summary>
    /// Allows the child object to take in the external data as parameter. Any number of strings can be passed in, but there is no built-in check to make sure everything is there.
    /// </summary>
    /// <param name="strings">Array containing all the data taken from the external file.</param>
    void LoadDataStrings(string[] strings);
}
