using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CameraHighlight : MonoBehaviour {

    public static CameraHighlight instance;

    private Vector3 originalRotation;
    private float originalFOV;
    public Image flash;
    public bool zoomed;
    public CanvasGroup letteringCanvas;
    public Text letteringText;

    public GameObject particlePrefab;
    //public Transform focus;


    public float timeToTarget = .8f;
    public float timeToComeBack = .8f;
    public float timeLookingAtObject = 1.0f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //cam = Camera.main.GetComponent<Camera>();
        originalRotation = transform.localEulerAngles;
        originalFOV = Camera.main.fieldOfView;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    zoomed = !zoomed;
        //    if (zoomed)
        //        LookAtTarget(focus, .5f);
        //    else ResetView(.5f);
        //}
    }

    public void LookAtTarget(Transform target, string targetName){
        letteringText.text = targetName;
        StopAllCoroutines();
        transform.DOLookAt(target.position, timeToTarget).SetEase(Ease.OutBack);
        Camera.main.DOFieldOfView(15, timeToTarget).SetEase(Ease.OutBack).OnComplete(()=>
        {
            Flash(target);
            StartCoroutine(WaitThenReset());
        });
    }

    private IEnumerator WaitThenReset()
    {
        yield return new WaitForSeconds(timeLookingAtObject);
        ResetView();
    }

    void Flash(Transform target){
        flash.DOFade(1, .1f).OnComplete(() => flash.DOFade(0, .2f));
        transform.DOComplete();
        transform.DOShakePosition(0.3f, .2f, 20, 90, false, true);

        letteringCanvas.DOFade(1,.2f);
        letteringCanvas.transform.DOComplete();
        letteringCanvas.transform.DOPunchScale(Vector3.one / 3, 0.4f, 10,1);

        Instantiate(particlePrefab, target);

        //letteringText.text = (SELECTED PLACE)

    }

    public void ResetView()
    {
        transform.DOComplete();
        transform.DORotate(originalRotation, timeToComeBack);
        Camera.main.DOFieldOfView(originalFOV, timeToComeBack);
        letteringCanvas.DOFade(0, .2f);
    }
}
