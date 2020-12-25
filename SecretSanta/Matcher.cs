using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using SecretSanta.Exceptions;
using SecretSanta.Extensions;
using SecretSanta.Models;

namespace SecretSanta
{
    public class Matcher
    {
        private readonly IEnumerable<Participant> participants;
        
        public Matcher(IEnumerable<Participant> participants)
        {
            this.participants = participants;
        }

        public IEnumerable<Participant> Participants()
        {
            return participants;
        }

        public IEnumerable<Participant> MatchedParticipants()
        {
            return participants.Where(p => p.Match != null).ToImmutableList();
        }
        
        public IEnumerable<Participant> UnmatchedParticipants()
        {
            return participants.Where(p => p.Match == null).ToImmutableList();
        }
        
        private List<Participant?> Receivers()
        {
            var matches = participants.Where(p => p.Match != null)
                .Select(p=> p.Match)
                .ToList();

            return participants.Except(matches).ToList();
        }
        
        public void Match(bool shuffleParticipants = true, bool throwOnUnresolved = false, bool validateUniqueMatches = true)
        {
            TryMatchParticipants(participants, shuffleParticipants);
            if (participants.Any(p => p.Match == null))
            {
                TryMatchParticipants(participants.Where(p => p.Match == null).ToList(), shuffleParticipants);
            }

            if (throwOnUnresolved && participants.Any(p => p.Match == null))
            {
                throw new UnresolvedMatchException("Some participants could not be matched");
            }

            if (!validateUniqueMatches) return;
            {
                ValidateUniqueMatches();
            }
        }

        private void TryMatchParticipants(IEnumerable<Participant> participantsToMatch, bool shuffleParticipants)
        {
            var toMatch = participantsToMatch.ToList();
            if (shuffleParticipants)
            {
                toMatch = toMatch.Shuffle().ToList();
            }
            
            foreach (var participant in toMatch)
            {
                var receivers = Receivers();
                using var receiversEnumerator = receivers.GetEnumerator();
                if (participant.Match != null)
                {
                    // Already matched this participant
                    continue;
                }

                ((IEnumerator) receiversEnumerator).Reset();
                if (!receivers.Any())
                {
                    // We ran out of receivers
                    return;
                }

                receiversEnumerator.MoveNext();
                while (receiversEnumerator.Current != null && receiversEnumerator.Current.Equals(participant))
                {
                    receiversEnumerator.MoveNext();
                }

                if (receiversEnumerator.Current == null)
                {
                    continue;
                }

                if (participant.Excludes != null &&
                    participant.Excludes.Any(e => e.Equals(receiversEnumerator.Current)))
                {
                    receiversEnumerator.MoveNext();
                    if (receiversEnumerator.Current == null)
                    {
                        continue;
                    }
                }

                try
                {
                    participant.Match = (Participant?) receiversEnumerator.Current;
                }
                catch (InvalidMatchException)
                {
                }
            }
        }
        
        private void ValidateUniqueMatches()
        {
            var matchedParticipantGuids = participants.Where(p => p.Match != null)
                .Select(p => p.Match.Id)
                .ToList();
            
            var diffChecker = new HashSet<Guid>();
            if (!matchedParticipantGuids.All(guid => diffChecker.Add(guid)))
            {
                throw new InvalidMatchException("Receiver matched to more than 1 participant");
            }
        }
    }
}