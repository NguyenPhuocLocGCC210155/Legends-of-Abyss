using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance {get; private set;}
    [SerializeField] CinemachineVirtualCamera[] allVirtualCamera;
    CinemachineVirtualCamera currentCamera;
    CinemachineFramingTransposer framingTransposer;

    [Header("Y Damping setting for Player Fall/Jump")]
    [SerializeField] float panAmount = 0.1f;
    [SerializeField] float panTime = 0.2f;
    public float playerFallSpeedTheshold = -10;
    public bool isLerpingYDamping;
    public bool hasLerpedYDamping;
    float normalYDamping;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        for (int i = 0; i < allVirtualCamera.Length; i++)
        {
            if (allVirtualCamera[i].enabled)
            {
                currentCamera = allVirtualCamera[i];

                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        normalYDamping = framingTransposer.m_YDamping;
    }

    private void Start() {
        for (int i = 0; i < allVirtualCamera.Length; i++)
        {
            allVirtualCamera[i].Follow = PlayerController.Instance.transform;
        }
    }

    public void SwapCamera(CinemachineVirtualCamera _newCam){
        currentCamera.enabled = false;
        currentCamera = _newCam;
        currentCamera.enabled = true;
    }

    public IEnumerator LerpYDamping(bool _isPlayerFalling){
        isLerpingYDamping = true;
        //take start y damp amount;
        float _startYDamp = framingTransposer.m_YDamping;
        float _endYDamp = 0;
        //determine end damp amout
        if (_isPlayerFalling)
        {
            _endYDamp = panAmount;
            hasLerpedYDamping = true;
        }else{
            _endYDamp = normalYDamping;
        }
        //lerp panAmount
        float timer = 0;
        while (timer < panTime){
            timer += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(_startYDamp, _endYDamp, (timer/panTime));
            framingTransposer.m_YDamping = lerpedPanAmount;
            yield return null;
        }
        isLerpingYDamping = false;
    }
}
