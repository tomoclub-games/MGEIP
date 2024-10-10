using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CrowAnimation : MonoBehaviour
{
    [SerializeField] private bool leftToRight;
    [SerializeField] private float distance = 100f;
    [SerializeField] private float moveDuration = 5f;
    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float maxInterval = 5f;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private void Start()
    {
        AnimateCrow();

        startPosition = transform.localPosition;
    }

    private void AnimateCrow()
    {
        Sequence crowSequence = DOTween.Sequence();

        crowSequence.AppendInterval(Random.Range(minInterval, maxInterval));
        if (leftToRight)
            crowSequence.Append(transform.DOMoveX(transform.localPosition.x + distance, moveDuration).SetEase(Ease.Linear));
        else
            crowSequence.Append(transform.DOMoveX(transform.localPosition.x - distance, moveDuration).SetEase(Ease.Linear));
        crowSequence.AppendCallback(() => { transform.localPosition = startPosition; });

        crowSequence.SetLoops(-1, LoopType.Restart);
    }
}
