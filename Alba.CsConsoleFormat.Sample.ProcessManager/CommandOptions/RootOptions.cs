using CommandLine;
using JetBrains.Annotations;

namespace Alba.CsConsoleFormat.Sample.ProcessManager.CommandOptions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal sealed class RootOptions
    {
        [VerbOption("list", HelpText = "List running processes.")]
        public ListOptions List { get; set; }

        [VerbOption("start", HelpText = "Start a new process.")]
        public StartOptions Start { get; set; }

        [VerbOption("help", HelpText = "Display help.")]
        public HelpOptions Help { get; set; }
    }
}