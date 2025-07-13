using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkingText : MonoBehaviour
{
    public Text targetText;           // 깜빡일 텍스트
    public float blinkSpeed = 1.0f;   // 깜빡이는 속도 (초 단위)

    void Start()
    {
        if (targetText != null)
            StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            // 알파값 1 → 0 → 1로 천천히 깜빡이기
            for (float t = 0f; t < 1f; t += Time.deltaTime / blinkSpeed)
            {
                Color c = targetText.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                targetText.color = c;
                yield return null;
            }

            for (float t = 0f; t < 1f; t += Time.deltaTime / blinkSpeed)
            {
                Color c = targetText.color;
                c.a = Mathf.Lerp(0f, 1f, t);
                targetText.color = c;
                yield return null;
            }
        }
    }
}
