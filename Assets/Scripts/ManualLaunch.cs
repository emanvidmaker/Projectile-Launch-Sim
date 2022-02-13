using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LaunchClass { //model
    public LaunchClass(float angle, float velocity)
    {
        this.angle = angle;
        this.velocity = velocity;
        StartPos = new Vector3(0, 0, angle);
    }
    public static float g => Physics.gravity.y; //gravity

    //Variables
    public float angle, velocity;
    public GameObject visual;
    public Vector3 StartPos;
   
    //paso 1
    public Vector3 VelocityVector  {get { return (Quaternion.Euler(0, 0, -angle) * Vector3.right * velocity); }    }
    public Vector3 PositionAtTime(float time)
    {
        Vector3 pos = Vector3.zero;
        //paso 3
        pos.y = (VelocityVector.y * time + (g * (time * time)) / 2); //Ymax
        //paso 5
        pos.x = VelocityVector.x * time; //Xmax

        pos.z = StartPos.z; //stay in the same z position; 

        return pos;
    }
    public Vector3 EndPosition() => PositionAtTime(MaxTime);    
    public Vector3 MidPosition() => PositionAtTime(HalfTime);
    public float HalfTime    {
        get{
            float v = -VelocityVector.y / g; //Paso 2
            return (v < 0 ? 0 : v); //time cant be negative
        }
    }
    public float MaxTime { get { return (HalfTime * 2); } } //paso 4

    public Vector3 PositionAtPercent(float percent) => PositionAtTime(percent * this.MaxTime);

 
    public Vector3 PositionAtTimeWithFloorDetection(float time)
    {
       if (GlobalVars.UseFloor && time > MaxTime)
           return EndPosition();
       else
           return PositionAtTime(time);
    }
    public override string ToString()
    {
        string s =
            $"\t\t<color=#{ ColorUtility.ToHtmlStringRGBA(GetCustomColor()) }>\n" +
            $"\t\tAngle: {angle} \n" +
            $"\t\tVelocity: {velocity} \n" +
            $"\t\tAirTime: {MaxTime} \n" +
             "\t\tHighest Position: " + MidPosition().ToString() + "\n" +
             "\t\tLanding Position: " + EndPosition().ToString() + "\n" +
             "</color>";
        return s;
    }
    public Color GetCustomColor(float v = 1f) //Color.HSVToRGB(angle / 180f, 1, v)
    {
        Color c = Color.HSVToRGB((angle / 180f)%1, angle > 180F ? 0.75F:1F, v);
        c.a = 0.5f;
        return c;
    }
}

public class ManualLaunch : MonoBehaviour //view
{
    //info displays
    public LineRenderer trejectoryRenderer;
    public LineRenderer trejectoryAfterFloorHitRenderer;
    public MeshRenderer meshRenderer;
    public TextMesh text;


    public LaunchClass launch;
    private void Start() => Recalculate();
    private void Update() => transform.position = launch.PositionAtTimeWithFloorDetection(GlobalVars.Timer);
    public void Recalculate()
    {
        if (launch == null) launch = new LaunchClass(transform.position.z, Physics.gravity.y);
        ShowText();
        DrawLines();
    }
    void ShowText()
    {
        if (launch.angle % 45 == 0)
        {
            text.text = launch.angle.ToString();
        }
        else
        {
            text.text = "";
        }

    }
    void DrawLines() {
        int resolution = 20;
        trejectoryRenderer.positionCount = resolution + 1;
        Vector3 v = Vector3.zero;
        for (int i = 0; i <= resolution; i++)
        {
            v = launch.PositionAtPercent((float)i / ((float)resolution));
            trejectoryRenderer.SetPosition(i, v);
        }
        trejectoryRenderer.startColor = launch.GetCustomColor();
        trejectoryRenderer.endColor = launch.GetCustomColor(0.65f);
        //////////////////////////////////////////////////////////////
        trejectoryAfterFloorHitRenderer.positionCount = resolution + 1;
        for (int i = 0; i <= resolution; i++)
        {
            v = launch.PositionAtPercent(1f + ((float)i / ((float)resolution)));
            trejectoryAfterFloorHitRenderer.SetPosition(i, v);
        }
        trejectoryAfterFloorHitRenderer.startColor = launch.GetCustomColor(0.5f);
        trejectoryAfterFloorHitRenderer.endColor = launch.GetCustomColor(0.25f);

        meshRenderer.material.SetColor("_BaseColor", launch.GetCustomColor());
        meshRenderer.material.SetColor("_EmissionColor", launch.GetCustomColor());
    }
}

