using System;
using System.Reflection;
 
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Windows.Azure.Msbuild")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("Windows.Azure.Msbuild")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("49d9838a-6c58-4bc2-ad62-5f9b06215018")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

public enum Reason
{
    Delegate,
    Humble,
    Framework,
    ThirdParty,
    Test
}

public class CoverageExcludeAttribute : Attribute
{
    public CoverageExcludeAttribute()
    {

    }

    public CoverageExcludeAttribute(Reason reason)
    {
        Reason = reason;
    }

    public Reason Reason { get; private set; }
}
