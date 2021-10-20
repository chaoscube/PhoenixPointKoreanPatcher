using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UABEAvalonia;
using UABEAvalonia.Plugins;
using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace PhoenixPointKoreanPatcher
{
    class MgrAsset
    {
        const string pathFile = "./../PhoenixPointWin64_Data/sharedassets0.assets";

        private AssetsManager        m_AssetsManager      = null;
        private AssetsFileInstance   m_AssetsFileInstance = null;
        private ClassDatabasePackage m_ClassDataPackage   = null;
        private ClassDatabaseFile    m_ClassDataFile      = null;

        Dictionary<AssetsFileInstance, List<AssetsReplacer>> m_fileToReplacer = new Dictionary<AssetsFileInstance, List<AssetsReplacer>>();

        public MgrAsset()
        {
            m_AssetsManager      = new AssetsManager();
            m_ClassDataPackage   = m_AssetsManager.LoadClassPackage("classdata.tpk");
            m_ClassDataFile      = m_AssetsManager.LoadClassDatabaseFromPackage("U2019.2.0f1");
            m_AssetsFileInstance = m_AssetsManager.LoadAssetsFile(pathFile, true);
        }

        ~MgrAsset()
        {
            m_AssetsManager.UnloadAll();
        }

        public void SingleExportDump(string nameFile)
        {
            string              managedFolder       = Path.Combine(Path.GetDirectoryName(m_AssetsFileInstance.path), "Managed");
            AssetFileInfoEx     assetFileInfoEx     = m_AssetsFileInstance.table.GetAssetInfo(nameFile);
            AssetTypeValueField assetTypeValueField = MonoDeserializer.GetMonoBaseField(m_AssetsManager, m_AssetsFileInstance, assetFileInfoEx, managedFolder);
            AssetImportExport   dumper              = new AssetImportExport();
            StreamWriter        outputFile          = new StreamWriter($"{nameFile}.txt");
            {
                dumper.DumpTextAsset(outputFile, assetTypeValueField);
            }
            outputFile.Close();
        }

        public void SingleImportDump(string nameFile)
        {
            if (nameFile != null && nameFile != string.Empty)
            {
                using (FileStream fs = File.OpenRead($"{nameFile}.txt"))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        AssetImportExport importer        = new AssetImportExport();
                        AssetFileInfoEx   assetFileInfoEx = m_AssetsFileInstance.table.GetAssetInfo(nameFile);

                        var MonoId  = AssetHelper.GetScriptIndex(m_AssetsFileInstance.file, assetFileInfoEx);
                        var PathId  = assetFileInfoEx.index;
                        var ClassId = assetFileInfoEx.curFileType;

                        byte[]? bytes = importer.ImportTextAsset(sr);

                        AssetsReplacer replacer = new AssetsReplacerFromMemory(0, PathId, (int)ClassId, MonoId, bytes);

                        if (m_fileToReplacer.ContainsKey(m_AssetsFileInstance) != true)
                        {
                            m_fileToReplacer[m_AssetsFileInstance] = new List<AssetsReplacer>();
                        }

                        for (int i = 0; i < m_fileToReplacer[m_AssetsFileInstance].Count; i++)
                        {
                            if (m_fileToReplacer[m_AssetsFileInstance][i].GetPathID() == PathId)
                            {
                                m_fileToReplacer[m_AssetsFileInstance].RemoveAt(i);
                                break;
                            }
                        }

                        m_fileToReplacer[m_AssetsFileInstance].Add(replacer);
                    }
                }
            }
        }

        public void SaveFile(string nameFile)
        {
            foreach (var kvp in m_fileToReplacer)
            {
                AssetsFileInstance file = kvp.Key;
                List<AssetsReplacer> replacers = kvp.Value;

                string filePath = nameFile;

                using (FileStream fs = File.OpenWrite(filePath))
                {
                    using (AssetsFileWriter w = new AssetsFileWriter(fs))
                    {
                        file.file.Write(w, 0, replacers, 0);
                    }
                }
            }
        }
    }
}
