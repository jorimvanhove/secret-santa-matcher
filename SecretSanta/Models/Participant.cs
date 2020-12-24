using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SecretSanta.Exceptions;

namespace SecretSanta.Models
{
    public class Participant
    {
        public Participant(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
        public Guid Id { get; }
        public string Name { get; }
        private Participant? match;
        public IEnumerable<Participant>? Excludes { get; set; }
        
        public Participant? Match
        {
            get => match;
            set
            {
                Debug.Assert(value != null, nameof(value) + " != null");
                if (value.Id == Id)
                {
                    throw new InvalidMatchException("Participant can not be matched to self.");
                }
                match = value;
            }
        }
    }
}