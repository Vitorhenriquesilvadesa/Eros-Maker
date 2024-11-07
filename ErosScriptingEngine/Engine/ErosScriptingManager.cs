using System.Collections.Generic;
using ErosScriptingEngine.Interceptor.Parse;
using ErosScriptingEngine.Interceptor.Scan;
using ErosScriptingEngine.Lexer;
using ErosScriptingEngine.Parse;
using ErosScriptingEngine.Scan;
using ErosScriptingEngine.Util;
using UnityEngine;

namespace ErosScriptingEngine.Engine
{
    public class ErosScriptingManager
    {
        private readonly CompilationPipeline _compilationPipeline;
        private readonly Dictionary<string, ErosExecutableScript> _erosExecutableScripts = new();

        public ErosScriptingManager()
        {
            _compilationPipeline = new CompilationPipeline();

            ScanPass scanPass = new ScanPass();
            scanPass.AddInterceptor(new TokenPrinterInterceptor());

            ParsePass parsePass = new ParsePass();
            parsePass.AddInterceptor(new ASTPrinterInterceptor());

            _compilationPipeline.InsertStage(scanPass);
            _compilationPipeline.InsertStage(parsePass);

            ErosScriptableFile file =
                new ErosScriptableFile("C:\\Users\\vitor\\OneDrive\\Área de Trabalho\\Test Eros Script\\test.eros");

            ErosExecutableScript script =
                (ErosExecutableScript)_compilationPipeline.RunWithInterceptors(file);

            _erosExecutableScripts.Add(file.GetName(), script);
        }

        public ErosExecutableScript GetScriptByName(string name)
        {
            if (_erosExecutableScripts.ContainsKey(name))
            {
                return _erosExecutableScripts[name];
            }

            Error($"'{name}' not found.");
            return null;
        }

        public static void Error(string message)
        {
            Debug.LogError($"{message}");
        }

        public static void Error(string message, Token location)
        {
            Debug.LogError($"{message} At line {location.line}.");
        }

        public static void Error(string message, uint line)
        {
            Debug.LogError($"{message} at line {line}.");
        }
    }
}