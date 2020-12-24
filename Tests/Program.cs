using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Options;
using SecretSanta;
using SecretSanta.Models;

namespace Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int numberOfParticipants = 250;
            int maxAllowedNumberOfExcludes = 4;

            // var optionSet = new OptionSet
            // {
            //     "Options:",
            //     {"p|participants", "Number of participants to match", (int p) => { numberOfParticipants = p ; }},
            //     {"e|excludes", "Max number of excludes per participant", (int e) => { maxAllowedNumberOfExcludes = e ; }},
            // };
            //
            // optionSet.WriteOptionDescriptions(Console.Out);
            //
            // optionSet.Parse(args);

            var participants = AddExcludes(ParticipantFactory(numberOfParticipants), maxAllowedNumberOfExcludes);
            var matcher = new Matcher(participants);
            
            matcher.Match();

            Console.Title = "Matched participants:";
            foreach (var matched in matcher.MatchedParticipants())
            {
                Console.WriteLine(matched.Name + " -> " + matched.Match.Name);
            }
            
            Console.Title = "Unmatched participants:";
            foreach (var matched in matcher.UnmatchedParticipants())
            {
                Console.WriteLine(matched.Name + " -> - ");
            }
        }

        private static IEnumerable<Participant> ParticipantFactory(int numberOfParticipants)
        {
            var participants = new List<Participant>();
            
            for (var i = 0; i <= numberOfParticipants; i++)
            {
                participants.Add(new Participant("Test" + i));
            }

            return participants;
        }
        
        private static IEnumerable<Participant> AddExcludes(IEnumerable<Participant> participants,
            int maxAllowedNumberOfExcludes)
        {
            var random = new Random();
            var participantsToProcess = participants.ToList();
            foreach (var participant in participantsToProcess)
            {
                var excludes = new List<Participant>();
                
                for (var i = 0; i <= maxAllowedNumberOfExcludes; i++)
                {
                    if (i == random.Next(maxAllowedNumberOfExcludes))
                    {
                        continue;
                    }

                    var exclude = participantsToProcess.Where(p => !p.Equals(participant))
                        .Skip(random.Next(participantsToProcess.Count))
                        .Take(1)
                        .Single();
                    
                    if (exclude != participant && !excludes.Any(x => x.Equals(exclude)))
                    {
                        excludes.Add(exclude);
                    }
                }

                if (excludes.Any())
                {
                    participant.Excludes = excludes;
                }
                
            }
            
            return participantsToProcess;
        }
    }
}