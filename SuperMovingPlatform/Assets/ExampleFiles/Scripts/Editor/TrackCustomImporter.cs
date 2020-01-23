using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;

namespace SuperMovingPlatform
{
    public class TrackCustomImporter : CustomTmxImporter
    {
        private TmxAssetImportedArgs m_ImportedArgs;

        public override void TmxAssetImported(TmxAssetImportedArgs args)
        {
            m_ImportedArgs = args;
            InstantiateMovingPlatforms();
        }

        private void InstantiateMovingPlatforms()
        {
            // Find all Tiled Objects in our map that are the moving platform type
            var platforms = m_ImportedArgs.ImportedSuperMap.GetComponentsInChildren<SuperObject>().Where(o => o.m_Type == "MovingPlatform");

            // Find all tracks in our maps. These are edge colliders under the "Track" layer
            var tracks = m_ImportedArgs.ImportedSuperMap.GetComponentsInChildren<EdgeCollider2D>().Where(c => c.gameObject.layer == LayerMask.NameToLayer("Track"));

            foreach (var marker in platforms)
            {
                // Instantiate the sprite for our moving platform
                var compPlatform = InstantiateMovingPlatform("MovingPlatform", marker);
                if (compPlatform != null)
                {
                    // Find a track for our platform to be attached to
                    foreach (var track in tracks)
                    {
                        if (compPlatform.AssignTrackIfClose(track))
                        {
                            break;
                        }
                    }
                }
            }
        }

        private MovingPlatformOnTrack InstantiateMovingPlatform(string name, SuperObject marker)
        {
            var go = Resources.Load("MovingPlatform");
            if (go == null)
            {
                m_ImportedArgs.AssetImporter.ReportError("MovingPlatform resource not found.");
                return null;
            }

            // Add our moving platform to the map
            var goPlatform = UnityEngine.Object.Instantiate(go, marker.transform) as GameObject;

            // Custom properties can control platform speed and inital direction
            var compPlatform = goPlatform.GetComponent<MovingPlatformOnTrack>();
            compPlatform.m_Speed = marker.gameObject.GetSuperPropertyValueFloat("Speed", 64.0f);
            compPlatform.m_InitialDirection.x = marker.gameObject.GetSuperPropertyValueFloat("Direction_x", 1.0f);
            compPlatform.m_InitialDirection.y = marker.gameObject.GetSuperPropertyValueFloat("Direction_y", 1.0f);

            return compPlatform;
        }
    }
}
