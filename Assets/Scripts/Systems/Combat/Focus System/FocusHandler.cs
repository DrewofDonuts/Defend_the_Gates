using System;
using System.Collections;
using Etheral;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FocusHandler : MonoBehaviour
{
    [SerializeField] GameObject focusSystemUI;
    [SerializeField] public RectTransform shape; // The shape to move in a circular path
    [SerializeField] public float radius = 100f; // Radius of the circular path
    [SerializeField] public float speed = 2f; // Speed of the shape’s rotation
    [SerializeField] public float targetAngle = 90f; // Angle that counts as a "hit"
    [MaxValue(30)]
    [SerializeField] float tolerance = 15f; // Angle tolerance for scoring
    [SerializeField] public RectTransform indicator;

    [SerializeField] Image[] focusImages;

    [ReadOnly]
    public float currentAngle = 0f; // Tracks the shape’s current angle

    WaitForSeconds wait = new(.5f);
    public bool isActive { get; private set; }

    void Start()
    {
        isActive = false;
        focusSystemUI.SetActive(false);
    }

    void Update()
    {
        if (focusSystemUI.activeSelf)
            MoveInCircle();
    }

    public void StartFocus()
    {
        isActive = true;
        focusSystemUI.SetActive(true);
        SetTargetAngle();
        SetIndicatorPosition();
        StartCoroutine(BeginFadingUIImages());
    }

    public bool AttemptFocus()
    {
        // Check if shape's angle is within the target angle ± tolerance
        if (Mathf.Abs(currentAngle - targetAngle) <= tolerance)
        {
            Debug.Log("Focus Successful!");
            DisableFocus();
            return true;
        }
        else
        {
            Debug.Log("Focus Failed!");
            DisableFocus();
            return false;
        }
    }


    IEnumerator BeginFadingUIImages()
    {
        yield return wait;
        foreach (var image in focusImages)
        {
            image.CrossFadeAlpha(0, 1f, true);
        }

        yield return new WaitForSeconds(1.1f);
        DisableFocus();
    }


    void SetTargetAngle()
    {
        int random = UnityEngine.Random.Range(20, 340);
        targetAngle = random;
    }

    void SetIndicatorPosition()
    {
        // Calculate the position of the indicator based on the target angle
        float x = Mathf.Cos(targetAngle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(targetAngle * Mathf.Deg2Rad) * radius;
        indicator.anchoredPosition = new Vector2(x, y);

        indicator.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }


    void MoveInCircle()
    {
        var _speed = speed;

        if (Time.timeScale < 1) _speed /= Time.timeScale;


        // Increment the current angle based on speed
        currentAngle += _speed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;

        // Calculate new position in the circular path
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
        shape.anchoredPosition = new Vector2(x, y);
    }

    public void DisableFocus()
    {
        isActive = false;
        focusSystemUI.SetActive(false);
        currentAngle = 0f;

        foreach (var image in focusImages)
        {
            image.CrossFadeAlpha(1, 0, true);
        }
    }
}