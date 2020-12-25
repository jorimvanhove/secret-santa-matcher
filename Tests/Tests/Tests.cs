using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SecretSanta;
using SecretSanta.Exceptions;
using SecretSanta.Models;

namespace Tests.Tests
{
    [TestFixture]
    public class MatcherTest
    {
        [Test]
        public void CanMatchAllTest()
        {
            var participants = ParticipantFactory();
            var matcher = new Matcher(participants);
            
            matcher.Match(false, true);
            Assert.IsTrue(matcher.Participants().All(p => p.Match != null));
        }
        
        [Test]
        public void CanMatchAnyTest()
        {
            var participants = ParticipantFactory();
            var matcher = new Matcher(participants);
            
            matcher.Match();
            Assert.IsTrue(matcher.Participants().Any(p => p.Match != null));
        }

        [Test]
        public void ThrowsOnUnresolvedParticipantsTest()
        {
            var participants = ImpossibleToMatchParticipantFactory();
            var matcher = new Matcher(participants);
            Assert.Throws<UnresolvedMatchException>(() => matcher.Match(true, true));
        }
        
        [Test]
        public void ValidateUniqueMatchesTest()
        {
            var participants = ParticipantFactory();
            var matcher = new Matcher(participants);
            
            matcher.Match();
            Assert.DoesNotThrow(() => matcher.Match());
        }
        
        private static IEnumerable<Participant> ParticipantFactory()
        {
            var participantA = new Participant("A");
            var participantB = new Participant("B");
            var participantC = new Participant("C");
            var participantD = new Participant("D");
            var participantE = new Participant("E");
            
            participantA.Excludes = new List<Participant>
            {
                participantC,
                participantD,
                participantE
            };
            
            participantB.Excludes = new List<Participant>
            {
                participantA,
                participantD,
                participantE
            };
            
            participantC.Excludes = new List<Participant>
            {
                participantA,
                participantB,
                participantE
            };
            
            participantD.Excludes = new List<Participant>
            {
                participantA,
                participantB,
                participantC
            };
            
            participantE.Excludes = new List<Participant>
            {
                participantB,
                participantC,
                participantD
            };
            
            return new List<Participant>
            {
                participantA,
                participantB,
                participantC,
                participantD,
                participantE,
            };
        }

        private IEnumerable<Participant> ImpossibleToMatchParticipantFactory()
        {
            var participantA = new Participant("A");
            var participantB = new Participant("B");
            var participantC = new Participant("C");
            
            participantA.Excludes = new List<Participant>
            {
                participantC,
            };
            
            participantB.Excludes = new List<Participant>
            {
                participantC,
            };
            
            return new List<Participant>
            {
                participantA,
                participantB,
                participantC,
            };
        }
    }
}