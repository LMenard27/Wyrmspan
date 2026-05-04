/*
Enums to represent the different states of the game
*/
public enum States
{
    // choose resource to discard
    AWAIT_DISCARD_RESOURCE = 0,
    // choose resource from bank
    AWAIT_GET_RESOURCE = 1,
    // choose to excavate, entice or explore
    AWAIT_PLAYER_ACTION = 2,
    // choose cave from shop
    AWAIT_GET_CAVE = 3,
    // choose dragon from shop
    AWAIT_GET_DRAGON = 4,
    // discard dragon from hand
    AWAIT_DISCARD_DRAGON = 5,
    // discard cave from hand
    AWAIT_DISCARD_CAVE = 6,
    // no-op state for when a player skips payment or has no payment to make
    NOP = 7, 
    // end of round upkeep
    END_ROUND = 8,
    // end of game
    END_GAME = 9,
    // choose benefit to discard
    AWAIT_DISCARD_BENEFIT = 10,
    // choose benefit to gain
    AWAIT_GAIN_BENEFIT = 11,
}