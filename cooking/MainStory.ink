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
+ "Okay"
    -> END