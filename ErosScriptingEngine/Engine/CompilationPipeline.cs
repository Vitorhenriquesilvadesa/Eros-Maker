using System;
using System.Collections.Generic;
using ErosScriptingEngine.Component;
using ErosScriptingEngine.Pass;

namespace ErosScriptingEngine.Engine
{
    public class CompilationPipeline
    {
        private readonly List<IErosCompilationPass> passes = new();

        public CompilationPipeline InsertStage(IErosCompilationPass pass)
        {
            passes.Add(pass);
            return this;
        }

        public void Run(IErosScriptingIOComponent input)
        {
            var currentInput = input;
            Console.WriteLine("Running without interceptors.");

            foreach (var pass in passes)
            {
                currentInput = RunPass(pass, currentInput);
            }
        }

        public void RunWithInterceptors(IErosScriptingIOComponent input)
        {
            IErosScriptingIOComponent currentInput = input;
            Console.WriteLine("Running with interceptors.");

            foreach (var pass in passes)
            {
                currentInput = RunPassWithInterceptors(pass, currentInput);
            }
        }

        private IErosScriptingIOComponent RunPass(IErosCompilationPass pass,
            IErosScriptingIOComponent input)
        {
            if (!pass.GetInputType().IsInstanceOfType(input))
            {
                throw new ArgumentException(
                    $"Input type mismatch. Expected: {pass.GetInputType()}, but got: {input.GetType()}");
            }

            return (IErosScriptingIOComponent)pass.Run(input);
        }

        private IErosScriptingIOComponent RunPassWithInterceptors(IErosCompilationPass pass,
            IErosScriptingIOComponent input)
        {
            if (!pass.GetInputType().IsInstanceOfType(input))
            {
                throw new ArgumentException(
                    $"Input type mismatch. Expected: {pass.GetInputType()}, but got: {input.GetType()}");
            }

            return (IErosScriptingIOComponent)pass.RunWithInterceptors(input);
        }

        public List<IErosCompilationPass> GetPasses()
        {
            return passes;
        }
    }
}