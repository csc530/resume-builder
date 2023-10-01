using System.Runtime.InteropServices;
using Spectre.Console.Cli;

namespace resume_builder.cli;

public class CLISettings : CommandSettings
{
	[CommandOption("-v|--verbose")] public bool Verbose { get; set; }
}