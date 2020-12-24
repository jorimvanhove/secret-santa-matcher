using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SecretSanta;
using SecretSanta.Models;

namespace Tests.Tests
{
    [TestFixture]
    public class MatcherTest
    {
        [Test]
        public void TestCanMatchAll()
        {
            var participants = ParticipantFactory();
            
            var matcher = new Matcher(participants);
            
            matcher.Match(false, true);
            Assert.IsTrue(matcher.Participants().All(p => p.Match != null));
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
                participantE
            };
            
        }
    }
}