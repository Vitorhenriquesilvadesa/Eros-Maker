using System;
using System.IO;
using System.Text;
using ErosScriptingEngine.Component;

namespace ErosScriptingEngine.Util
{
    public sealed class ErosScriptableFile : ErosScriptingIOComponent<ErosScriptableFile>
    {
        private readonly string source;

        public ErosScriptableFile(string path)
        {
            ErosValidationComponent<string> validator = new FileExtensionValidator("eros");
            validator.Validate(path);
            source = Extract(path);
        }

        public ErosScriptableFile(ErosScriptableFile scriptableFile)
        {
            source = scriptableFile.source;
        }

        public string GetSource()
        {
            return source;
        }

        private string Extract(string path)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        sb.AppendLine(line);
                    }
                }
            }
            catch (IOException e)
            {
                throw new System.Exception("Error reading file", e);
            }

            return sb.ToString();
        }

        public override object Clone()
        {
            return new ErosScriptableFile(this);
        }
    }
}