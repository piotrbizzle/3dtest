VAR inventory_0 = ""
VAR inventory_1 = ""
VAR inventory_2 = ""
VAR score = 100

== function inventory_has(item) ==
{
    - inventory_0 == item:
        ~ return true
    - inventory_1 == item:
        ~ return true
    - inventory_2 == item:
       ~ return true
}
    ~ return false

== foods_fish ==
- It's a Fish! Aquatic!
+ "More Fish!"
  -> more_fish 
+ "Less Fish!"
  -> less_fish
+ "Swap Screens!"
  -> swap_fish
+ "Nevermind"
  -> END


== more_fish ==
 - Moar! # give_foods_fish
 + "Okay"
    -> END

== less_fish ==
 - Noooo! # take_foods_fish
 + "Okay"
    -> END

== swap_fish ==
 - OOOOKAY! # swap_screen
 + "Okay"
    -> END
