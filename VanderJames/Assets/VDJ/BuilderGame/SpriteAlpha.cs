using UnityEngine;
using DG.Tweening;

public class SpriteAlpha : MonoBehaviour
{
    public void Invisible(float opacity, float duration){
            print("Fade");
            GetComponent<SpriteRenderer>().DOFade(opacity,duration);
    }

}
