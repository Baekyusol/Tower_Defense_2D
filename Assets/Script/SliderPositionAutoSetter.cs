using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;
    private Transform targetTransform;
    private RectTransform rectTransform;

    public void Setup(Transform target)
    {
        // Slider가 쫒아다닐 target 설정
        targetTransform = target;
        rectTransform = GetComponent<RectTransform>();   
    }

    private void LateUpdate()
    {
        if (targetTransform == null) 
        {
            Destroy(gameObject); 
            return;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition + distance;
    }
}
