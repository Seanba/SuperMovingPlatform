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

        private GameObject InstantiateResource(string name, SuperObject marker)
        {
            var go = Resources.Load("MovingPlatform");

            if (go != null)
            {
                return UnityEngine.Object.Instantiate(go, marker.transform) as GameObject;
            }
            else
            {
                m_ImportedArgs.AssetImporter.ReportError("MovingPlatform resource not found.");
            }

            return null;
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
                var goPlatfrom = InstantiateResource("MovingPlatform", marker);
                if (goPlatfrom == null)
                    continue;

                var compPlatform = goPlatfrom.GetComponent<MovingPlatformOnTrack>();

                // Initialize the data // fixit - properties and stuff
                //var props = marker.GetComponent<SuperCustomProperties>();
                //props.try

                foreach (var track in tracks)
                {
                    compPlatform.AssignTrackIfClose(track);
                }
            }
        }
    }
}
