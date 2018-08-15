using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour
{

  // the image you want to fade, assign in inspector
  public Image img;
  public float FadeTime;

  public void StartImageFade()
  {
    StartCoroutine(FadeImage(true));
  }

  IEnumerator FadeImage(bool fadeAway)
  {
    // fade from opaque to transparent
    if (fadeAway)
    {
      // loop over 1 second backwards
      for (float i = FadeTime; i >= 0; i -= Time.deltaTime)
      {
        // set color with i as alpha
        img.color = new Color(1, 1, 1, i);
        yield return null;
      }
    }
    // fade from transparent to opaque
    else
    {
      // loop over 1 second
      for (float i = 0; i <= FadeTime; i += Time.deltaTime)
      {
        // set color with i as alpha
        img.color = new Color(1, 1, 1, i);
        yield return null;
      }
    }
  }
}
