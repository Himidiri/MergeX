using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.Burst;
using Unity.Burst.Editor;

public class BurstDisassemblerTests
{
    private BurstDisassembler _disassembler;

    [OneTimeSetUp]
    public void SetUp()
    {
        _disassembler = new BurstDisassembler();
    }

    private string GetDisassembly(string compileTargetName, int debugLvl)
    {
        // Get target job assembly:
        var assemblies = BurstReflection.EditorAssembliesThatCanPossiblyContainJobs;
        var result = BurstReflection.FindExecuteMethods(assemblies, BurstReflectionAssemblyOptions.None);
        var compileTarget = result.CompileTargets.Find(x => x.GetDisplayName() == compileTargetName);

        var options = new StringBuilder();

        compileTarget.Options.TryGetOptions(compileTarget.JobType, true, out string defaultOptions);
        options.AppendLine(defaultOptions);
        // Disables the 2 current warnings generated from code (since they clutter up the inspector display)
        // BC1370 - throw inside code not guarded with ConditionalSafetyCheck attribute
        // BC1322 - loop intrinsic on loop that has been optimised away
        options.AppendLine($"{BurstCompilerOptions.GetOption(BurstCompilerOptions.OptionDisableWarnings, "BC1370;BC1322")}");

        options.AppendLine($"{BurstCompilerOptions.GetOption(BurstCompilerOptions.OptionTarget, BurstTargetCpu.X64_SSE4)}");

        options.AppendLine($"{BurstCompilerOptions.GetOption(BurstCompilerOptions.OptionDebug, $"{debugLvl}")}");

        var baseOptions = options.ToString();

        var append = BurstInspectorGUI.GetDisasmOptions()[(int)DisassemblyKind.Asm];

        // Setup disAssembler with the job:
        compileTarget.RawDisassembly = BurstInspectorGUI.GetDisassembly(compileTarget.Method, baseOptions + append);
        return compileTarget.RawDisassembly.TrimStart('\n');
    }



    // A Test behaves as an ordinary method
    [Test]
    public void GetBlockIdxFromTextIdxTest()
    {
        var text = GetDisassembly("BurstInspectorGUITests.MyJob - (IJob)", 2);

        Assert.IsTrue(_disassembler.Initialize(text, BurstDisassembler.AsmKind.Intel, false, false));

        var block1Start = _disassembler.GetOrRenderBlockToText(0).Length;
        var block1End = block1Start + _disassembler.GetOrRenderBlockToText(1).Length-1;

        Assert.AreEqual((1, block1Start, block1End),
            _disassembler.GetBlockIdxFromTextIdx(block1Start + 1), "Block index was wrong");
    }
}
