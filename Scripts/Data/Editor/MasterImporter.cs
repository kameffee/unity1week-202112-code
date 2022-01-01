#if UNITY_EDITOR
using Unity1week202112.Data.Master;
using UnityEditor;
using UnityEngine;

namespace Unity1week202112.Data.Editor
{
    /// <summary>
    /// マスターデータ更新
    /// </summary>
    public class MasterImporter : AssetPostprocessor
    {
        private static readonly string OutputFolder = "Assets/Application/Resources/MasterData";

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (string str in importedAssets)
            {
                if (str.IndexOf($"/ScenarioMaster.csv") != -1)
                {
                    TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                    string assetfile = OutputFolder + "/ScenarioMaster.asset";
                    ScenarioMaster master = AssetDatabase.LoadAssetAtPath<ScenarioMaster>(assetfile);
                    if (master == null)
                    {
                        master = ScenarioMaster.Create();
                        AssetDatabase.CreateAsset(master, assetfile);
                    }

                    master._data = (CSVSerializer.Deserialize<ScenarioData>(data.text));

                    EditorUtility.SetDirty(master);
                    AssetDatabase.SaveAssets();
                    ReimportLog("Reimported Asset: " + str);
                }

                if (str.IndexOf($"/MapMaster.csv") != -1)
                {
                    TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                    string assetfile = OutputFolder + "/MapMaster.asset";
                    MapMaster master = AssetDatabase.LoadAssetAtPath<MapMaster>(assetfile);
                    if (master == null)
                    {
                        master = ScriptableObject.CreateInstance<MapMaster>();
                        AssetDatabase.CreateAsset(master, assetfile);
                    }

                    master._data = (CSVSerializer.Deserialize<MapData>(data.text));

                    EditorUtility.SetDirty(master);
                    AssetDatabase.SaveAssets();
                    ReimportLog("Reimported Asset: " + str);
                }

                if (str.IndexOf($"/EffectTypeMaster.csv") != -1)
                {
                    TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                    string assetfile = OutputFolder + "/EffectTypeMaster.asset";
                    var master = AssetDatabase.LoadAssetAtPath<EffectTypeMaster>(assetfile);
                    if (master == null)
                    {
                        master = ScriptableObject.CreateInstance<EffectTypeMaster>();
                        AssetDatabase.CreateAsset(master, assetfile);
                    }

                    master._data = (CSVSerializer.Deserialize<EffectTypeData>(data.text));

                    EditorUtility.SetDirty(master);
                    AssetDatabase.SaveAssets();
                    ReimportLog("Reimported Asset: " + str);
                }

                if (str.IndexOf($"/PlayerDeckMaster.csv") != -1)
                {
                    TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                    string assetfile = OutputFolder + "/PlayerDeckMaster.asset";
                    var master = AssetDatabase.LoadAssetAtPath<PlayerDeckMaster>(assetfile);
                    if (master == null)
                    {
                        master = ScriptableObject.CreateInstance<PlayerDeckMaster>();
                        AssetDatabase.CreateAsset(master, assetfile);
                    }

                    master._data = (CSVSerializer.Deserialize<PlayerDeckData>(data.text));

                    EditorUtility.SetDirty(master);
                    AssetDatabase.SaveAssets();
                    ReimportLog("Reimported Asset: " + str);
                }

                if (str.IndexOf($"/CardLotteryMaster.csv") != -1)
                {
                    TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                    string assetfile = OutputFolder + "/CardLotteryMaster.asset";
                    var master = AssetDatabase.LoadAssetAtPath<CardLotteryMaster>(assetfile);
                    if (master == null)
                    {
                        master = ScriptableObject.CreateInstance<CardLotteryMaster>();
                        AssetDatabase.CreateAsset(master, assetfile);
                    }

                    master._data = (CSVSerializer.Deserialize<CardLotteryData>(data.text));

                    EditorUtility.SetDirty(master);
                    AssetDatabase.SaveAssets();
                    ReimportLog("Reimported Asset: " + str);
                }
            }
        }

        public static void ReimportLog(string str)
        {
#if DEBUG_LOG || UNITY_EDITOR
            Debug.Log("Reimported Asset: " + str);
#endif
        }
    }
}

#endif
