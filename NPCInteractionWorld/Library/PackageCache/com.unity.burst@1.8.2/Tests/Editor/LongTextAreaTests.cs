using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using Unity.Burst.Editor;
using System.Text;
using Unity.Burst;
using System.Text.RegularExpressions;

[TestFixture]
public class LongTextAreaTests
{
    private LongTextArea _textArea;

    [OneTimeSetUp]
    public void SetUp()
    {
        _textArea = new LongTextArea();
    }

    [Test]
    [TestCase("", "        push        rbp\n        .seh_pushreg rbp\n", 7, true)]
    [TestCase("<color=#CCCCCC>", "        push        rbp\n        .seh_pushreg rbp\n", 25, true)]
    [TestCase("<color=#d7ba7d>", "        push        rbp\n        .seh_pushreg rbp\n", 21 + 15 + 8 + 15, true)]
    [TestCase("", "\n# hulahop    hejsa\n", 5, false)]
    public void GetStartingColorTagTest(string tag, string text, int textIdx, bool syntaxHighlight)
    {
        var disAssembler = new BurstDisassembler();
        _textArea.SetText("", text, true, disAssembler, disAssembler.Initialize(text, BurstDisassembler.AsmKind.Intel, true, syntaxHighlight));
        if (!_textArea.CopyColorTags) _textArea.ChangeCopyMode();

        Assert.That(_textArea.GetStartingColorTag(0, textIdx), Is.EqualTo(tag));
    }

    [Test]
    [TestCase("", "        push        rbp\n        .seh_pushreg rbp\n", 7, true)]
    [TestCase("</color>", "        push        rbp\n        .seh_pushreg rbp\n", 25, true)]
    [TestCase("</color>", "        push        rbp\n        .seh_pushreg rbp\n", 21 + 15 + 8 + 15, true)]
    [TestCase("", "        push        rbp\n        .seh_pushreg rbp\n", 14 + 15 + 8, true)]
    [TestCase("", "\n# hulahop    hejsa\n", 5, false)]
    public void GetEndingColorTagTest(string tag, string text, int textIdx, bool syntaxHighlight)
    {
        var disAssembler = new BurstDisassembler();
        _textArea.SetText("", text, true, disAssembler, disAssembler.Initialize(text, BurstDisassembler.AsmKind.Intel, true, syntaxHighlight));
        if (!_textArea.CopyColorTags) _textArea.ChangeCopyMode();

        Assert.That(_textArea.GetEndingColorTag(0, textIdx), Is.EqualTo(tag));
    }

    [Test]
    [TestCase("<color=#FFFF00>hulahop</color>    <color=#DCDCAA>hejsa</color>\n", 0, 16, 16)]
    [TestCase("<color=#FFFF00>hulahop</color>\n    <color=#DCDCAA>hejsa</color>\n", 1, 40, 9)]
    [TestCase("<color=#FFFF00>hulahop</color>\n    <color=#DCDCAA>hejsa</color>\n hej", 2, 67, 3)]
    [TestCase("<color=#FFFF00>hulahop</color>    <color=#DCDCAA>hejsa</color>", 0, 15, 15)]
    [TestCase("\n        <color=#4EC9B0>je</color>                <color=#d4d4d4>.LBB11_4</color>", 1, 34, 33)]
    // Test cases for when on enhanced text and not coloured.
    [TestCase("hulahop    hejsa\n", 0, 16, 16)]
    [TestCase("hulahop\n    hejsa\n", 1, 17, 9)]
    [TestCase("hulahop\n    hejsa\n hej", 2, 21, 3)]
    [TestCase("hulahop    hejsa", 0, 15, 15)]
    public void GetEndIndexOfColoredLineTest(string text, int line, int resTotal, int resRel)
    {
        Assert.That(_textArea.GetEndIndexOfColoredLine(text, line), Is.EqualTo((resTotal, resRel)));
    }

    [Test]
    [TestCase("hulahop    hejsa\n", 0, 16, 16)]
    [TestCase("hulahop\n    hejsa\n", 1, 17, 9)]
    [TestCase("hulahop\n    hejsa\n hej", 2, 21, 3)]
    [TestCase("hulahop    hejsa", 0, 15, 15)]
    [TestCase("\nhulahop    hejsa", 1, 16, 15)]
    public void GetEndIndexOfPlainLineTest(string text, int line, int resTotal, int resRel)
    {
        Assert.That(_textArea.GetEndIndexOfPlainLine(text, line), Is.EqualTo((resTotal, resRel)));
    }

