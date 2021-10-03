using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public Level t;
    public void OnSceneGUI()
    {
        t = target as Level;

        SpawnPointHandles();
        LevelEndHandles();
        LevelBoundsHandles();
        StarsHandles();
    }

    private void SpawnPointHandles()
    {
        if (t.levelStart != null)
        {
            t.levelStart.rotation = Quaternion.Euler(0, 90, 0);
            t.levelStart.position = Handles.PositionHandle(t.levelStart.position, Quaternion.identity);
        }
    }

    private void LevelEndHandles()
    {
        if (t.levelEnd != null)
        {
            t.levelEnd.position = Handles.PositionHandle(t.levelEnd.position, Quaternion.identity);
            t.levelEndBounds.center = t.levelEnd.position + Vector3.up * t.levelEndBounds.extents.y;
            t.levelEndBounds.size = Handles.ScaleHandle(t.levelEndBounds.size, t.levelEndBounds.center, Quaternion.identity, 1);
            t.levelEndBounds.size = Vector3.Max(t.levelEndBounds.size, Vector3.one);
        }
    }

    private void StarsHandles()
    {
        foreach (var star in t.stars)
        {
            star.position = Handles.PositionHandle(star.position, Quaternion.identity);
        }
    }

    private void LevelBoundsHandles()
    {
        if (t.levelEnd != null && t.levelStart != null)
        {
            Vector3 levelSize = Handles.ScaleHandle(t.levelBounds.size, t.levelBounds.center - Vector3.up * 2, Quaternion.identity, 1);
            Vector3 levelPosition = Handles.PositionHandle(t.levelBounds.center, Quaternion.identity);
            float xEnd = t.levelEndBounds.center.x + t.levelEndBounds.extents.x;
            float xStart = t.levelStart.position.x - 2;

            levelSize.x = Mathf.Abs(xEnd - xStart);
            levelSize.y = Mathf.Max(levelSize.y, 1);
            levelSize.z = 5;
            levelPosition.x = (xEnd + xStart) / 2;
            t.levelBounds.center = levelPosition;
            t.levelBounds.size = levelSize;
        }
    }
}
