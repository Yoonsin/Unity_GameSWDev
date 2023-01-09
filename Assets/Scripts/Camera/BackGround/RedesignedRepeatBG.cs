using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedesignedRepeatBG : MonoBehaviour
{
    [SerializeField][Range(1f, 50f)] float BGspeed = 20f;
    [SerializeField] float posValue;

    Vector2 startPos;
    float newPos;

    void Start()
    {
        startPos=transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        newPos = Mathf.Repeat(Time.time * BGspeed, posValue);
        transform.localPosition = startPos + Vector2.left * newPos;
    }
}
