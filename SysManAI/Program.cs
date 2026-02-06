using System.Text.RegularExpressions;
using TcSysManRMLib;

var systemManagerRM = Activator.CreateInstance(Type.GetTypeFromProgID("TcSysManagerRM", throwOnError: true)!) as ITcSysManagerRM;
var stSysManager = systemManagerRM?.CreateSysManager15();
if (stSysManager is null) return;

// configuration string
Console.WriteLine("Input a layout (e.g. 'AAABBB'): ");
var layout = Console.ReadLine()?.ToUpper();
File.WriteAllText(@"C:\Temp\layout.txt", layout);

// validate input
if (string.IsNullOrEmpty(layout) || !Regex.IsMatch(layout, @"^[AB]{1,10}$"))
{ Console.WriteLine("Invalid input"); return; }

// open base project
var path = Path.Combine(AppContext.BaseDirectory, "TwinCAT");
stSysManager.OpenConfiguration(Path.Combine(path, "TwinCAT Project.tsproj"));

// get EtherCAT master
ITcSmTreeItem ecMaster = stSysManager.LookupTreeItem("TIID^Device 1 (EtherCAT)^Term 1 (EK1100)");

int terminalIndex = 2, aIndex = 0, bIndex = 0;
foreach (char c in layout)
{
    // station type A
    if (c == 'A')
    {
        // proxes
        var inputPath = $"TIPC^PLC^PLC Instance^PlcTask Inputs^GVL_Stations.A[{aIndex}]";
        var DIs = ecMaster.CreateChild($"Term {terminalIndex} (EL1004)", 9099, "", "EL1004");
        stSysManager.LinkVariables($"{inputPath}.PRX_1", $"{DIs.PathName}^Channel 1^Input");
        stSysManager.LinkVariables($"{inputPath}.PRX_2", $"{DIs.PathName}^Channel 2^Input");
        terminalIndex++;

        // PBs
        var outputPath = $"TIPC^PLC^PLC Instance^PlcTask Outputs^GVL_Stations.A[{aIndex}]";
        var DOs = ecMaster.CreateChild($"Term {terminalIndex} (EL2004)", 9099, "", "EL2004");
        stSysManager.LinkVariables($"{outputPath}.PB_1", $"{DOs.PathName}^Channel 1^Output");
        stSysManager.LinkVariables($"{outputPath}.PB_2", $"{DOs.PathName}^Channel 2^Output");
        terminalIndex++;
        aIndex++;
    }
    // station B type
    if (c == 'B')
    {
        // proxes
        var inputPath = $"TIPC^PLC^PLC Instance^PlcTask Inputs^GVL_Stations.B[{bIndex}]";
        var DIs = ecMaster.CreateChild($"Term {terminalIndex} (EL1004)", 9099, "", "EL1004");
        stSysManager.LinkVariables($"{inputPath}.PRX_1", $"{DIs.PathName}^Channel 1^Input");
        stSysManager.LinkVariables($"{inputPath}.PRX_2", $"{DIs.PathName}^Channel 2^Input");
        stSysManager.LinkVariables($"{inputPath}.PRX_3", $"{DIs.PathName}^Channel 3^Input");
        terminalIndex++;

        // pot
        var AI = ecMaster.CreateChild($"Term {terminalIndex} (EL3002)", 9099, "", "EL3002");
        stSysManager.LinkVariables($"{inputPath}.POT_1", $"{AI.PathName}^AI Standard Channel 1^Value");
        terminalIndex++;
        bIndex++;
    }
}

stSysManager.SaveConfiguration(Path.Combine(path, "TwinCAT Project.tsproj"));
stSysManager.ActivateConfiguration();
stSysManager.StartRestartTwinCAT();