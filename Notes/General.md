Timing of events
----------------
Would like a head-check from someone who knows more about timing to see if I'm doing these things correctly... esp w/ regards to doing the manipulation stuff in the LateUpdate (seems to be the best way to ensure all the focal points have fired appropriately)

Things to play with later
-------------------------
There are lots of things that, for the first pass, I'm going to punt on playing with even though I find them compelling.  Would be interested to wait and see if there is any need for these features before building out.  Here are a few:

* Handles to constrain rotation / scaling / movement against only one dimension
* Different layers of Focal Points for different controllers (so that each hand can manipulate it's own object)
* Distinguishing between objects and environments... or discovering patterns that properly account for both
* Allowing control on a per user basis
* "Displaced Claw" -- when you grip, the difference of your controller and the claw gets cemented, so if you rotate your hand, the claw only rotates, and if you displace your hand, the claw displaces in a mirrored manner
* menu (swipe right to show menu) that has checkboxes:
  * [x] translate
  * [x] rotate
  * [x] scale
* When the focal point is on the ground, pointing off into the horizon has undesirable results.  Would be nice if there were some way to map a sphere around the user onto that surface so that they can move around freely without flying way far off.
