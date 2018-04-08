using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WorldSpaceUI : MonoBehaviour
{
    Text textPrefab;

	void Start ()
    {
        textPrefab = GetComponentInChildren<Text>();
	}
	
	public void SpawnText(Vector3 position, string text)
    {
        var newText = GameObject.Instantiate<Text>(textPrefab, this.transform);
        newText.name = "pickupText";
        newText.transform.position = position;
        newText.enabled = true;
        newText.text = text;

        newText.DOFade(0f, 2f);
        newText.transform.DOMoveY(position.y + 5f, 2.5f).OnComplete(() => Destroy(newText.gameObject));
    }
}
