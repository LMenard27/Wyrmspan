public enum States
{
    AWAIT_DISCARD_RESOURCE = 0, // choose resource to discard
    AWAIT_GET_RESOURCE = 1, // choose resource from bank
    AWAIT_PLAYER_ACTION = 2, // choose to excavate, entice or explore
    AWAIT_GET_CAVE = 3, // choose cave from shop
    AWAIT_GET_DRAGON = 4, // choose dragon from shop
    AWAIT_DISCARD_DRAGON = 5, // discard dragon from hand
    AWAIT_DISCARD_CAVE = 6,  // discard cave from hand
    NOP = 7, // no-op
    END_ROUND = 8, // end of round upkeep
    END_GAME = 9, // end of game
}