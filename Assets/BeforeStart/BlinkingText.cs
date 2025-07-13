using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkingText : MonoBehaviour
{
    public Text targetText;           // ������ �ؽ�Ʈ
    public float blinkSpeed = 1.0f;   // �����̴� �ӵ� (�� ����)

    void Start()
    {
        if (targetText != null)
            StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            // ���İ� 1 �� 0 �� 1�� õõ�� �����̱�
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
