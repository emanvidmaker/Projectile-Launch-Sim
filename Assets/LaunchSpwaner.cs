using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchSpwaner : MonoBehaviour //controler
{
    public float AngleScale = 1;
    public float AngleMax = 360;

    public Text DataDisplay;
    public Text Velocity;
    public Text Velocity2;

    public GameObject BallPrefab;
    LineRenderer[] allLine;
    public List<LaunchClass> launches;
    public LaunchClass TallestLaunch    =>  launches.OrderByDescending(_ => Mathf.Abs(_.MidPosition().y)).First();//.Max(_ => _.MidPosition().y);
    public LaunchClass LowestLaunch     =>  launches.OrderByDescending(_ => Mathf.Abs(_.MidPosition().y)).Last();//.Max(_ => _.MidPosition().y);
    public LaunchClass LongestLaunch    => launches.OrderByDescending(_ => Mathf.Abs(_.EndPosition().x)).First();
    public LaunchClass ShortestLaunch   => launches.OrderByDescending(_ => Mathf.Abs(_.EndPosition().x)).Last();
    public LaunchClass MostAirTime      => launches.OrderByDescending(_ => _.MaxTime).First();//.Max(_ => _.MidPosition().y);
    public LaunchClass LeastAirTime     => launches.OrderByDescending(_ => _.MaxTime).Last();//.Max(_ => _.MidPosition().y);

    public LineRenderer arrow;
    public string GetData()
    {
        string s =
            $"Y:\n" +
            $"\tTallestLaunch: {TallestLaunch}" +
            $"\tLowestLaunch: {LowestLaunch}" +
            $"X:\n" +
            $"\tLongestLaunch: {LongestLaunch}" +
            $"\tShortestLaunch: {ShortestLaunch}" +
            $"Misc:\n" +
            $"\tMostAirTime: {MostAirTime}" +
            $"\tLeastAirTime: {LeastAirTime}" +
            $"";
        return s;
    }
    void Start() => ReCalulate();
    float v = Physics.gravity.y;
    public void ReCalulate(string vt = "")
    {
        //transcribe el string a float
        if (!string.IsNullOrEmpty(vt))
        {
            try
            {
                v = -float.Parse(vt);

            }
            catch
            {
                v = Physics.gravity.y;
            }
        }
        
        Velocity.text = v.ToString();
        Velocity2.text = v.ToString();

        //si hay launches en la filla borralos
        if (launches != null)
        {
            foreach (var l in launches)
            {
                Destroy(l.visual);
            }
            launches.Clear();
        }
        else
            launches = new List<LaunchClass>();
            
        //crea los launchers y manual launch
        for (float a = 0; a < AngleMax; a += AngleScale)
        {
            var l = new LaunchClass(a, v);
            l.visual = Instantiate(BallPrefab);
            l.visual.transform.position = l.StartPos;
            l.visual.GetComponent<ManualLaunch>().launch = l;
            launches.Add(l);
        }
                       
        GlobalVars.maxtime = MostAirTime.MaxTime;

        DataDisplay.text = GetData();

        arrow.SetPosition(0, Vector3.zero);
        arrow.SetPosition(1, Vector3.right * v);
        arrow.SetPosition(2, Vector3.right * v + Vector3.forward*360);

        allLine = FindObjectsOfType<LineRenderer>();
    }
    public void HideLineRender()
    {
        foreach(var l in allLine)
        {
            if (l != null)
            l.enabled = !l.enabled;
        }
    }
}
