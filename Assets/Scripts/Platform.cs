using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public enum ColorType { Red = 1, Green = 2, Blue = 3 }
    public ColorType currentColor;

    private Renderer rend;
    private float colorChangeTimer = 0f;
    public float colorChangeInterval = 2f;
    public float minInterval = 0.5f;
    public bool isLocked = false;

    private Dictionary<ColorType, Color> colorMap;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("Renderer not found on Platform.");
            return;
        }

        colorMap = new Dictionary<ColorType, Color>
        {
            { ColorType.Red, Color.red },
            { ColorType.Green, Color.green },
            { ColorType.Blue, Color.blue }
        };

        ChangeColor();
    }

    void Update()
    {
        if (isLocked || rend == null) return;

        colorChangeTimer += Time.deltaTime;
        if (colorChangeTimer >= colorChangeInterval)
        {
            ChangeColor();
            colorChangeTimer = 0f;

            if (colorChangeInterval > minInterval)
                colorChangeInterval -= 0.1f;
        }

        if (transform.position.y <= -20f)
        {
            Destroy(gameObject);
        }
    }

    void ChangeColor()
    {
        currentColor = (ColorType)Random.Range(1, 4);
        if (colorMap.ContainsKey(currentColor))
        {
            rend.material.color = colorMap[currentColor];
        }
    }

    public void LockColor()
    {
        isLocked = true;
    }

    public int GetColorCode()
    {
        return (int)currentColor;
    }
}
