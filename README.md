# secret-santa-matcher

An attempt at providing a solution for https://github.com/Intracto/SecretSanta/issues/510
This is the C# implementation of https://github.com/jorimvanhove/secret-santa-codechallenge-510

# Functional requirements
##### Write an algorithm that assigns every participant to another participant

- Given participants A, B, C, D, E.
- Could result in A -> B -> C -> D -> E -> A (randomization is required)
- Multiple loops like A -> B -> A + C -> D -> E -> C are allowed
- A -> A is not allowed

##### Must work with an odd number of participants

##### Participants can have (multiple) excludes 

Eg this must work:
- A: exclude C, D, E
- B: exclude A, D, E
- C: exclude A, B, E
- D: exclude A, B, C
- E: exclude B, C, D

In this case the only valid list is **A -> B -> C -> D -> E -> A**

##### The solution must error-handle a situation that can't be resolved. 
Eg with participants A, B, C:
- A: exclude C
- B: exclude C
- C: -

This would result in **A -> B -> A** and a **C** that can't be assigned.

##### Must run in an acceptable time with > 200 participants.
