using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class DrawManger : MonoBehaviour, GameManger.GameLauncher
{

    public GameObject line;
    public Linesbehaviour linesbehaviour;

    BoxCollider2D linecollider;
    BoxCollider2D drawercollider;
    LineRenderer linerenderer;
    
    Vector3 startPos;
    Vector3 endPos;
    List<GameObject> lines;
    Color defaultColor;

    float totaldraw = 0;


    public BoxCollider2D Col => linecollider;
    public LineRenderer Linerenderer => lines.Last<GameObject>().GetComponent<LineRenderer>();

    public void AddLine(GameObject line)
    {
        lines.Add(line);
    }
    private void Awake()
    {
        GameManger.Instance.AddGameLauncherListner(this);
        drawercollider = GetComponent<BoxCollider2D>();
        lines = new List<GameObject>();
    }

    void OnMouseDown()
    {
        if(GameManger.Instance.Currentlevelmaneger.GetMaxLineLength() < GameManger.Instance.totalDistanceDraw)
            return;

        if (IsPointerOverUIObject()) return;

        startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 9));
        StartMouseDown(startPos);

    }
    
    public void StartMouseDown(Vector3 startPos)
    {
        GameObject newline = Instantiate(line);

        linecollider = newline.GetComponent<BoxCollider2D>();
        linerenderer = newline.GetComponent<LineRenderer>();
        linerenderer.useWorldSpace = true;
        linerenderer.sortingOrder = 4;
        linerenderer.SetWidth(0.3f, 0.3f);

        linerenderer.SetPosition(0, startPos);
        linerenderer.SetPosition(1, startPos);

        Linesbehaviour.LineData lineData = linesbehaviour.GetLineData(0);

        if (lineData == null) return;

        linerenderer.SetWidth(lineData.start, lineData.end);
        linerenderer.material.color = lineData.color;
        if (GameManger.Instance != null)
            GameManger.Instance.HideTutorialIfActive();
        if (AudioManager.Instance != null)
            AudioManager.Instance.StartStretchSFX();
    }

    private void OnMouseDrag()
    {
        if (IsPointerOverUIObject()) return;
        endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 9);

        StartMouseDrag(endPos);
    }
    
    public void StartMouseDrag(Vector3 endPos)
    {
        float distance = Vector3.Distance(startPos, endPos);

        float total = GameManger.Instance.totalDistanceDraw+distance;
        
        if(total>GameManger.Instance.Currentlevelmaneger.GetMaxLineLength())
            return;

        linerenderer.SetPosition(1, endPos);

     

        Linesbehaviour.LineData lineData = linesbehaviour.GetLineData(distance);

        if (lineData == null) return;

        linerenderer.SetWidth(lineData.width, lineData.width);

        GameManger.Instance.TriggerOnDrawin(distance);

        linerenderer.material.color = lineData.color;
        // Debug.Log(UIManger.Instance.linelengthpercent.text);
        
    }

    private void OnMouseUp()
    {

        if (linerenderer == null)
            return;

        StartMouseUp();
    }
    
    public void StartMouseUp()
    {
        if(AudioManager.Instance != null)
        AudioManager.Instance.StopStretchSFX();
        
        float lineLength = Vector3.Distance(startPos, endPos); // length of line

        SetUpLine(lineLength,startPos,endPos);
    }
    
    public void SetUpLine(float lineLength,Vector3 startPos,Vector3 endPos)
    {
        if (lineLength < 0.4 || IsPointerOverUIObject())
        {

            if (AudioManager.Instance != null)
                AudioManager.Instance.StopStretchSFX();

            Destroy(linerenderer.gameObject);
            
            return;
        }

        GameManger.Instance.AddDistance(lineLength);
        totaldraw+=lineLength;
        Linesbehaviour.LineData lineData = linesbehaviour.GetLineData(lineLength);

        linecollider.size = new Vector3(lineLength, lineData.width, lineData.width * 10); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
        Vector3 midPoint = (startPos + endPos) / 2;
        linecollider.transform.position = midPoint; // setting position of collider object

        float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));

        if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        {
            angle *= -1;
        }

        angle = Mathf.Rad2Deg * Mathf.Atan(angle);

        linecollider.transform.Rotate(0, 0, angle);
        linerenderer.SetWidth(lineData.width, lineData.width);
        float divide = lineLength / 2.5f;
        linecollider.sharedMaterial.bounciness = lineData.strength;

        linerenderer.material.color = lineData.color;
        AddLine(linerenderer.gameObject);
        linerenderer = null;
    }
    
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void OnReady()
    {
        drawercollider.enabled = true;
    }
    
    public void OnStart()
    {
        drawercollider.enabled = false;
    }
    
    public void OnNextLevel()
    {

    }
    
    public void OnEnd()
    {
        foreach (GameObject gameObject in lines)
        {
            Destroy(gameObject);
        }
        lines.Clear();
    }
    
}
