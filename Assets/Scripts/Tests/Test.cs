using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class Test
{
    [UnityTest]
    public IEnumerator TestRoutine()
    {
        yield return null;
        Assert.Pass();
    }
}