    [Test]
    [TestCase("<color=#FFFF00>hulahop</color>\n    <color=#DCDCAA>hejsa</color>\n hej", 2, 2, 0)]
    [TestCase("<color=#FFFF00>hulahop</color>\n    <color=#DCDCAA>hejsa</color>\n hej", 1, 5, 15)]
    [TestCase("<color=#FFFF00>hulahop</color>    <color=#DCDCAA>hejsa</color>:", 0, 17, 46)]
    public void BumpSelectionXByColortagTest(string text, int lineNum, int charsIn, int colourTagFiller)
    {
        var (idxTotal, idxRel) = _textArea.GetEndIndexOfColoredLine(text, lineNum);
        Assert.That(_textArea.BumpSelectionXByColorTag(text, idxTotal - idxRel, charsIn), Is.EqualTo(charsIn + colourTagFiller));
    }

    [Test]
    [TestCase("        push        rbp\n        .seh_pushreg rbp\n", false)]
    [TestCase("        push        rbp\n        .seh_pushreg rbp\n", true)]
    public void SelectAllTest(string text, bool useDisassembler)
    {
        if (useDisassembler)
        {
            var disAssembler = new BurstDisassembler();
            _textArea.SetText("", text, true, disAssembler, disAssembler.Initialize(text, BurstDisassembler.AsmKind.Intel));
            _textArea.LayoutEnhanced(GUIStyle.none, Rect.zero, true);
        }
        else
        {
            _textArea.SetText("", text, true, null, false);
        }


        _textArea.selectPos = new Vector2(2, 2);
        // There is no inserted comments or similar in my test example, so finalAreaSize, should be equivalent for the two.
        _textArea.finalAreaSize = new Vector2(7.5f * text.Length, 15.2f);

        _textArea.SelectAll();
        Assert.That(_textArea.selectPos, Is.EqualTo(Vector2.zero));
        Assert.That(_textArea.selectDragPos, Is.EqualTo(new Vector2(7.5f * text.Length, 15.2f)));

        if (!useDisassembler)
        {
            Assert.That(_textArea.textSelectionIdx, Is.EqualTo((0, text.Length)));
        }
    }

    private BurstDisassembler GetDisassemblerandText(string compileTargetName, int debugLvl, out string textToRender)
    {
        // Get target job assembly:
        var assemblies = BurstReflection.EditorAssembliesThatCanPossiblyContainJobs;
        var result = BurstReflection.FindExecuteMethods(assemblies, BurstReflectionAssemblyOptions.None);
        var compileTarget =  result.CompileTargets.Find(x => x.GetDisplayName() == compileTargetName);

        Assert.IsTrue(compileTarget != default, $"Could not find compile target: {compileTarget}");

        BurstDisassembler disassembler = new BurstDisassembler();

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
        textToRender = compileTarget.RawDisassembly.TrimStart('\n');

        return disassembler;
    }

    [Test]
    [TestCase("BurstInspectorGUITests.MyJob - (IJob)", true, true, 2)]
    [TestCase("BurstInspectorGUITests.MyJob - (IJob)", true, true, 1)]
    [TestCase("BurstInspectorGUITests.MyJob - (IJob)", true, false, 2)]
    [TestCase("BurstInspectorGUITests.MyJob - (IJob)", true, false, 1)]
    [TestCase("BurstInspectorGUITests.MyJob - (IJob)", false, true, 2)]
    [TestCase("BurstInspectorGUITests.MyJob - (IJob)", false, true, 1)]
    [TestCase("BurstInspectorGUITests.MyJob - (IJob)", false, false, 2)]
    [TestCase("BurstInspectorGUITests.MyJob - (IJob)", false, false, 1)]
    public void CopyAllTest(string compileTargetName, bool useDisassembler, bool coloured, int debugLvl)
    {
        // Get target job assembly:
        var disassembler = GetDisassemblerandText(compileTargetName, debugLvl, out string textToRender);

        if (useDisassembler)
        {
            _textArea.SetText("", textToRender, true, disassembler, disassembler.Initialize(textToRender, BurstDisassembler.AsmKind.Intel, true, coloured));
            _textArea.ExpandAllBlocks();

            var builder = new StringBuilder();

            for (int i = 0; i < disassembler.Blocks.Count; i++)
            {
                builder.Append(disassembler.GetOrRenderBlockToText(i));
            }

            textToRender = builder.ToString();
        }
        else
        {
            _textArea.SetText("", textToRender, true, null, false);
        }

        _textArea.Layout(GUIStyle.none, _textArea.horizontalPad);

        _textArea.SelectAll();
        _textArea.DoSelectionCopy();

        Assert.AreEqual(textToRender, EditorGUIUtility.systemCopyBuffer);
    }

