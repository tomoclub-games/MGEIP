using System.Collections;
using System.Collections.Generic;
using Assets.Script.MGEIP.Service;
using DG.Tweening;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private MainMenuService mainMenuService;

    [Header("Reveal Animation")]
    [SerializeField] private List<GameObject> leftClouds;
    [SerializeField] private List<GameObject> rightClouds;
    [SerializeField] private GameObject crowsGO;
    [SerializeField] private GameObject environmentGO;
    [SerializeField] private Animator boatAnimator;
    [SerializeField] private float maxCloudDelay = 1f;
    [SerializeField] private float cloudAnimationDuration = 1f;
    [SerializeField] private float mapScaleDelay = 1f;
    [SerializeField] private float mapAnimationDuration = 3f;

    public void RevealAnimation()
    {
        crowsGO.SetActive(false);

        foreach (GameObject cloud in leftClouds)
        {
            float randomDelay = Random.Range(0f, maxCloudDelay);
            cloud.transform.DOMoveX(cloud.transform.position.x - 10f, cloudAnimationDuration).SetEase(Ease.InOutQuad).SetDelay(randomDelay);
        }

        foreach (GameObject cloud in rightClouds)
        {
            float randomDelay = Random.Range(0f, maxCloudDelay);
            cloud.transform.DOMoveX(cloud.transform.position.x + 10f, cloudAnimationDuration).SetEase(Ease.InOutQuad).SetDelay(randomDelay);
        }

        environmentGO.transform.localScale = Vector3.zero;
        environmentGO.transform.DOScale(Vector3.one, mapAnimationDuration).SetEase(Ease.OutSine).SetDelay(mapScaleDelay).OnComplete(() => { mainMenuService.AnimateStartButton(); crowsGO.SetActive(true); });
    }

    public void StartBoatAnim()
    {
        boatAnimator.enabled = true;
    }
}
