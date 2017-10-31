using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraJump : MonoBehaviour {

    public void ChangeColorOverTime()
    {
        StartCoroutine(ColorManager.Instance.ChangeColorAfterTime(GetComponent<SpriteRenderer>(), 0.3f,new Color[] {
                new Color(189 / 255f, 38  / 255f, 27  / 255f),
                new Color(242 / 255f, 110 / 255f, 55  / 255f),
                new Color(209 / 255f, 188 / 255f, 49  / 255f),
                new Color(52  / 255f, 123 / 255f, 36  / 255f),
                new Color(66  / 255f, 187 / 255f, 249 / 255f)
            }
 ));
    }
}
