Timing of events
----------------
I want to be really aware of is making sure that this library executes everything with proper timings.  When do things get saved to memory, when do they get retrieved, and when do they get drawn?  Execution order means the difference between being up to date or being one or two frames behind (as was in my first pass).

Rotations are complicated...
----------------------------
Rotations anchored off of only two points will cause weird undesired effects, because two isn't enough to determine "up" in the rotations.  Perhaps a note in the readme or a warning in the console or something to set expectations.

Things to play with later
-------------------------
There are lots of things that, for the first pass, I'm going to punt on playing with even though I find them compelling.  Would be interested to wait and see if there is any need for these features before building out.  Here are a few:

Handles to constrain rotation / scaling / movement against only one dimension
Different layers of Focal Points for different controllers (so that each hand can manipulate it's own object)
Further research into distinguishing between objects and environments... or discovering patterns that properly account for both
Raycasting not on the object itself, but a plane normal to the raycast hitpoint
  (if the controller moves too fast within a frame and / of if the object is really small, it's easy for the raycast to not hit the object)
Allowing control on a per user basis
