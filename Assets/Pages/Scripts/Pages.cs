using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pages
{
    class Pages : MonoBehaviour, IPointerClickHandler
    {
        public Page[] Elements;
        public float PageSpacing = 0.01f;
        public float PageGap = 0.1f;
        public Vector2 PageBorders = new Vector2(0,0);
        public float FlipSpeed = 1;

        private bool fliped = false;

        public void OnEnable()
        {
            float s = PageBorders.x;
            foreach (var page in Elements)
            {
                page.UpdateProgress(s, false);
                s += PageSpacing;
            }
        }

        public void Flip()
        {
            float s = PageBorders.y;
            int index = 0;
            foreach (var page in Elements)
            {
                page.Direction = !page.Direction;
                page.DOKill();
                var targetProgress = s ;//page.Direction ? 1 : 0;
                DOTween.To(() => page.Progress, f => page.UpdateProgress(f, page.Direction), targetProgress, FlipSpeed)
                    .SetSpeedBased(true)
                    .SetDelay(index++ * PageGap)
                    .SetTarget(page);
                s -= PageSpacing;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Flip");
            Flip();
        }
    }
}