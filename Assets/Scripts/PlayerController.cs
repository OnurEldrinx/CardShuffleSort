using DG.Tweening;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Slot fromSlot;
    public Slot toSlot;
    
    // Animation Settings
    public float totalDuration;
    public float delay;
    public float height;
    public Ease ease;
    
    private int _resX;
    private int _resY;
    //private int _hZ;

    public int totalCoin;
    public int totalGem;
    public int levelNo;
    
    public float cardDisableTime;

    public bool animationOnPlay;
    
    private Camera _mainCamera;

    public bool dealButtonActive;

    public float cardPositionOffsetY = 0.075f;
    
    private void Awake()
    {
        levelNo = 1;
        if (PlayerPrefs.HasKey("resX"))
        {
            _resX = PlayerPrefs.GetInt("resX");
            _resY = PlayerPrefs.GetInt("resY");
            //_hZ = PlayerPrefs.GetInt("hZ");
        }
        else
        {
            PlayerPrefs.SetInt("resX", Screen.currentResolution.width);
            PlayerPrefs.SetInt("resY", Screen.currentResolution.height);
            PlayerPrefs.SetInt("hZ", Screen.currentResolution.refreshRate);
            _resX = PlayerPrefs.GetInt("resX");
            _resY = PlayerPrefs.GetInt("resY");
            //_hZ = PlayerPrefs.GetInt("hZ");

        }

        if (GameManager.Instance.degradeResolution)
        {
            Screen.SetResolution((int)(_resX * 0.75f),(int)(_resY * 0.75f), FullScreenMode.FullScreenWindow);
        }
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (dealButtonActive) { return; }
        TouchHandler();
    }
    
    private void TouchHandler()
    {
        if (Input.touchCount <= 0) return;
        
        Touch touch = Input.GetTouch(0);

        Ray ray = _mainCamera.ScreenPointToRay(touch.position);

        if (!Physics.Raycast(ray, out var hit)) return;
        
        GameObject touchedObject = hit.collider.gameObject;

        //if (!touchedObject.TryGetComponent(out Slot s)) { return; }

        if (touch.phase == TouchPhase.Began)
        {

            if (touchedObject.TryGetComponent(out Slot slot))
            {
                switch (slot.status)
                {
                    case SlotStatus.Active:
                        slot.HandleTap();
                        break;
                    case SlotStatus.Locked :
                        print("Locked");
                        break;
                }
            }else if (touchedObject.TryGetComponent(out DealButton dealButton))
            {
                if (animationOnPlay)return;
                if (!dealButtonActive)
                {
                    dealButtonActive = true;
                }
                dealButton.HandleTap();
            }
        }

    }
}
