using UnityEngine;

namespace LinkThemAll.UI
{
    public class SafeArea : MonoBehaviour
    {
        private void Awake()
        {
            Rect safeArea = Screen.safeArea;
            
            RectTransform rectTransform = transform as RectTransform;

            Vector2 minAnchor = safeArea.position;
            Vector2 maxAnchor = minAnchor + safeArea.size;
            
            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;
            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;
            
            rectTransform.anchorMin = minAnchor;
            rectTransform.anchorMax = maxAnchor;
        }
    }
}