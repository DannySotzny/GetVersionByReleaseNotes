using System;
using System.IO;

namespace GetVersionByReleaseNotes
{
    public static class ReleaseNotesVersionTool
    {
        /// <summary>
        /// Extract the version number by Release Notes
        /// </summary>
        /// <param name="filename">the Release Notes MarkDown file</param>
        /// <returns></returns>
        public static string GetVersion(string filename)
        {
            var fi = new FileInfo(filename);
            if (!fi.Exists) { throw new ArgumentException($"File {filename} doesn´t exist."); }

            var content = File.ReadAllLines(fi.FullName);
            var hauptversion = "";

            for (int index = 0; index < content.Length; index++)
            {
                var row = content[index];
                if (string.IsNullOrWhiteSpace(row))
                {
                    continue;
                }

                var tmp = row;
                tmp = tmp.Replace("#", "");
                if (tmp.IndexOf('-') > 0)
                {
                    tmp = tmp.Substring(0, tmp.IndexOf('-'));
                }

                tmp = tmp.Trim();

                // z.B. 1.3.1. <--can´t parse them - so remove last dot
                if (tmp.EndsWith("."))
                {
                    tmp = tmp.Substring(0, tmp.Length - 1);
                }

                var versionslänge = tmp.Length - tmp.Replace(".", "").Length;

                if (versionslänge > 2)
                {
                    throw new Exception(
                        "in the extended version the version number may have only 2 digits e.g. 1.1 | The extended version then maintains the 3rd digit and the 4th is reserved for the build number!");
                }


                int extendedVersionInfo = Look4NextEntrys(content, index);
                hauptversion = tmp + "." + extendedVersionInfo;
                break;


            }


            Version version;
            Version.TryParse(hauptversion, out version);

            if (version != null)
            {
                return version.ToString();
            }
            else
            {
                // one-digit version numbers
                int test = 0;
                int.TryParse(hauptversion, out test);
                if (test > 0)
                {
                    return test.ToString();
                }
            }

            throw new Exception("No version found");
        }

        private static int Look4NextEntrys(string[] content, int index)
        {
            index++; //next entry
            bool isNumberMode = false;
            bool isFirstLine = true;

            var countItems = 0;
            var lastNumberValue = 0;
            var countWhitespace = 0;

            for (int i = index; i < content.Length; i++)
            {
                var tmp = content[i];
                if (string.IsNullOrWhiteSpace(tmp)) continue;
                var firstWord = GetFirstWord(tmp.Trim());

                if (isFirstLine)
                {
                    countWhitespace = GetCountIdent(tmp);
                    if (!(firstWord == "*" || firstWord == "-"))
                        isNumberMode = true;

                    isFirstLine = false;
                }


                var isTitelFoundOnLine = firstWord.StartsWith("#");
                if (isTitelFoundOnLine)
                {
                    break;
                }

                var isLineIdent = tmp.Substring(countWhitespace).StartsWith("  ");
                if (isLineIdent)
                {
                    continue;
                }

                if (isNumberMode)
                {
                    int found = -1;
                    int.TryParse(firstWord.Replace(".", ""), out found);
                    if (found > -1)
                    {
                        if (found != lastNumberValue + 1)
                        {
                            throw new InvalidDataException($"The value {lastNumberValue} must not be followed by {found}.");
                        }
                        lastNumberValue = found;
                    }
                }
                else
                {
                    countItems++;
                    continue;
                }

            }

            return isNumberMode ? lastNumberValue : countItems;
        }

        private static int GetCountIdent(string tmp)
        {
            var result = 0;
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i] == ' ')
                {
                    result++;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private static string GetFirstWord(string tmp)
        {
            var result = "";
            for (var i = 0; i < tmp.Length; i++)
            {
                char c = tmp[i];
                if (c == ' ')
                {
                    return result;
                }
                result += c;
            }

            return result;
        }
    }
}