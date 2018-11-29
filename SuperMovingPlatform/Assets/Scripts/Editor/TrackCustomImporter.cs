using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;


namespace SuperMovingPlatformEditor
{
    public class TrackCustomImporter : CustomTmxImporter
    {
        public override void TmxAssetImported(TmxAssetImportedArgs args)
        {
            InstantiateMovingPlatforms(args.ImportedSuperMap);
        }

        private void InstantiateMovingPlatforms(SuperMap superMap)
        {
            // Find all Tiled Objects in our map that are the moving platform type
            var platforms = superMap.GetComponentsInChildren<SuperObject>().Where(o => o.m_Type == "MovingPlatform");
            foreach (var platform in platforms)
            {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Sprites/Platform/MovingPlatform");
                GameObject.Instantiate(go, new Vector3(platform.m_X, platform.m_Y, 0), Quaternion.identity, platform.transform); 
                //PrefabUtility.InstantiatePrefab(go, )
            }
        }
    }
}