    [Test]
    [TestCase("BurstInspectorGUITests.MyJob - (IJob)")]
    public void CopyTextIdenticalWithAndWithoutColorTags(string compileTargetName)
    {
        // We don't wanna go messing with the users system buffer. At least if user didn't break anything.
        var savedSystemBuffer = EditorGUIUtility.systemCopyBuffer;

        // Get target job assembly:
        var disassembler = GetDisassemblerandText(compileTargetName, 2, out var textToRender);

        _textArea.SetText("", textToRender, true, disassembler,
            disassembler.Initialize(
                textToRender,
                BurstDisassembler.AsmKind.Intel));

        _textArea.Layout(GUIStyle.none, _textArea.horizontalPad);
        _textArea.LayoutEnhanced(GUIStyle.none, Rect.zero, true);
        for (var i=0; i<disassembler.Blocks[0].Length+50; i++) _textArea.MoveSelectionDown(Rect.zero, true);

        _textArea.LayoutEnhanced(GUIStyle.none, Rect.zero, true);
        _textArea.UpdateEnhancedSelectTextIdx(_textArea.horizontalPad);

        _textArea.DoSelectionCopy();
        var copiedText1 = EditorGUIUtility.systemCopyBuffer;

        _textArea.ChangeCopyMode();
        _textArea.DoSelectionCopy();
        var copiedText2 = EditorGUIUtility.systemCopyBuffer;

        var regx = new Regex(@"(<color=#[0-9A-Za-z]*>)|(</color>)");

        if (!_textArea.CopyColorTags)
        {
            (copiedText1, copiedText2) = (copiedText2, copiedText1);
        }
        copiedText2 = regx.Replace(copiedText2, "");

        EditorGUIUtility.systemCopyBuffer = savedSystemBuffer;
        Assert.AreEqual(copiedText1, copiedText2,
            "Copy with color tags did not match copy without " +
            "(Color tags is removed from the copy to make it comparable with the color-tag-less copy).");
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void KeepingSelectionWhenMovingTest(bool useDisassembler)
    {
        const string jobName = "BurstInspectorGUITests.MyJob - (IJob)";
        BurstDisassembler disassembler = GetDisassemblerandText(jobName, 2, out string textToRender);
        Rect workingArea = new Rect();

        if (useDisassembler)
        {
            _textArea.SetText(jobName, textToRender, true, disassembler, disassembler.Initialize(textToRender, BurstDisassembler.AsmKind.Intel));
            _textArea.LayoutEnhanced(GUIStyle.none, workingArea, true);
        }
        else
        {
            _textArea.SetText(jobName, textToRender, false, null, false);
        }
        _textArea.Layout(GUIStyle.none, _textArea.horizontalPad);

        Assert.IsFalse(_textArea.HasSelection);

        Vector2 start = _textArea.selectDragPos;
        if (useDisassembler) start.x = _textArea.horizontalPad + _textArea.fontWidth / 2;

        // Horizontal movement:
        _textArea.MoveSelectionRight(workingArea, true);
        Assert.IsTrue(_textArea.HasSelection);
        Assert.AreEqual(start + new Vector2(_textArea.fontWidth, 0), _textArea.selectDragPos,
            "Text selection did not match after right movement.");

        _textArea.MoveSelectionLeft(workingArea, true);
        Assert.IsTrue(_textArea.HasSelection);
        Assert.AreEqual(start, _textArea.selectDragPos,
            "Text selection did not match after left movement.");

        // Vertical movement:
        _textArea.MoveSelectionDown(workingArea, true);
        Assert.IsTrue(_textArea.HasSelection);
        Assert.AreEqual(start + new Vector2(0, _textArea.fontHeight), _textArea.selectDragPos,
            "Text selection did not match after downwards movement.");

        _textArea.MoveSelectionUp(workingArea, true);
        Assert.IsTrue(_textArea.HasSelection);
        Assert.AreEqual(start, _textArea.selectDragPos,
            "Text selection did not match after upwards movement.");
    }

    [Test]
    public void GetFragNrFromBlockIdxTest()
    {
        var disassembler = GetDisassemblerandText("BurstInspectorGUITests.MyJob - (IJob)", 2, out var textToRender);

        _textArea.SetText("", textToRender, true, disassembler,
            disassembler.Initialize(textToRender, BurstDisassembler.AsmKind.Intel, false, false));


        var garbageVariable = 0f;
        var numBlocks = disassembler.Blocks.Count;

        // Want to get the last fragment possible
        var expectedFragNr = 0;
        for (var i = 0; i < _textArea.blocksFragmentsPlain.Length-1; i++)
        {
            expectedFragNr += _textArea.GetPlainFragments(i).Count;
        }

        Assert.AreEqual(expectedFragNr, _textArea.GetFragNrFromBlockIdx(numBlocks-1, 0, 0, ref garbageVariable));

        Assert.AreEqual(3, _textArea.GetFragNrFromBlockIdx(3, 1, 1, ref garbageVariable));
    }

    [Test]
    public void GetFragNrFromEnhancedTextIdxTest()
    {
        const string jobName = "BurstJobTester2.MyJob - (IJob)";

        var disassembler = GetDisassemblerandText(jobName, 2, out var textToRender);
        _textArea.SetText(jobName, textToRender, true, disassembler,
            disassembler.Initialize(textToRender, BurstDisassembler.AsmKind.Intel, false, false));

        var garbageVariable = 0f;
        const int blockIdx = 2;

        var fragments = _textArea.RecomputeFragmentsFromBlock(blockIdx);
        var text = _textArea.GetText;
        var expectedFrag = blockIdx + fragments.Count - 1;

        var info = disassembler.BlockIdxs[blockIdx];

        var extraFragLen = fragments.Count > 1
            ? fragments[0].text.Length + 1 // job only contains 2 fragments at max
            : 0;

        var idx = info.startIdx + extraFragLen + 1;

        var expected = (expectedFrag, info.startIdx + extraFragLen);
        var actual = _textArea.GetFragNrFromEnhancedTextIdx(idx, 0, 0, 0, ref garbageVariable);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void SearchTextEnhancedTest(bool colored)
    {
        var disassembler = GetDisassemblerandText("BurstInspectorGUITests.MyJob - (IJob)", 1, out string textToRender);
        _textArea.SetText("", textToRender, true, disassembler, disassembler.Initialize(textToRender, BurstDisassembler.AsmKind.Intel, true, colored));

        var workingArea = new Rect(0, 0, 10, 10);
        _textArea.SearchText(new SearchCriteria(".Ltmp.:", true, false, true), new Regex(@".Ltmp.:"), ref workingArea);

        Assert.AreEqual(10, _textArea.NrSearchHits);

        // Check that they are filled out probably
        int nr = 0;
        foreach (var fragHits in _textArea.searchHits.Values)
        {
            foreach (var hit in fragHits)
            {
                Assert.AreEqual((0, 7, nr++), hit);
            }
        }
    }

    [Test]
    public void SelectOnOneLineTest()
    {
        const string testCase = "\n<color=#d4d4d4>.Ltmp12</color>: ...";

        var disassembler = new BurstDisassembler();
        _textArea.SetText("", testCase, false, disassembler, disassembler.Initialize(testCase, BurstDisassembler.AsmKind.Intel));

        // Set fontWidth and fontHeight
        _textArea.Layout(GUIStyle.none, 20f);

        // Set selection markers.
        // Error happened when selection started at the lowest point of a line.
        _textArea.selectPos = new Vector2(0, _textArea.fontHeight);
        // Select further down to make sure it wont be switched with selectPos.
        _textArea.selectDragPos = new Vector2(10 * _textArea.fontWidth, _textArea.fontHeight*2);

        // Hopefully it wont throw anything
        Assert.DoesNotThrow(() =>
            _textArea.PrepareInfoForSelection(0, 0, _textArea.fontHeight,
                new LongTextArea.Fragment() { text = testCase.TrimStart('\n'), lineCount = 1 },
                _textArea.GetEndIndexOfColoredLine));
    }
}