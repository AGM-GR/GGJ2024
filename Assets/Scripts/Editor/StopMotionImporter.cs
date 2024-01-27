using UnityEditor;
using UnityEngine;

public class StopMotionImporter : AssetPostprocessor
{
    void OnPreprocessAnimation()
    {
        if (assetPath.ToLower().Contains("_stopmotion"))
        {
            Debug.Log("Setting Stop Motion animation import");
            ModelImporter modelImporter = assetImporter as ModelImporter;
            modelImporter.resampleCurves = false;
            //modelImporter.animationCompression = ModelImporterAnimationCompression.Off;
        }
    }

    void OnPostprocessAnimation(GameObject root, AnimationClip clip)
    {
        if (assetPath.ToLower().Contains("_stopmotion"))
        {
            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
            foreach (EditorCurveBinding curveBinding in curveBindings)
            {
                AnimationCurve clipCurve = AnimationUtility.GetEditorCurve(clip, curveBinding);

                for (int i = 0; i < clipCurve.keys.Length; i++)
                {
                    AnimationUtility.SetKeyLeftTangentMode(clipCurve, i, AnimationUtility.TangentMode.Constant);
                    AnimationUtility.SetKeyRightTangentMode(clipCurve, i, AnimationUtility.TangentMode.Constant);
                }

                AnimationUtility.SetEditorCurve(clip, curveBinding, clipCurve);
            }

            Debug.Log("Animation Clip: " + clip.name + " - Imported as Stop Motion Clip");
        }
    }
}