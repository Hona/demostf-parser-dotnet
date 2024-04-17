// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DemosTF.Parser.NET;

BenchmarkRunner.Run<Benchmarks>();

public class Benchmarks
{
    private const string StvDemo = "auto-20140701-233440-jump_propel_fixed_v3.dem";
    private const string DemoParserExe = "parse_demo_latest.exe";
    
    [Benchmark]
    public StvParserResponse Analyze_ProcessStart()
    {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = DemoParserExe,
                Arguments = StvDemo,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            var process = Process.Start(processStartInfo);
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            
            if (output is null)
            {
                throw new InvalidOperationException("STV was invalid, parsing failed.");
            }
            
            return JsonSerializer.Deserialize<StvParserResponse>(output) ?? throw new InvalidOperationException("STV was invalid, parsing failed. Error: " + output);
    }

    [Benchmark]
    public StvParserResponse? Analyze_PInvoke()
    {
        return TfDemoParser.AnalyzeDemo(StvDemo);
    }
}