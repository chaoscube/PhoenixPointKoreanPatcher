using System;

namespace PhoenixPointKoreanPatcher
{
    class Program
    {
        static MgrAsset mgrAsset = new MgrAsset();

        static void Main(string[] args)
        {
            Translate("I2Languages");
            Translate("I2Languages_GeoEvents");
            Translate("I2Languages_Research");
            Translate("I2Languages_CharNames");
            Translate("I2Languages_GeoHavens");
            Translate("I2Languages_Saber");
            Translate("I2Languages_Mod");

            mgrAsset.SaveFile("sharedassets0.assets");
        }

        static void Translate(string nameFile)
        {
            StringTable stringTable = new StringTable();

            mgrAsset.SingleExportDump(nameFile);
            stringTable.ExportStringTalbe(nameFile);
            stringTable.ImportStringTalbe(nameFile);
            mgrAsset.SingleImportDump(nameFile);
        }
    }
}
