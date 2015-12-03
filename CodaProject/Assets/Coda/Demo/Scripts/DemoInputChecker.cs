using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Coda;
using JBirdEngine;

public class DemoInputChecker : MonoBehaviour {

    public KeyCode inputKey;

    public MoreColors.BobRoss.ColorPalette timingPerfect;
    public MoreColors.BobRoss.ColorPalette timingExcellent;
    public float windowExcellent;
    public MoreColors.BobRoss.ColorPalette timingGood;
    public float windowGood;
    public MoreColors.BobRoss.ColorPalette timingOkay;
    public float windowOkay;
    public MoreColors.BobRoss.ColorPalette timingBad;

    public Text textField;
    public float textTime;

    private bool _gotBeat;
    private Timing _lastTiming;

    private Renderer _renderer;

    private Color _colorPerfect {
        get { return MoreColors.BobRoss.EnumToColor(timingPerfect); }
    }

    private Color _colorExcellent {
        get { return MoreColors.BobRoss.EnumToColor(timingExcellent); }
    }

    private Color _colorGood {
        get { return MoreColors.BobRoss.EnumToColor(timingGood); }
    }

    private Color _colorOkay {
        get { return MoreColors.BobRoss.EnumToColor(timingOkay); }
    }

    private Color _colorBad {
        get { return MoreColors.BobRoss.EnumToColor(timingBad); }
    }

    void Awake () {
        _renderer = GetComponent<Renderer>();
        _gotBeat = false;
    }

    void Update () {
        if (Maestro.current.closestBeatIndex % 2 == 0) {
            if (!_gotBeat) {
                if (Maestro.current.IsOnBeat()) {
                    if (Input.GetKeyDown(inputKey)) {
                        _renderer.material.color = _colorPerfect;
                        StartCoroutine(CreateTextDisplay(new MessageInfo("Perfect!", _colorPerfect)));
                        _gotBeat = true;
                    }
                }
                else if (Maestro.current.IsOnBeat(windowExcellent)) {
                    if (Input.GetKeyDown(inputKey)) {
                        _renderer.material.color = _colorExcellent;
                        StartCoroutine(CreateTextDisplay(new MessageInfo("Excellent!", _colorExcellent)));
                        _gotBeat = true;
                    }
                }
                else if (Maestro.current.IsOnBeat(windowGood)) {
                    if (Input.GetKeyDown(inputKey)) {
                        _renderer.material.color = _colorGood;
                        StartCoroutine(CreateTextDisplay(new MessageInfo("Good!", _colorGood)));
                        _gotBeat = true;
                    }
                }
                else if (Maestro.current.IsOnBeat(windowOkay)) {
                    if (Input.GetKeyDown(inputKey)) {
                        _renderer.material.color = _colorOkay;
                        StartCoroutine(CreateTextDisplay(new MessageInfo("Okay!", _colorOkay)));
                        _gotBeat = true;
                    }
                }
                else if (Input.GetKeyDown(inputKey)) {
                    _renderer.material.color = _colorBad;
                    StartCoroutine(CreateTextDisplay(new MessageInfo("Bad!", _colorBad)));
                    _gotBeat = true;
                }
            }
            else if (_lastTiming == Timing.late) {
                Maestro.current.IsOnBeat(out _lastTiming);
                if (_lastTiming == Timing.early) {
                    _gotBeat = false;
                }
            }
            else {
                Maestro.current.IsOnBeat(out _lastTiming);
            }
        }
        else if (Input.GetKeyDown(inputKey)) {
            _renderer.material.color = _colorBad;
            StartCoroutine(CreateTextDisplay(new MessageInfo("Bad!", _colorBad)));
            _gotBeat = true;
        }
    }

    class MessageInfo {
        public string message;
        public Color color;
        public MessageInfo (string m, Color c) {
            message = m;
            color = c;
        }
    }

    IEnumerator CreateTextDisplay (MessageInfo info) {
        textField.text = info.message;
        textField.color = info.color;
        yield return new WaitForSeconds(textTime);
        textField.text = "";
        yield break;
    }

}
