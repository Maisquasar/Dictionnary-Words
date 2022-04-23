using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Graphic : MonoBehaviour
{
    [SerializeField] GameObject ReferencePoint;
    [SerializeField] LineRenderer ReferenceLine;
    [SerializeField] Gradient _color;
    [SerializeField] float MaxYValue = 120f; // The Y scale.
    [SerializeField] float MaxValues = 100; //The Max Points on Graph
    [SerializeField] bool EnablePoints;

    float LineWidth = 0.05f;
    float PointSize = 0.5f;

    RectTransform _graphic;
    float MaxY;

    List<GameObject> _points = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        ReferencePoint.SetActive(false);
        _graphic = GetComponent<RectTransform>();
        MaxY = _graphic.sizeDelta.y;

        ReferenceLine.colorGradient = _color;
        ReferenceLine.loop = false;
        ReferenceLine.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAllPoints();
    }

    public void CreateNewPoint(int value)
    {
        if (_points.Count == MaxValues)
        {
            Destroy(_points[0]);
            _points.RemoveAt(0);
        }
        _points.Add(Instantiate(ReferencePoint, ReferencePoint.transform.parent));
        _points.Last().gameObject.SetActive(true);
        _points.Last().GetComponentInChildren<HorizontalLayoutGroup>().padding.bottom = GetY(value);
        _points.Last().GetComponentInChildren<Image>().enabled = EnablePoints;
    }


    // Height -> 
    // MaxYValue -> value
    public int GetY(float value)
    {
        int var = (int)(value * MaxY / MaxYValue);
        Debug.Log($"Value : {value}, Size of rect : {MaxY}, Final Y {var}");
        if (var > MaxY)
            return (int)MaxY;
        return var;
    }

    public void UpdateAllPoints()
    {
        ReferenceLine.positionCount = 0;
        ReferenceLine.positionCount = _points.Count;
        for (int i = 0; i < ReferenceLine.positionCount; i++)
        {
            var pos = _points[i].GetComponentInChildren<PointPosition>().transform.position;
            if (pos == ReferenceLine.GetPosition(i))
                continue;
            ReferenceLine.SetPosition(i, new Vector3(pos.x, pos.y, 20));
        }

        SetLineWidth();
        SetPointSize();
    }

    public void SetLineWidth()
    {
        if (ReferenceLine.positionCount != 0)
        {
            float width = (LineWidth * (float)(2f / ReferenceLine.positionCount)) + 0.01f;
            ReferenceLine.endWidth = width;
            ReferenceLine.startWidth = width;
        }
    }

    public void SetPointSize()
    {
        foreach (var point in _points)
        {
            if (point.GetComponentInChildren<Image>().rectTransform.sizeDelta.x <= 0.1f)
                continue;
            var x = ReferenceLine.positionCount;
            float size = PointSize - (0.01f * x);
            //float size = PointSize * (1f / ReferenceLine.positionCount + 0.5f);
            point.GetComponentInChildren<Image>().rectTransform.sizeDelta = new Vector2(size, size);
        }
    }
}
