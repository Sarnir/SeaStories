using UnityEngine;
using System.Collections;

[System.Serializable]
public class SkillDuration
{
    public float DurationInSeconds;

    public delegate void DurationCallback();
    DurationCallback OnDurationEnded;

    IEnumerator UpdateDuration()
    {
        yield return new WaitForSeconds(DurationInSeconds);

        if (OnDurationEnded != null)
        {
            OnDurationEnded();
            OnDurationEnded = null;
        }
    }

    public void Start(DurationCallback callback)
    {
        OnDurationEnded = callback;
        MonoBehaviorHost.Instance.StartCoroutine(UpdateDuration());
    }
}
