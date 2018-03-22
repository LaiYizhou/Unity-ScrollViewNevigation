using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScrollViewNevigation : MonoBehaviour
{

    private ScrollRect scrollRect;
    private RectTransform viewport;
    private RectTransform content;

	// Use this for initialization
	void Start ()
	{

	    Init();
	    //Nevigate(content.GetChild(45).GetComponent<RectTransform>());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Init()
    {
        if (scrollRect == null)
        {
            scrollRect = this.GetComponent<ScrollRect>();
        }

        if (viewport == null)
        {
            viewport = this.transform.Find("Viewport").GetComponent<RectTransform>();
        }

        if (content == null)
        {
            content = this.transform.Find("Viewport/Content").GetComponent<RectTransform>();
        }
    }

    public void Nevigate(RectTransform item)
    {

        Vector3 itemCurrentLocalPostion = scrollRect.GetComponent<RectTransform>().InverseTransformVector(ConvertLocalPosToWorldPos(item));
        Vector3 itemTargetLocalPos = scrollRect.GetComponent<RectTransform>().InverseTransformVector(ConvertLocalPosToWorldPos(viewport));

        Vector3 diff = itemTargetLocalPos - itemCurrentLocalPostion;
        diff.z = 0.0f;

        var newNormalizedPosition = new Vector2(
            diff.x / (content.GetComponent<RectTransform>().rect.width - viewport.rect.width),
            diff.y / (content.GetComponent<RectTransform>().rect.height - viewport.rect.height)
            );

        newNormalizedPosition = scrollRect.GetComponent<ScrollRect>().normalizedPosition - newNormalizedPosition;

        newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
        newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);

        DOTween.To(() => scrollRect.GetComponent<ScrollRect>().normalizedPosition, x => scrollRect.GetComponent<ScrollRect>().normalizedPosition = x, newNormalizedPosition, 0.8f);
    }

    private Vector3 ConvertLocalPosToWorldPos(RectTransform target)
    {
        var pivotOffset = new Vector3(
            (0.5f - target.pivot.x) * target.rect.size.x,
            (0.5f - target.pivot.y) * target.rect.size.y,
            0f);

        var localPosition = target.localPosition + pivotOffset;

        return target.parent.TransformPoint(localPosition);
    }
}
