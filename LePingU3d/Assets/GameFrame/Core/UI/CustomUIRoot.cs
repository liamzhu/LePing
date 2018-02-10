#define NGUI

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomUIRoot : MonoBehaviour
{
    private static CustomUIRoot m_Instance = null;

    public static CustomUIRoot Instance
    {
        get
        {
            if (m_Instance == null)
            {
                InitRoot();
            }
            return m_Instance;
        }
    }

    public Transform mUIRoot;
    public Transform mFixedRoot;
    public Transform mNormalRoot;
    public Transform mPopupRoot;
    public Transform mTopBarRoot;
    public Transform mGameUIRoot;
    public Camera mUICamera;

    private static void InitRoot()
    {
#if NGUI
        InitNGUIRoot();
#elif UGUI
        InitUGUIRoot();
#endif
    }

    private static void InitNGUIRoot()
    {
        GameObject go = new GameObject("UIRoot");
        go.layer = LayerMask.NameToLayer("UI");
        m_Instance = go.GetOrAddComponent<CustomUIRoot>();
        m_Instance.mUIRoot = go.transform;

        UIRoot root = go.AddComponent<UIRoot>();
        root.scalingStyle = UIRoot.Scaling.Constrained;
        root.manualWidth = AppConst.AppContentWidth;
        root.manualHeight = AppConst.AppContentHeight;
        root.fitHeight = true;
        root.fitWidth = false;

        //go.AddComponent<UIPanel>();

        //Rigidbody rigidbody = go.AddComponent<Rigidbody>();
        //rigidbody.useGravity = false;
        //rigidbody.isKinematic = true;

        GameObject camGo = new GameObject("Camera");
        camGo.layer = LayerMask.NameToLayer("UI");
        camGo.transform.parent = go.transform;
        camGo.transform.localPosition = Vector3.zero;

        Camera cam = camGo.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Depth;
        cam.depth = 2;
        cam.cullingMask = 5 << 5;
        cam.orthographic = true;
        cam.orthographicSize = 1;
        cam.farClipPlane = 200f;
        m_Instance.mUICamera = cam;
        cam.nearClipPlane = -10f;
        cam.farClipPlane = 10f;

        camGo.AddComponent<UICamera>();

        GameObject subRoot = CreateGameObject(go.transform);
        subRoot.name = "FixedRoot";
        m_Instance.mFixedRoot = subRoot.transform;

        subRoot = CreateGameObject(go.transform);
        subRoot.name = "NormalRoot";
        m_Instance.mNormalRoot = subRoot.transform;

        subRoot = CreateGameObject(go.transform);
        subRoot.name = "PopupRoot";
        m_Instance.mPopupRoot = subRoot.transform;

        subRoot = CreateGameObject(go.transform);
        subRoot.name = "TopBarRoot";
        m_Instance.mTopBarRoot = subRoot.transform;

        subRoot = CreateGameObject(go.transform);
        subRoot.name = "GameUIRoot";
        m_Instance.mGameUIRoot = subRoot.transform;
    }

    private static GameObject CreateGameObject(Transform root)
    {
        GameObject go = new GameObject("go");
        go.transform.parent = root;
        go.layer = LayerMask.NameToLayer("UI");
        return go;
    }

    private static void InitUGUIRoot()
    {
        GameObject go = new GameObject("UIRoot");
        go.layer = LayerMask.NameToLayer("UI");
        m_Instance = go.AddComponent<CustomUIRoot>();
        go.AddComponent<RectTransform>();
        m_Instance.mUIRoot = go.transform;

        Canvas can = go.AddComponent<Canvas>();
        can.renderMode = RenderMode.ScreenSpaceCamera;
        can.pixelPerfect = true;
        GameObject camObj = new GameObject("UICamera");
        camObj.layer = LayerMask.NameToLayer("UI");
        camObj.transform.parent = go.transform;
        camObj.transform.localPosition = new Vector3(0, 0, -100f);
        Camera cam = camObj.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Depth;
        cam.orthographic = true;
        cam.farClipPlane = 200f;
        can.worldCamera = cam;
        m_Instance.mUICamera = cam;
        cam.cullingMask = 1 << 5;
        cam.nearClipPlane = -50f;
        cam.farClipPlane = 50f;

        //add audio listener
        camObj.AddComponent<AudioListener>();
        camObj.AddComponent<GUILayer>();

        CanvasScaler cs = go.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1136f, 640f);
        cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        GameObject subRoot = CreateSubCanvasForRoot(go.transform, 250);
        subRoot.name = "FixedRoot";
        m_Instance.mFixedRoot = subRoot.transform;

        subRoot = CreateSubCanvasForRoot(go.transform, 0);
        subRoot.name = "NormalRoot";
        m_Instance.mNormalRoot = subRoot.transform;

        subRoot = CreateSubCanvasForRoot(go.transform, 500);
        subRoot.name = "PopupRoot";
        m_Instance.mPopupRoot = subRoot.transform;

        //add Event System
        GameObject esObj = GameObject.Find("EventSystem");
        if (esObj != null)
        {
            GameObject.DestroyImmediate(esObj);
        }

        GameObject eventObj = new GameObject("EventSystem");
        eventObj.layer = LayerMask.NameToLayer("UI");
        eventObj.transform.SetParent(go.transform);
        eventObj.AddComponent<EventSystem>();
        eventObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
    }

    private static GameObject CreateSubCanvasForRoot(Transform root, int sort)
    {
        GameObject go = new GameObject("canvas");
        go.transform.parent = root;
        go.layer = LayerMask.NameToLayer("UI");

        Canvas can = go.AddComponent<Canvas>();
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;

        can.overrideSorting = true;
        can.sortingOrder = sort;

        go.AddComponent<GraphicRaycaster>();

        return go;
    }

    private void OnDestroy()
    {
        m_Instance = null;
    }
}
