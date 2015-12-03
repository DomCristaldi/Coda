using UnityEngine;
using System.Collections;
using Coda;
using JBirdEngine;

[RequireComponent(typeof(Light))]
public class DemoRaveLight : MusicBehaviour {

    public float startHue;
    public float baseIntensity;
    public float extraIntensity;
    public float baseAngle;
    public float extraAngle;
    public float timePrecison;
    private float _huePerSecond;
    private Light _light;

    protected override void Awake () {
        _light = GetComponent<Light>();
        _light.intensity = baseIntensity;
        _light.color = new ColorHelper.ColorHSV(startHue, 1f, 1f, 1f).ToColor();
    }

    protected override void Start () {
        base.Start();
        DetermineHuePerSecond();
    }

    protected override void Update () {
        _light.color = _light.color.ToHSV().ShiftHue(_huePerSecond * Time.deltaTime).ToColor();
        if (Maestro.current.closestBeatIndex % 2 == 0) {
            float timeDiff;
            if (Maestro.current.IsOnBeat(timePrecison, out timeDiff)) {
                _light.intensity = baseIntensity + extraIntensity * ((timePrecison - timeDiff) / timePrecison);
                _light.spotAngle = baseAngle + extraAngle * ((timePrecison - timeDiff) / timePrecison);
            }
            else {
                _light.intensity = baseIntensity;
                _light.spotAngle = baseAngle;
            }
        }
    }

    public override void OnBeat () {
        DetermineHuePerSecond();
    }

    void DetermineHuePerSecond () {
        _huePerSecond = 30f / Maestro.current.timeUntilNextBeat;
    }

}
