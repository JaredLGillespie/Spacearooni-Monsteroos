using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAndDestroy : MonoBehaviour
{ 
    [SerializeField] private float ShrinkSize = 0.1f; // Destroy after this small
    [SerializeField] private Vector2 ShrinkSpeed = Vector2.zero; // Shrink by this much each interval
    [SerializeField] private float ShrinkInterval = 0.1f; // Shrink interval
    [SerializeField] private float Delay = 5.0f; // Delay

    private void Start()
    {
        StartCoroutine("ShrinkMe"); // This is the only time in which shrinkage is acceptable
    }

    private IEnumerator ShrinkMe()
    {
        yield return new WaitForSeconds(Delay);

        var minXReached = false;
        var minYReached = false;

        while (System.Math.Abs((new Vector2(this.transform.localScale.x, this.transform.localScale.y)).magnitude) >= ShrinkSize)
        {
            // Shrink x and y
            var scale = this.transform.localScale;
            if (!minXReached)
                if (scale.x > 0)
                    scale.x -= ShrinkSpeed.x;
                else
                    scale.y += ShrinkSpeed.x;

            if (!minYReached)
                if (scale.y > 0)
                    scale.y -= ShrinkSpeed.y;
                else
                    scale.y += ShrinkSpeed.y;

            var curScale = this.transform.localScale;
            if ((scale.x > 0 && curScale.x < 0) || (scale.x < 0 && curScale.x > 0))
                minXReached = true;

            if ((scale.y > 0 && curScale.y < 0) || (scale.y < 0 && curScale.y > 0))
                minYReached = true;

            this.transform.localScale = scale;

            yield return new WaitForSeconds(ShrinkInterval);
        }

        Destroy(this.gameObject);
    }
}
