public enum States
{
    AWAIT_DISCARD_RESOURCE, // choose resource to discard
    AWAIT_GET_RESOURCE, // choose resource from bank
    AWAIT_PLAYER_ACTION, // choose to excavate, entice or explore
    AWAIT_GET_CAVE, // choose cave from shop
    AWAIT_GET_DRAGON, // choose dragon from shop
    AWAIT_DISCARD_DRAGON, // discard dragon from hand
    AWAIT_DISCARD_CAVE,  // discard cave from hand
    NOP, // no-op
    END_ROUND, // end of round upkeep
    END_GAME, // end of game
}