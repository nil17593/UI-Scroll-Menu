using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ScrollMenu : MonoBehaviour,IEndDragHandler//, IBeginDragHandler
{
    [SerializeField] private float scrollingSpeedOnButtonPress;
    [SerializeField] private ScrollRect scrollRect; // the scroll rect to scroll
    [SerializeField] private SnapDirection direction; // the direction we are scrolling
    [SerializeField] private int itemCount; // how many items we have in our scroll rect

    [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f); // a curve for transitioning in order to give it a little bit of extra polish
    [SerializeField] private float speed; // the speed in which we snap ( normalized position per second? )

    private void Start()
    {
        if (scrollRect == null) // if we are resetting or attaching our script, try and find a scroll rect for convenience 
            scrollRect = GetComponent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StopCoroutine(SnapRect()); // if we are snapping, stop for the next input
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(SnapRect());
    }

    private IEnumerator SnapRect()
    {
        if (scrollRect == null)
            throw new System.Exception("Scroll Rect can not be null");
        if (itemCount == 0)
            throw new System.Exception("Item count can not be zero");

        float startNormal;
        if (direction == SnapDirection.Horizontal)
        {
            startNormal = scrollRect.horizontalNormalizedPosition;
        }
        else
        {
            startNormal = scrollRect.verticalNormalizedPosition;
        }

        float delta = 1f / (float)(itemCount - 1); // percentage each item takes
        int target = Mathf.RoundToInt(startNormal / delta); // this finds us the closest target based on our starting point
        float endNormal = delta * target; // this finds the normalized value of our target
        float duration = Mathf.Abs((endNormal - startNormal) / speed); // this calculates the time it takes based on our speed to get to our target

        float timer = 0f;
        while (timer < 1f) // loop until we are done
        {
            timer = Mathf.Min(1f, timer + Time.deltaTime / duration); // calculate our timer based on our speed
            float value = Mathf.Lerp(startNormal, endNormal, curve.Evaluate(timer));// curve.Evaluate(timer)); // our value based on our animation curve, cause linear is lame

            if (direction == SnapDirection.Horizontal) // depending on direction we set our horizontal or vertical position
                scrollRect.horizontalNormalizedPosition = value;
            else
                scrollRect.verticalNormalizedPosition = value;
            yield return new WaitForEndOfFrame(); // wait until next frame
        }
           

    }

    public void OnMenuButtonClick(float valx)
    {
        scrollRect.DONormalizedPos(new Vector2(valx, 0), scrollingSpeedOnButtonPress);
    }

    public void OnVerticalMove(float valy)
    {
        scrollRect.DONormalizedPos(new Vector2(0, valy), scrollingSpeedOnButtonPress);
    }
}

// The direction we are snapping in
public enum SnapDirection
{
    Horizontal,
    Vertical,
}

