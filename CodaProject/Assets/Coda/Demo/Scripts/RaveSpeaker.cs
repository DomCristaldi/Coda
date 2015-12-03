using UnityEngine;
using System.Collections;
using Coda;
using JBirdEngine;

public class RaveSpeaker : MusicBehaviour {

    public Material raveMat;
    public float startHue;
    public float baseSize;
    public float extraSize;
    public float centerRigidity;
    public float timePrecison;
    private float _huePerSecond;
    public Transform diaphragm;
    public Transform center;

    protected override void Start () {
        base.Start();
        raveMat.SetColor("_Color", new ColorHelper.ColorHSV(startHue, 1f, 1f, 1f).ToColor());
        DetermineHuePerSecond();
    }

    protected override void Update () {
        raveMat.SetColor("_Color", raveMat.color.ToHSV().ShiftHue(_huePerSecond * Time.deltaTime).ToColor());
        if (Maestro.current.closestBeatIndex % 2 == 0) {
            float timeDiff;
            if (Maestro.current.IsOnBeat(timePrecison, out timeDiff)) {
                diaphragm.localScale = Vector3.one * (baseSize + extraSize * ((timePrecison - timeDiff) / timePrecison));
                center.localScale = new Vector3 (1f * (baseSize + extraSize * ((timePrecison - timeDiff) / timePrecison) * (1f - centerRigidity)), 1f, 1f);
            }
            else {
                diaphragm.localScale = Vector3.one * baseSize;
                center.localEulerAngles = Vector3.one * baseSize;
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
