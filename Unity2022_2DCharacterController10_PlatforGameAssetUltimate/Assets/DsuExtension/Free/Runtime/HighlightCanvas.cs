using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Dsu.Extension
{
    [AddComponentMenu("Dsu/HighlightCanvas")]
    public class HighlightCanvas : MonoBehaviour
    {
#if UNITY_EDITOR

        public Color inactiveColor = new Color(1, 1, 1, .5f);
        public Color selectionColor = new Color(1, 1, 0);
        public Color childColor = new Color(1, 0, 1);
        public Color raycastColor = new Color(1, 0, 0, 1);
        public bool enableGizmos = true;

        private void OnDrawGizmos()
        {
            if (this.enableGizmos == false)
                return;

            var _selectionIsRectTransform = Selection.activeGameObject == null
                ? false : Selection.activeGameObject.GetComponent<RectTransform>() != null;
            var _rectTransforms = this.GetComponentsInChildren<RectTransform>();

            foreach (var _rectTransform in _rectTransforms) {
                this.DrawRectTransformInline(_rectTransform, this.inactiveColor);

                var _graphic = _rectTransform.GetComponent<Graphic>();
                //if (_graphic != null &&
                //    _graphic.raycastTarget == true)
                {
                    var _lightRaycastColor = this.raycastColor;
                    _lightRaycastColor.a = .1f;
                    // this.DrawRectTransformOutline (_rectTransform, this.raycastColor);
                    this.DrawRectTransformInline(_rectTransform, this.raycastColor);
                    this.DrawRectTransformFill(_rectTransform, _lightRaycastColor);
                }
            }

            if (_selectionIsRectTransform == true) {
                var _selectionRectTransformChildren =
                    Selection.activeGameObject.GetComponentsInChildren<RectTransform>();
                foreach (var _rectTransform in _selectionRectTransformChildren)
                    this.DrawRectTransformInline(_rectTransform, this.childColor);
                this.DrawRectTransformInline(
                    Selection.activeGameObject.GetComponent<RectTransform>(),
                    this.selectionColor);
            }
        }

        private void DrawRectTransformInline(RectTransform rectTransform, Color color)
        {
            if (this.IsActiveOrVisible(rectTransform) == false)
                return;
            var _corners = new Vector3[4];
            Gizmos.color = color;
            rectTransform.GetWorldCorners(_corners);
            for (var _i = 0; _i < 4; _i++)
                Gizmos.DrawLine(_corners[_i], _corners[_i == 3 ? 0 : _i + 1]);
        }

        private void DrawRectTransformOutline(RectTransform rectTransform, Color color)
        {
            if (this.IsActiveOrVisible(rectTransform) == false)
                return;
            var _corners = new Vector3[4];
            Gizmos.color = color;
            rectTransform.GetWorldCorners(_corners);
            _corners[0].x--;
            _corners[0].y--;
            _corners[1].x--;
            _corners[1].y++;
            _corners[2].x++;
            _corners[2].y++;
            _corners[3].x++;
            _corners[3].y--;
            for (var _i = 0; _i < 4; _i++)
                Gizmos.DrawLine(_corners[_i], _corners[_i == 3 ? 0 : _i + 1]);
        }

        private void DrawRectTransformFill(RectTransform rectTransform, Color color)
        {
            if (this.IsActiveOrVisible(rectTransform) == false)
                return;
            var _corners = new Vector3[4];
            Gizmos.color = color;
            rectTransform.GetWorldCorners(_corners);
            Gizmos.DrawCube(
                new Vector3(
                    _corners[0].x + ((_corners[2].x - _corners[0].x) / 2f),
                    _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f)
                ),
                new Vector3(
                    _corners[0].x - _corners[2].x,
                    _corners[0].y - _corners[1].y
                ));
        }

        private bool IsActiveOrVisible(RectTransform rectTransform)
        {
            return SceneVisibilityManager.instance.IsHidden(rectTransform.gameObject) == false && rectTransform.gameObject.activeSelf;
        }

#endif
    }
}
