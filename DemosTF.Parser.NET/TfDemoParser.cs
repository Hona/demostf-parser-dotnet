using System.Runtime.InteropServices;
using System.Text.Json;

namespace DemosTF.Parser.NET;

public static partial class TfDemoParser
{
    private const string DllName = "tf_demo_parser.dll";

    [LibraryImport(DllName, EntryPoint = "analyze_demo", StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr analyze_demo(string path);

    [LibraryImport(DllName, EntryPoint = "free_string", StringMarshalling = StringMarshalling.Utf8)]
    private static partial void free_string(IntPtr ptr);

    public static StvParserResponse? AnalyzeDemo(string path)
    {
        var resultPtr = analyze_demo(path);

        try
        {
            if (resultPtr == IntPtr.Zero)
            {
                return null;
            }

            var result = Marshal.PtrToStringAnsi(resultPtr);

            if (result is null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<StvParserResponse>(result);
        }
        finally
        {
            // Free the memory allocated in the DLL
            free_string(resultPtr);
        }
    }
}