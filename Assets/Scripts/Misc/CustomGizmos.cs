using UnityEditor;
using UnityEngine;

public class CustomGizmos : MonoBehaviour {
    public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {
#if UNITY_EDITOR
        Gizmos.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
#endif
    }

    public static void DrawWireCircle(Vector3 pos, Vector3 normal, float radius) {
#if UNITY_EDITOR
        Handles.matrix = Gizmos.matrix;
        Color drawColor = Gizmos.color;
        Handles.color = drawColor;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        Handles.DrawWireDisc(pos, normal, radius);
        drawColor.a *= 0.2f;
        Handles.color = drawColor;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
        Handles.DrawWireDisc(pos, normal, radius);
#endif
    }

    public static void DrawCircle(Vector3 pos, Vector3 normal, float radius) {
#if UNITY_EDITOR
        Handles.matrix = Gizmos.matrix;
        Color drawColor = Gizmos.color;
        Handles.color = drawColor;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        Handles.DrawSolidDisc(pos, normal, radius);
        drawColor.a *= 0.2f;
        Handles.color = drawColor;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
        Handles.DrawSolidDisc(pos, normal, radius);
#endif
    }

    public static void DrawWireSphere(Vector3 pos, float radius) {
#if UNITY_EDITOR
        Gizmos.DrawWireSphere(pos, radius);
        DrawWireCircle(pos, -SceneView.lastActiveSceneView.camera.transform.forward, radius);
#endif
    }

    public static void DrawWireCapsule(Vector3 pos, float radius, float height) {
#if UNITY_EDITOR
        Handles.matrix = Gizmos.matrix;
        Color drawColor = Gizmos.color;
        Handles.color = drawColor;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        HandlesWireCapsule(pos, radius, height);
        drawColor.a *= 0.2f;
        Handles.color = drawColor;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
        HandlesWireCapsule(pos, radius, height);

#endif
    }

    public static void HandlesWireCapsule(Vector3 pos, float radius, float height) {
#if UNITY_EDITOR
        Vector3 baseCenter = new Vector3(pos.x, pos.y + radius, pos.z);
        Vector3 topCenter = new Vector3(pos.x, pos.y + height - radius, pos.z);
        Handles.DrawWireArc(baseCenter, Vector3.forward, Vector3.left, 180f, radius);
        Handles.DrawWireArc(baseCenter, Vector3.right, Vector3.forward, 180f, radius);
        Handles.DrawWireDisc(baseCenter, Vector3.up,radius);
        Handles.DrawWireArc(topCenter, Vector3.forward, Vector3.right, 180f, radius);
        Handles.DrawWireArc(topCenter, Vector3.right, Vector3.back, 180f, radius);
        Handles.DrawWireDisc(topCenter, Vector3.up, radius);
        Handles.DrawLine(baseCenter + Vector3.forward * radius, topCenter + Vector3.forward * radius);
        Handles.DrawLine(baseCenter + Vector3.back * radius, topCenter + Vector3.back * radius);
        Handles.DrawLine(baseCenter + Vector3.right * radius, topCenter + Vector3.right * radius);
        Handles.DrawLine(baseCenter + Vector3.left * radius, topCenter + Vector3.left * radius);
#endif
    }

    public static void DrawFrustum(Vector3 pos, Vector3 forward, Vector3 right, Vector3 up, Vector3 size, float baseMultiplier = 1.5f) {
        Vector3 baseSize = new Vector3(size.x, size.y, 0f);
        Gizmos.DrawWireCube(pos, baseSize);
        Gizmos.DrawWireCube(pos + forward * size.z, baseSize * baseMultiplier);
        Gizmos.DrawLine(pos + right * baseSize.x / 2f + up * baseSize.y / 2f, pos + forward * size.z + right * (baseSize.x / 2f) * baseMultiplier + up * (baseSize.y / 2f) * baseMultiplier);
        Gizmos.DrawLine(pos - right * baseSize.x / 2f + up * baseSize.y / 2f, pos + forward * size.z - right * (baseSize.x / 2f) * baseMultiplier + up * (baseSize.y / 2f) * baseMultiplier);
        Gizmos.DrawLine(pos - right * baseSize.x / 2f - up * baseSize.y / 2f, pos + forward * size.z - right * (baseSize.x / 2f) * baseMultiplier - up * (baseSize.y / 2f) * baseMultiplier);
        Gizmos.DrawLine(pos + right * baseSize.x / 2f - up * baseSize.y / 2f, pos + forward * size.z + right * (baseSize.x / 2f) * baseMultiplier - up * (baseSize.y / 2f) * baseMultiplier);
    }

    public static void DrawArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius) {
#if UNITY_EDITOR
        Handles.matrix = Gizmos.matrix;
        Color drawColor = Gizmos.color;
        Handles.color = drawColor;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
        Handles.DrawWireArc(center, normal, from, angle, radius);
        drawColor.a *= 0.2f;
        Handles.color = drawColor;
        Handles.DrawSolidArc(center, normal, from, angle, radius);

        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
        Handles.DrawWireArc(center, normal, from, angle, radius);
        drawColor.a *= 0.2f;
        Handles.color = drawColor;
        Handles.DrawSolidArc(center, normal, from, angle, radius);
#endif
    }
}
