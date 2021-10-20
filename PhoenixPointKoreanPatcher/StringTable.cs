using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixPointKoreanPatcher
{
    class StringPair
    {
        public StringPair(string strOrigin, string strTranslated)
        {
            _Origin     = strOrigin;
            _Translated = strTranslated;
        }

        public string _Origin     = string.Empty;
        public string _Translated = string.Empty;
    }

    class StringTable
    {
        private Dictionary<string, StringPair> m_tableString = new Dictionary<string, StringPair>();

        public void LoadTableTranslated(string nameFile)    // tsv파일에서 키/원문/번역문 을 읽어온다
        {
            m_tableString.Clear();

            if (File.Exists($"{nameFile}.tsv"))
            {
                string[] Buffer = File.ReadAllLines($"{nameFile}.tsv");

                for (int i = 0; i < Buffer.Length; i++)
                {
                    string[] strTemp = Buffer[i].Split("\t");

                    m_tableString[strTemp[0]] = new StringPair(strTemp[1], strTemp[2]);
                }
            }
        }

        public void ExportStringTalbe(string nameFile)  // 에셋 파일을 덤프한 txt파일에서 키/원문/"번역문" 을 추출해 tsv에 저장한다.
        {
            LoadTableTranslated(nameFile);

            string[] Buffer = File.ReadAllLines($"{nameFile}.txt");

            int iLanguageIndex = GetLanguageIndex(Buffer);
            int iOrigin        = 6;
            int iTranslated    = -1;

            switch (iLanguageIndex)
            {
                case 0: iTranslated =  6; break;
                case 1: iTranslated =  8; break;
                case 2: iTranslated = 10; break;
                case 3: iTranslated = 12; break;
                case 4: iTranslated = 14; break;
                case 5: iTranslated = 16; break;
                case 6: iTranslated = 18; break;
                case 7: iTranslated = 20; break;
                case 8: iTranslated = 22; break;
                case 9: iTranslated = 24; break;
            }

            for (int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i].Contains("1 string Term = "))
                {
                    string strKey        = GetText(Buffer[i]);
                    string strOrigin     = GetText(Buffer[i + iOrigin]);
                    string strTranslated = GetText(Buffer[i + iTranslated]);

                    if (strOrigin == string.Empty)
                    {
                        Console.WriteLine($"Value Empty : {strKey}");
                    }
                    else
                    {
                        if (m_tableString.ContainsKey(strKey))
                        {
                            if (m_tableString[strKey]._Origin != strOrigin)
                            {
                                m_tableString[strKey] = new StringPair(strOrigin, "번역문");
                            }
                        }
                        else
                        {
                            m_tableString[strKey] = new StringPair(strOrigin, "번역문");
                        }
                    }
                }
            }

            StreamWriter writer = new StreamWriter($"{nameFile}.tsv", false);

            foreach (var item in m_tableString)
            {
                writer.WriteLine($"{item.Key}\t{item.Value._Origin}\t{item.Value._Translated}");
            }

            writer.Close();
        }

        public void ImportStringTalbe(string nameFile)  // tsv에서 키/원문/번역문 을 추출해 에셋 파일을 덤프한 txt파일에 저장한다.
        {
            LoadTableTranslated(nameFile);

            string[] Buffer = File.ReadAllLines($"{nameFile}.txt");

            int iLanguageIndex = GetLanguageIndex(Buffer);
            int iOrigin        = 6;
            int iTranslated    = -1;

            switch (iLanguageIndex)
            {
                case 0: iTranslated =  6; break;
                case 1: iTranslated =  8; break;
                case 2: iTranslated = 10; break;
                case 3: iTranslated = 12; break;
                case 4: iTranslated = 14; break;
                case 5: iTranslated = 16; break;
                case 6: iTranslated = 18; break;
                case 7: iTranslated = 20; break;
                case 8: iTranslated = 22; break;
                case 9: iTranslated = 24; break;
            }

            for (int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i].Contains("1 string Term = "))
                {
                    string strKey        = GetText(Buffer[i]);
                    string strOrigin     = GetText(Buffer[i + iOrigin]);
                    string strTranslated = GetText(Buffer[i + iTranslated]);

                    if (m_tableString.ContainsKey(strKey))
                    {
                        if (m_tableString[strKey]._Origin == strOrigin)
                        {
                            string strTemp = Buffer[i + iTranslated];

                            int iStart = strTemp.IndexOf("\"") + 1;

                            strTemp = strTemp.Substring(0, iStart);

                            if (m_tableString[strKey]._Translated != "번역문")
                            {
                                Buffer[i + iTranslated] = $"{strTemp}{m_tableString[strKey]._Translated}\"";
                            }
                            else
                            {
                                Buffer[i + iTranslated] = $"{strTemp}{strKey}\"";
                            }
                        }
                    }
                }
            }

            File.WriteAllLines($"{nameFile}.txt", Buffer);
        }

        int GetLanguageIndex(string[] Buffer)
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                if (Buffer[i].Contains("1 string Name = \"Chinese (Simplified)\"") || Buffer[i].Contains("1 string Name = \"Chinese\""))
                {
                    int    iTemp = 0;
                    string temp  = Buffer[i - 2];

                    temp = temp.Replace(" ", "");
                    temp = temp.Replace("[", "");
                    temp = temp.Replace("]", "");

                    if (int.TryParse(temp, out iTemp))
                    {
                        return iTemp;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            return -1;
        }

        // 읽어들인 문자열에서 따움표 사이의 번역 키, 번역 문장을 뽑하내기 위해 사용
        private string GetText(string strValue)
        {
            int iStart = strValue.IndexOf    ("\"") + 1;  // 따옴표 제거를 위해 따옴표 다음 문자 부터 시작
            int iLast  = strValue.LastIndexOf("\"");

            if (iStart >= iLast)
            {
                return string.Empty;
            }
            return strValue.Substring(iStart, iLast - iStart);
        }
    }
}