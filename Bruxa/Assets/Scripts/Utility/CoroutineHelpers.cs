using System.Collections;
using UnityEngine;
public static class CoroutineHelpers
{

  public static IEnumerator InterpolateByTime(float time, System.Action<float> interpolator)
  {
    for (float t = 0f; t < time; t += Time.deltaTime)
    {
      var k = t / time;
      interpolator(k);
      yield return null;
    }
    interpolator(1);
  }

}