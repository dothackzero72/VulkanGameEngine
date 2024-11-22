using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassEditor;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class ScriptCompiler
    {
        static public void CompileScript(List<string> scriptPathList)
        {
            List<Assembly> assemblyList = new List<Assembly>();

            foreach (var sourceCodeScript in scriptPathList)
            {
                string className = GetClassName(sourceCodeScript);
                string scriptText = File.ReadAllText(sourceCodeScript);

                if (NeedToCompile(sourceCodeScript))
                {
                    var syntaxTree = CSharpSyntaxTree.ParseText(scriptText);
                    var references = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => !x.IsDynamic)
                        .Select(x => MetadataReference.CreateFromFile(x.Location))
                        .ToList();

                    var compilation = CSharpCompilation.Create(className, new[] { syntaxTree }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                    using (var stream = new MemoryStream())
                    {
                        var result = compilation.Emit(stream);
                        if (result.Success)
                        {
                            using (var fileStream = new FileStream($"{className}.dll", FileMode.Create, FileAccess.Write))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                stream.CopyTo(fileStream);
                            }
                        }
                        else
                        {
                            PrintDiagnostics(result.Diagnostics);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"No recompilation needed for {sourceCodeScript}");
                }
            }
        }

        static private string GetClassName(string scriptPath)
        {
            return Path.GetFileNameWithoutExtension(scriptPath);
        }

        static private bool NeedToCompile(string scriptPath)
        {
            string compiledDllPath = Path.ChangeExtension(scriptPath, ".dll");
            return !File.Exists(compiledDllPath) ||
                   File.GetLastWriteTime(scriptPath) > File.GetLastWriteTime(compiledDllPath);
        }

        static private void PrintDiagnostics(IEnumerable<Diagnostic> diagnostics)
        {
            foreach (var diagnostic in diagnostics)
            {
                Console.WriteLine(diagnostic.ToString());
            }
        }
    }
}
