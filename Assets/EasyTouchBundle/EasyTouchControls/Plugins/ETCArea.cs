using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ETCArea : MonoBehaviour
{
	public enum AreaPreset { Choose, TopLeft, TopRight, BottomLeft, BottomRight };

	public bool show;

	#region Constructeur

	public ETCArea () => show = true;

	#endregion

	#region MonoBehaviour Callback

	public void Awake () => GetComponent<Image> ().enabled = show;

	#endregion

	public void ApplyPreset (AreaPreset preset)
	{
		RectTransform parent = transform.parent.GetComponent<RectTransform> ();

		switch (preset)
		{
			case AreaPreset.TopRight:
			this.RectTransform ().anchoredPosition = new Vector2 (parent.rect.width / 4f, parent.rect.height / 4f);
			this.RectTransform ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, parent.rect.width / 2f);
			this.RectTransform ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, parent.rect.height / 2f);

			this.RectTransform ().anchorMin = new Vector2 (1, 1);
			this.RectTransform ().anchorMax = new Vector2 (1, 1);
			this.RectTransform ().anchoredPosition = new Vector2 (-this.RectTransform ().sizeDelta.x / 2f, -this.RectTransform ().sizeDelta.y / 2f);

			break;

			case AreaPreset.TopLeft:
			this.RectTransform ().anchoredPosition = new Vector2 (-parent.rect.width / 4f, parent.rect.height / 4f);
			this.RectTransform ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, parent.rect.width / 2f);
			this.RectTransform ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, parent.rect.height / 2f);

			this.RectTransform ().anchorMin = new Vector2 (0, 1);
			this.RectTransform ().anchorMax = new Vector2 (0, 1);
			this.RectTransform ().anchoredPosition = new Vector2 (this.RectTransform ().sizeDelta.x / 2f, -this.RectTransform ().sizeDelta.y / 2f);

			break;

			case AreaPreset.BottomRight:
			this.RectTransform ().anchoredPosition = new Vector2 (parent.rect.width / 4f, -parent.rect.height / 4f);
			this.RectTransform ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, parent.rect.width / 2f);
			this.RectTransform ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, parent.rect.height / 2f);

			this.RectTransform ().anchorMin = new Vector2 (1, 0);
			this.RectTransform ().anchorMax = new Vector2 (1, 0);
			this.RectTransform ().anchoredPosition = new Vector2 (-this.RectTransform ().sizeDelta.x / 2f, this.RectTransform ().sizeDelta.y / 2f);

			break;

			case AreaPreset.BottomLeft:
			this.RectTransform ().anchoredPosition = new Vector2 (-parent.rect.width / 4f, -parent.rect.height / 4f);
			this.RectTransform ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, parent.rect.width / 2f);
			this.RectTransform ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, parent.rect.height / 2f);

			this.RectTransform ().anchorMin = new Vector2 (0, 0);
			this.RectTransform ().anchorMax = new Vector2 (0, 0);
			this.RectTransform ().anchoredPosition = new Vector2 (this.RectTransform ().sizeDelta.x / 2f, this.RectTransform ().sizeDelta.y / 2f);

			break;
		}
	}
}