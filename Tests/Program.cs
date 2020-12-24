using System;
using System.Reflection;
using Mono.Options;

namespace Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int numberOfParticipants = 250;
            int maxAllowedNumberOfExcludes = 4;

            var optionSet = new OptionSet
            {
                "Options:",
                {"p|participants", "Number of participants to match", (int p) => { numberOfParticipants = p; }},
                {"e|excludes", "Max number of excludes per participant", (int e) => { maxAllowedNumberOfExcludes = e; }},
            };
            
            optionSet.WriteOptionDescriptions(Console.Out);

            optionSet.Parse(args);
            
            // TODO participantfactory & match
        }
    }
}