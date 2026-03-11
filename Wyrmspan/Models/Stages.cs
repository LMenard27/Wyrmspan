enum Stages
{
    SETUP_HAND,
    SETUP_RESOURCES,
    START_TURN,
    TAKE_CAVE_FROM_SHOP,
    TAKE_DRAGON_FROM_SHOP,
    TAKE_RESOURCE_FROM_BANK,
    END_ROUND,
    END_GAME,
    NOP, // no operation, a pass-through
}