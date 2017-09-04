using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NeoCity Auth Server")]
[assembly: AssemblyDescription("NeoCity Auth Server")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("DCNC")]
[assembly: AssemblyProduct("NeoCity Auth Server")]
[assembly: AssemblyCopyright("Copyright © 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d13f8db8-307c-4588-abc1-7b8dae93acc0")]

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