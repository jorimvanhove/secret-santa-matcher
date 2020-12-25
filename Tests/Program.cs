using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            var stopwatch = new Stopwatch();
            int numberOfParticipants = 250;
            int maxAllowedNumberOfExcludes = 4;
            bool stressTest = false;
            int iterations = 1;
            
            var optionSet = new OptionSet
            {
                "Options:",
                {"p|participants:", "Number of participants to match", (int p) => { numberOfParticipants = p ; }},
                {"e|excludes:", "Max number of excludes per participant", (int e) => { maxAllowedNumberOfExcludes = e ; }},
                {"s|stresstest", "", (s) => { stressTest = s != null; }},
            };
            
            optionSet.WriteOptionDescriptions(Console.Out);
            optionSet.Parse(args);

            if (stressTest)
            {
                iterations = 200;
                numberOfParticipants = 100;
                maxAllowedNumberOfExcludes = 0;
            }

            for (int i = 0; i < iterations; i++)
            {
                
                if (stressTest)
                {
                    numberOfParticipants = i * 100;
                    maxAllowedNumberOfExcludes = i * i;
                }
                
                var participants = AddExcludes(ParticipantFactory(numberOfParticipants), maxAllowedNumberOfExcludes);
                var matcher = new Matcher(participants);
            
                stopwatch.Start();
                matcher.Match();
                stopwatch.Stop();

                if (!stressTest)
                {
                    Console.WriteLine("Matched participants:");
                    foreach (var matched in matcher.MatchedParticipants())
                    {
                        Console.WriteLine(matched.Name + " -> " + matched.Match.Name);
                    }
                    
                    if (matcher.UnmatchedParticipants().Any())
                    {
                        Console.WriteLine("Unmatched participants:");
            
                        foreach (var matched in matcher.UnmatchedParticipants())
                        {
                            Console.WriteLine(matched.Name + " -> - ");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Matched {0} of {1}", matcher.MatchedParticipants().Count(), numberOfParticipants);
                    if (matcher.UnmatchedParticipants().Any())
                    {
                        Console.WriteLine("Could not match {0} participants", matcher.UnmatchedParticipants().Count());
                    }
                }
                
                Console.WriteLine("Matching {0} with {1} excludes each took {2} ms", 
                    numberOfParticipants, maxAllowedNumberOfExcludes, stopwatch.ElapsedMilliseconds);

                for (int c = 0; c < 45; c++)
                {
                    Console.Write("-");
                }
                
                Console.WriteLine();
            }
        }

        private static IEnumerable<Participant> ParticipantFactory(int numberOfParticipants)
        {
            var participants = new List<Participant>();
            
            for (var i = 0; i < numberOfParticipants; i++)
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
                
                for (var i = 0; i < maxAllowedNumberOfExcludes; i++)
                {
                    if (i == random.Next(maxAllowedNumberOfExcludes))
                    {
                        continue;
                    }

                    var exclude = participantsToProcess.Where(p => !p.Equals(participant))
                        .Skip(random.Next(participantsToProcess.Count - 1))
                        .Take(1)
                        .SingleOrDefault();
                    
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