using ErosScriptingEngine.Interceptor.Scan;
using ErosScriptingEngine.Scan;
using ErosScriptingEngine.Util;

namespace ErosScriptingEngine.Engine
{
    public class ErosScriptingManager
    {
        private CompilationPipeline _compilationPipeline;

        public ErosScriptingManager()
        {
            _compilationPipeline = new CompilationPipeline();

            ScanPass scanPass = new ScanPass();
            scanPass.AddInterceptor(new TokenPrinterInterceptor());
            _compilationPipeline.InsertStage(scanPass);

            ErosScriptableFile file =
                new ErosScriptableFile("C:\\Users\\vitor\\OneDrive\\Área de Trabalho\\Test Eros Script\\test.eros");

            _compilationPipeline.RunWithInterceptors(file);
        }
    }
}