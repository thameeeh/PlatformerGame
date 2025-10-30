using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dsu.Extension
{
    [AddComponentMenu("Dsu/HighlightThis")]
    public class HighlightThis : MonoBehaviour
    {
        public bool showLabel = true;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!showLabel) return;

            // �۾� ��ġ ����: Pivot �Ʒ���
            Vector3 labelPosition = transform.position + Vector3.down * 0.1f;

            // �ڵ� ���� �� ��Ÿ�� ����
            GUIStyle style = new GUIStyle();
            style.normal.background = Texture2D.grayTexture;
            style.normal.textColor = Color.red;
            style.alignment = TextAnchor.UpperCenter;
            style.fontSize = 10;

            Handles.Label(labelPosition, $"{gameObject.name}", style);
        }
#endif
    }
}
