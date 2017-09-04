using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Shared NeoCity Server Lib")]
[assembly: AssemblyDescription("Shared NeoCity Server Lib")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("DCNC")]
[assembly: AssemblyProduct("Shared NeoCity Server Lib")]
[assembly: AssemblyCopyright("Copyright © 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ffea87f0-8aa3-43b8-8914-78b6d5d6fb67")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
#if DEBUG
[assembly: AssemblyInformationalVersion("0.2.{dmin:2015}.{chash:6}{!}-{branch}-debug")]
#else
[assembly: AssemblyInformationalVersion("0.2.{dmin:2015}.{chash:6}{!}-{branch}-release")]
#endif
[assembly: AssemblyVersion("0.2.0.0")]
[assembly: AssemblyFileVersion("0.2.0.0")]
/*[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]*/