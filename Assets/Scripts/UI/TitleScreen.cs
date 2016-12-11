using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public Transform glass01;
    private Vector3 _glass01TargetPos = new Vector3(-12.51f, -2.37f, 0f);

    public Transform gourd01, gourd02, gourd03;
    private Vector3 _gourdRotation = new Vector3(0, 0, -20);

    public Transform chips;

    private void Update()
    {

        if (Vector3.Distance(glass01.position, _glass01TargetPos) < 0.3f)
        {
            _glass01TargetPos = new Vector3(-_glass01TargetPos.x, Random.Range(-2.37f, 2.37f), 0f);
        }
        glass01.position = Vector3.Lerp(glass01.position, _glass01TargetPos, Time.deltaTime * 1f);

        if(Vector3.Angle(gourd01.localEulerAngles, _gourdRotation) < 10f)
        {
            _gourdRotation = new Vector3(0, 0, -_gourdRotation.z);
        }
        gourd01.localEulerAngles = Vector3.Lerp(gourd01.localEulerAngles, _gourdRotation, Time.deltaTime);
        gourd02.localEulerAngles = Vector3.Lerp(gourd02.localEulerAngles, _gourdRotation, Time.deltaTime);
        gourd03.localEulerAngles = Vector3.Lerp(gourd03.localEulerAngles, _gourdRotation, Time.deltaTime);

        chips.Rotate(Vector3.forward * Time.deltaTime * 10f, Space.Self);
    }
}
