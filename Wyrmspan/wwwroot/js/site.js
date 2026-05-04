var statePlayer = 0;
var turnPlayer = 0;
var statePlayerName = "";
var turnPlayerName = "";
var currState = "";
var gameData = null;
var enticing = false;
let playerId = null;

var chooseOneModal = document.getElementById("chooseOneModal");
var exploreBtn = document.getElementById("exploreBtn");
var excavateBtn = document.getElementById("excavateBtn");
var enticeBtn = document.getElementById("enticeBtn");
var universalSkipBtn = document.getElementById("skipBtn");
var oneModalClose = document.getElementById("oneModalClose");
var twoModalClose = document.getElementById("twoModalClose");

exploreBtn.onclick = handleExploreBtn;
excavateBtn.onclick = handleExcavateBtn;
enticeBtn.onclick = handleEnticeBtn;
universalSkipBtn.onclick = callSkipEndpoint;

// When the user clicks on <span> (x), close the modal
oneModalClose.onclick = function() {
    chooseOneModal.style.display = "none";
}
twoModalClose.onclick = function() {
    chooseTwoModal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function(event) {
    if (event.target == chooseOneModal) {
        chooseOneModal.style.display = "none";
    }
}
window.onclick = function(event) {
    if (event.target == chooseTwoModal) {
        chooseTwoModal.style.display = "none";
    }
}

document.getElementById("rulesBtn").onclick = function () {
    window.open("/resources/rules.pdf", "_blank");
};

const savedPlayerId = localStorage.getItem("playerId");

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/displayHub", {accessTokenFactory: () => savedPlayerId})
    .build();

connection.on("updateDisplay", function (data) {
    console.log("Recieved websocket push");
    updateDisplay(data);
});

connection.on("assignPlayer", function (id) {
    playerId = id;
    console.log("You are Player:", playerId);
    localStorage.setItem("playerId", id);
    fetch("/Game/Ping", {
        method: "POST"
    });
});

connection.start()
    .then(() => console.log("✅ SignalR connected"))
    .catch(err => console.error("❌ SignalR error:", err));

connection.onclose(() => {
    console.warn("⚠️ SignalR disconnected");
});

/*
whenever an action occurs other than the player enticing or excavating,
the current state is changed to be the necessary AWAIT in order to 
resolve that action. This function is then called, which calls the 
necessary endpoint so that the backend can deal with the action
*/
function routeOneModalSubmit() {

    switch(currState) {
        case "AWAIT_PLAYER_ACTION":
            callExploreEndpoint();
            break;
        case "AWAIT_GET_RESOURCE":
            callGainResourceEndpoint();
            break;
        case "AWAIT_DISCARD_RESOURCE":
            callDiscardResourceEndpoint();
            break;
        case "AWAIT_GET_DRAGON":
            callGainDragonEndpoint();
            break;
        case "AWAIT_DISCARD_DRAGON":
            callDiscardDragonEndpoint();
            break;
        case "AWAIT_GET_CAVE":
            callGainCaveEndpoint();
            break;
        case "AWAIT_DISCARD_CAVE":
            callDiscardCaveEndpoint();
            break;
        case "AWAIT_GAIN_BENEFIT":
            console.log("await gain benefit");
            callGainBenefitEndpoint();
            break;
        case "AWAIT_DISCARD_BENEFIT":
            callDiscardResourceEndpoint();
            break;
        default:
            console.Error("One-Variable Modal should not be visible right now!");
            break;
    }

    chooseOneModal.style.display = "none";
    fetch("/Game/Ping", {
        method: "POST"
    });
}

/*
whenever the player excavates or entices, this is called, which then
calls the correct endpoint so that the backend can update necessary
acion to deal with the player's action. First checks that the game
is on the correct state to avoid bad actor calling said functions
*/
function routeTwoModalSubmit() {

    switch(currState) {
        case "AWAIT_PLAYER_ACTION":
            if (enticing) {
                callEnticeEndpoint();
            } else {
                callExcavateEndpoint();
            }
            break;
        default:
            console.Error("Two-Variable Modal should not be visible right now!");
            break;
    }

    chooseTwoModal.style.display = "none";
    fetch("/Game/Ping", {
        method: "POST"
    });
}

/*
called after the player chooses which cavern to explore, then
calls the backend to update player information for what they gained 
and spent when exploring. 
*/
function callExploreEndpoint() {
    var selectedCavern = document.querySelector("#oneModalDynamicContent select").selectedIndex;

    fetch("/Game/Explore?player=" + playerId + "&cavernID=" + selectedCavern, {
        method: "POST"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}

/*
called after player chooses which resource to gain and adds that 
resource to their inventory
*/
function callGainResourceEndpoint() {
    var selectedResource = document.querySelector("#oneModalDynamicContent select").value;

    fetch("/Game/ChooseResourceToGain?player=" + playerId + "&resource=" + selectedResource, {
        method: "POST"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}

/*
called after player chooses which resource to discard or a resource
is discarded as a cost of an action automatically, removing that
resource from the player's inventory
*/
function callDiscardResourceEndpoint() {
    var selectedResource = document.querySelector("#oneModalDynamicContent select").value;

    fetch("/Game/ChooseResourceToDiscard?player=" + playerId + "&resource=" + selectedResource, {
        method: "POST"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}

/*
called after a player chooses which benefit to gain, then 
has the backend deal with the result of that gain, which could
simply involve giving them a resource or giving them a dragon
from the shop and replacing that dragon in the shop. 
*/
function callGainBenefitEndpoint(){
    var selectedBenefit = document.querySelector("#oneModalDynamicContent select").value;
    var [type, value] = selectedBenefit.split(":");
    console.log("type of benefit: " + type);

    if (type === "dragon") {
        console.log("dragon endpoint called");
        fetch("/Game/ChooseDragonToGain?player=" + playerId + "&id=" + value, {
        method: "POST"
        }).then(response => response.text()).then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
        }).catch(error => {
        console.error("Error:", error);
        });
        board = gameData.board;
        for (i=0; i<board.dragonShop.length; i++) {
            if(board.dragonShop[i].id == value){
                gameData.board.pickDragonFromShop(i);
                break;
            }
        }
    } else if (type === "cave") {
        fetch("/Game/ChooseCaveToGain?player=" + playerId + "&id=" + value, {
        method: "POST"
    }).then(response => response.text()).then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    }).catch(error => {
        console.error("Error:", error);
    });
    } else {
        fetch("/Game/ChooseResourceToGain?player=" + playerId + "&resource=" + value, {
        method: "POST"
    }).then(response => response.text()).then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    }).catch(error => {
        console.error("Error:", error);
    });
    }
}

/*
calls backend function in order to update player information when a benefit is discarded,
placing the dragon in the correct player's hand, removing it from the shop, and
replacing the slot in the shop with a new dragon from the deck
*/
function callDiscardBenefitEndpoint(){
    var selectedBenefit = document.querySelector("#oneModalDynamicContent select").value;
    var [type, value] = selectedBenefit.split(":");
    console.log("type of benefit: " + type);

    if (type === "dragon") {
        console.log("dragon endpoint called");
        fetch("/Game/ChooseDragonToDiscard?player=" + playerId + "&id=" + value, {
        method: "POST"
        }).then(response => response.text()).then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
        }).catch(error => {
        console.error("Error:", error);
        });
        board = gameData.board;
        for (i=0; i<board.dragonShop.length; i++) {
            if(board.dragonShop[i].id == value){
                gameData.board.pickDragonFromShop(i);
                break;
            }
        }
    } else if (type === "cave") {
        fetch("/Game/ChooseCaveToDiscard?player=" + playerId + "&id=" + value, {
        method: "POST"
    }).then(response => response.text()).then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    }).catch(error => {
        console.error("Error:", error);
    });
    } else {
        fetch("/Game/ChooseResourceToDiscard?player=" + playerId + "&resource=" + value, {
        method: "POST"
    }).then(response => response.text()).then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    }).catch(error => {
        console.error("Error:", error);
    });
    }
}

/*
calls backend function in order to update player information when a dragon is gained,
placing the dragon in the correct player's hand, removing it from the shop, and
replacing the slot in the shop with a new dragon from the deck
*/
function callGainDragonEndpoint() {
    var selectedDragon = document.querySelector("#oneModalDynamicContent select").value;

    fetch("/Game/ChooseDragonToGain?player=" + playerId + "&id=" + selectedDragon, {
        method: "POST"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}

/*
calls backend function in order to update player information when a dragon is discarded
for any reason other than enticing 
*/
function callDiscardDragonEndpoint() {
    var selectedDragon = document.querySelector("#oneModalDynamicContent select").value;

    fetch("/Game/ChooseDragonToDiscard?player=" + playerId + "&id=" + selectedDragon, {
        method: "POST"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}

/*
calls backend function in order to update game information whenever a player skips,
which either occurs when a card had an optional reward or a player skips their turn. 
If the ladder occurred, that player will not have another action for the rest
of the round 
*/
function callSkipEndpoint() {
    fetch("/Game/PlayerSkip?player=" + playerId, {
        method: "POST"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}

/*
calls backend function in order to update player information when a cave is gained,
placing the cave in the correct player's hand, removing it from the shop, and
replacing the slot in the shop with a new cave from the deck
*/
function callGainCaveEndpoint() {
    var selectedCave = document.querySelector("#oneModalDynamicContent select").value;

    fetch("/Game/ChooseCaveToGain?player=" + playerId + "&id=" + selectedCave, {
        method: "POST"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}


/*
calls backend function in order to update player information when a cave is excavated,
placing the corresponding cave on the mat and discarding it from the player's hand
*/
function callExcavateEndpoint() {
    var selectedCave = document.querySelector("#twoModalDynamicContent select:nth-of-type(2)").value;
    var selectedCavern = document.querySelector("#twoModalDynamicContent select:nth-of-type(1)").value

    fetch("/Game/Excavate?player=" + playerId + "&caveID=" + selectedCave + "&cavernID=" + selectedCavern, {
        method: "POST"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}

/*
calls backend function in order to update player information when a dragon is enticed,
placing the dragon on player's mat and discarding it from their hadn
*/
function callEnticeEndpoint() {
    var selectedDragon = document.querySelector("#twoModalDynamicContent select:nth-of-type(2)").value;
    var selectedCavern = document.querySelector("#twoModalDynamicContent select:nth-of-type(1)").value

    fetch("/Game/Entice?player=" + playerId + "&dragonID=" + selectedDragon + "&cavernID=" + selectedCavern, {
        method: "POST"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}
/*
calls backend function in order to update player information when a cave is discarded
for a reason other than excavating 
*/
function callDiscardCaveEndpoint() {
    var selectedCave = document.querySelector("#oneModalDynamicContent select").value;

    fetch("/Game/ChooseCaveToDiscard?player=" + playerId + "&id=" + selectedCave, {
        method: "POST"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}

/*
this is called when a player is forced to discard something, most
often as a cost to an action, and the only possible choice is one 
of the basic resources.
*/
function handleDiscardResource() {
    chooseOneModal.style.display = "block";
    var oneModalSubmit = document.getElementById("oneModalSubmit");
    oneModalSubmit.onclick = routeOneModalSubmit;

    var skipBtn = document.getElementById("oneModalSkip");
    skipBtn.disabled = false;
    skipBtn.onclick = callSkipEndpoint;
    var cancelBtn = document.getElementById("oneModalClose");
    cancelBtn.disabled = true;

    var text = document.getElementById("oneModalDynamicText");

    text.innerHTML = `
    <b>${statePlayerName}:</b>
    <b>Select a resource to pay</b>`;

    var container = document.getElementById("oneModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");
    var addedAny = false;

    Object.keys(gameData.game_stack_frame.resources).forEach(key => {
    if (gameData.game_stack_frame.resources[key] === true) {

        var option = document.createElement("option");

        // Capitalize first letter
        var formatted = key.charAt(0).toUpperCase() + key.slice(1);

        if (key.valueOf() == "meat"){
            formatted += " 🍖"
        } else if (key.valueOf() == "amethyst") {
            formatted += " 🌑"
        } else if (key.valueOf() == "gold") {
            formatted += " ⚜️"
        } else if (key.valueOf() == "milk") {
            formatted += " 🥛"
        } else if (key.valueOf() == "eggs") {
            formatted += " 🥚"
        } else if (key.valueOf() == "coins") {
            formatted += " 💰"
        }

        option.value = key;
        option.textContent = formatted;

        dropdown.appendChild(option);
        addedAny = true;
    }
    });

    if(!addedAny) {
        var option = document.createElement("option");
        option.value = "meat";
        option.textContent = "Meat";
        dropdown.append(option);
    }

    container.appendChild(dropdown);
}

/*
whenever a player gains some benefit (resource, dragon, or cave) that is not just 
one of the basic resources, this function is called to present them with all possible choices.
If they may choose a dragon, all dragons from the shop will be added to the modal, and likewise
with caves. 
*/
function handleGainBenefit() {
    console.log("benefit gained");
    chooseOneModal.style.display = "block";
    var oneModalSubmit = document.getElementById("oneModalSubmit");
    oneModalSubmit.onclick = routeOneModalSubmit;

    var skipBtn = document.getElementById("oneModalSkip");
    skipBtn.disabled = false;
    skipBtn.onclick = callSkipEndpoint;
    var cancelBtn = document.getElementById("oneModalClose");
    cancelBtn.disabled = true;

    var text = document.getElementById("oneModalDynamicText");

    text.innerHTML = `
    <b>${statePlayerName}:</b>
    <b>Select a benefit to gain!</b>`;

    var container = document.getElementById("oneModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");
    var addedAny = false;

    Object.keys(gameData.game_stack_frame.resources).forEach(key => {
    if (gameData.game_stack_frame.resources[key] === true) {

        var option = document.createElement("option");

        // Capitalize first letter
        var formatted = key.charAt(0).toUpperCase() + key.slice(1);

        if (key.valueOf() == "meat"){
            formatted += " 🍖"
        } else if (key.valueOf() == "amethyst") {
            formatted += " 🌑"
        } else if (key.valueOf() == "gold") {
            formatted += " ⚜️"
        } else if (key.valueOf() == "milk") {
            formatted += " 🥛"
        } else if (key.valueOf() == "eggs") {
            formatted += " 🥚"
        } else if (key.valueOf() == "coins") {
            formatted += " 💰"
        }
        
        option.value = "resource:" + key;
        option.textContent = formatted;

        dropdown.appendChild(option);
        addedAny = true;
    }
    });
    if(gameData.game_stack_frame.canChooseDragon){
        console.log("dragon choosable when gaining benefit");
        gameData.board.dragonShop.forEach(dragon => {
        var option = document.createElement("option"); 

        option.value = "dragon:" + dragon.id;      // useful for later lookup
        option.textContent = dragon.name;

        dropdown.appendChild(option);
    });
    }

    if(gameData.game_stack_frame.canChooseCave){
        console.log("cave choosable when gaining benefit");
        gameData.board.caveShop.forEach(cave => {
        var option = document.createElement("option");

        option.value = "cave:" + cave.id;      // useful for later lookup
        option.textContent = cave.action.description;

        dropdown.appendChild(option);
    });
    }

    container.appendChild(dropdown);
}

/*
when a player is forced to discard something that is not just some resource, 
this is called to present them a choice of what to discard. They will be presented
with each benefit they could be allowed to discard, which could be any resource, dragon,
or cave card. If a player has 0 of a resource they will be allowed to select it but if
they select to discard a resource they have 0 of, they will be prompted with the choice
again
*/
function handleDiscardBenefit() {
    console.log("benefit gained");
    chooseOneModal.style.display = "block";
    var oneModalSubmit = document.getElementById("oneModalSubmit");
    oneModalSubmit.onclick = routeOneModalSubmit;

    var skipBtn = document.getElementById("oneModalSkip");
    skipBtn.disabled = false;
    skipBtn.onclick = callSkipEndpoint;
    var cancelBtn = document.getElementById("oneModalClose");
    cancelBtn.disabled = true;

    var text = document.getElementById("oneModalDynamicText");

    text.innerHTML = `
    <b>${statePlayerName}:</b>
    <b>Select a benefit to gain!</b>`;

    var container = document.getElementById("oneModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");
    var addedAny = false;

    Object.keys(gameData.game_stack_frame.resources).forEach(key => {
    if (gameData.game_stack_frame.resources[key] === true) {

        var option = document.createElement("option");

        // Capitalize first letter
        var formatted = key.charAt(0).toUpperCase() + key.slice(1);

        option.value = "resource:" + key;
        option.textContent = formatted;

        dropdown.appendChild(option);
        addedAny = true;
    }
    });
    if(gameData.game_stack_frame.canChooseDragon){
        console.log("dragon choosable when gaining benefit");
        gameData.board.dragonShop.forEach(dragon => {
        var option = document.createElement("option"); 

        option.value = "dragon:" + dragon.id;      // useful for later lookup
        option.textContent = dragon.name;

        dropdown.appendChild(option);
    });
    }

    if(gameData.game_stack_frame.canChooseCave){
        console.log("cave choosable when gaining benefit");
        gameData.board.caveShop.forEach(cave => {
        var option = document.createElement("option");

        option.value = "cave:" + cave.id;      // useful for later lookup
        option.textContent = cave.action.description;

        dropdown.appendChild(option);
    });
    }

    container.appendChild(dropdown);
}

/*
whenever a player gains some kind of resource or is allowed to choose some resource to gain,
this function is called so that they can choose which resource to gain. The player is only 
allowed to select resources that they are allowed to gain from their action, so in the event
that the action rewarded them a particular resource such as meat, meat will be the only option
*/
function handleGainResource() {
    chooseOneModal.style.display = "block";
    var oneModalSubmit = document.getElementById("oneModalSubmit");
    oneModalSubmit.onclick = routeOneModalSubmit;

    var skipBtn = document.getElementById("oneModalSkip");
    skipBtn.disabled = true;
    var cancelBtn = document.getElementById("oneModalClose");
    cancelBtn.disabled = true;

    var text = document.getElementById("oneModalDynamicText");

    text.innerHTML = `
    <b>${statePlayerName}:</b>
    <b>Select a resource to gain!</b>`;

    var container = document.getElementById("oneModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");
    var addedAny = false;

    Object.keys(gameData.game_stack_frame.resources).forEach(key => {
    if (gameData.game_stack_frame.resources[key] === true) {

        var option = document.createElement("option");

        // Capitalize first letter
        var formatted = key.charAt(0).toUpperCase() + key.slice(1);

        if (key.valueOf() == "meat"){
            formatted += " 🍖"
        } else if (key.valueOf() == "amethyst") {
            formatted += " 🌑"
        } else if (key.valueOf() == "gold") {
            formatted += " ⚜️"
        } else if (key.valueOf() == "milk") {
            formatted += " 🥛"
        } else if (key.valueOf() == "eggs") {
            formatted += " 🥚"
        } else if (key.valueOf() == "coins") {
            formatted += " 💰"
        }

        option.value = key;
        option.textContent = formatted;

        dropdown.appendChild(option);
        addedAny = true;
    }
    });

    if(!addedAny) {
        var option = document.createElement("option");
        option.value = "meat";
        option.textContent = "Meat";
        dropdown.append(option);
    }

    container.appendChild(dropdown);
}

/*
when the player presses the explore button in order to get rewards from their caves, 
this function is called in order to let the player choose which cavern to explore
*/
function handleExploreBtn() {
    if (currState != "AWAIT_PLAYER_ACTION") {
        postDisplayUpdates();
        return;
    }
    chooseOneModal.style.display = "block";
    var oneModalSubmit = document.getElementById("oneModalSubmit");
    oneModalSubmit.onclick = routeOneModalSubmit;

    var skipBtn = document.getElementById("oneModalSkip");
    skipBtn.disabled = true;
    var cancelBtn = document.getElementById("oneModalClose");
    cancelBtn.disabled = false;
    

    var text = document.getElementById("oneModalDynamicText");

    text.innerHTML = `
    <b>${statePlayerName}:</b>
    <b>Select a layer to explore!</b>`;

    var container = document.getElementById("oneModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");

    var options = ["Crimson Cavern", "Golden Grotto", "Amethyst Abyss"];

    options.forEach(opt => {
        var option = document.createElement("option");
        option.value = opt;
        option.textContent = opt;
        dropdown.appendChild(option);
    });

    container.appendChild(dropdown);
}

/*
when the player presses excavate in order to create a cave, this function is called 
in order to give them a modal to choose which cave to create
*/
function handleExcavateBtn() {
    if (currState != "AWAIT_PLAYER_ACTION") {
        postDisplayUpdates();
        return;
    }
    enticing = false;
    chooseTwoModal.style.display = "block";
    var twoModalSubmit = document.getElementById("twoModalSubmit");
    twoModalSubmit.onclick = routeTwoModalSubmit;

    var skipBtn = document.getElementById("twoModalSkip");
    skipBtn.disabled = true;
    var cancelBtn = document.getElementById("twoModalClose");
    cancelBtn.disabled = false;
    

    var text = document.getElementById("twoModalDynamicText");

    text.innerHTML = `
    <b>${statePlayerName}:</b>
    <b>Select a layer and cave to excavate!</b>`;

    var container = document.getElementById("twoModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");
    var dropdown2 = document.createElement("select");

    var options = ["Crimson Cavern", "Golden Grotto", "Amethyst Abyss"];

    options.forEach(opt => {
        var option = document.createElement("option");
        option.value = opt;
        option.textContent = opt;
        dropdown.appendChild(option);
    });

    gameData.players[statePlayer].cave_hand.forEach(cave => {
        var option = document.createElement("option");

        option.value = cave.id;      // useful for later lookup
        option.textContent = cave.action.description;

        dropdown2.appendChild(option);
    });

    container.appendChild(dropdown);
    container.appendChild(dropdown2);
}

/*
when the player presses entice in order to summon a dragon, this function is called 
in order to give them a modal to choose which dragon to summon
*/
function handleEnticeBtn() {
    if (currState != "AWAIT_PLAYER_ACTION") {
        postDisplayUpdates();
        return;
    }
    enticing = true;
    chooseTwoModal.style.display = "block";
    var twoModalSubmit = document.getElementById("twoModalSubmit");
    twoModalSubmit.onclick = routeTwoModalSubmit;

    var skipBtn = document.getElementById("twoModalSkip");
    skipBtn.disabled = true;
    var cancelBtn = document.getElementById("twoModalClose");
    cancelBtn.disabled = false;
    

    var text = document.getElementById("twoModalDynamicText");

    text.innerHTML = `
    <b>${statePlayerName}:</b>
    <b>Select a layer and dragon to entice!</b>`;

    var container = document.getElementById("twoModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");
    var dropdown2 = document.createElement("select");

    var options = ["Crimson Cavern", "Golden Grotto", "Amethyst Abyss"];

    options.forEach(opt => {
        var option = document.createElement("option");
        option.value = opt;
        option.textContent = opt;
        dropdown.appendChild(option);
    });

    gameData.players[statePlayer].dragon_hand.forEach(dragon => {
        var option = document.createElement("option");

        option.value = dragon.id;      // useful for later lookup
        option.textContent = dragon.name + " | " + getCostEmoji(dragon);

        dropdown2.appendChild(option);
    });

    container.appendChild(dropdown);
    container.appendChild(dropdown2);
}

/*
when a game action occurs, some AWAIT is added to the stack to be resolved, 
this function is then called and calls a necessary handle function in order to 
resolve the action the player took
*/
function postDisplayUpdates() {
    const isMyTurn = Number(statePlayer) === Number(playerId);

    exploreBtn.disabled = !isMyTurn;
    excavateBtn.disabled = !isMyTurn;
    enticeBtn.disabled = !isMyTurn;
    universalSkipBtn.disabled = !isMyTurn;

    if (!isMyTurn) {
        return;
    }

    switch(currState) {
        case "AWAIT_GET_RESOURCE":
            handleGainResource();
            break;
        case "AWAIT_DISCARD_RESSOURCE":
            handleDiscardResource();
            break;
        case "AWAIT_GET_DRAGON":
            handleGainDragon();
            break;
        case "AWAIT_DISCARD_DRAGON":
            handleDiscardDragon();
            break;
        case "AWAIT_GET_CAVE":
            handleGainCave();
            break;
        case "AWAIT_DISCARD_CAVE":
            handleDiscardCave();
            break;
        case "AWAIT_GAIN_BENEFIT":
            handleGainBenefit();
            break;
        case "AWAIT_DISCARD_BENEFIT":
            handleDiscardBenefit();
            break;
        default:
            break;
    }
    
}
/*
function that can be called in order to update the viewer on the information of the board
*/
function updateDisplay() {
    fetch("/Game/GetBoard", {
        method: "GET"
    })
    .then(response => response.text())
    .then(data => {
        // console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}

/*
gets the information about the board from the backend
*/
function getData() {
    fetch("/Game/GetBoard", {
        method: "GET"
    })
    .then(response => response.text())
    .then(data => {
        console.log(JSON.parse(data));
        displayBoardInfo(JSON.parse(data));
    })
    .catch(error => {
        console.error("Error:", error);
    });
}

/*
adds emojis to the dragon card sprites in order to represent the cost of the dragon
*/
function getCostEmoji(dragon) {
    let result = "";

    const add = (count, emoji) => {
        for (let i = 0; i < count; i++) {
            result += emoji;
        }
    };

    add(dragon.coinCost, "💰");
    add(dragon.meatCost, "🍖");
    add(dragon.goldCost, "⚜️");
    add(dragon.amethystCost, "🌑");
    add(dragon.milkCost, "🥛");

    return result || "Free";
}

/* 
function that displays all of the information on the website other than modal popups
data is a variable that essentially contains all of the information that is manipulated and set up in the backend
*/
function displayBoardInfo(data) {
    statePlayer = data.game_stack_frame.state_player;
    currState = data.game_stack_frame.state;
    turnPlayer = data.active_player;
    statePlayerName = data.players[statePlayer].name;
    turnPlayerName = data.players[turnPlayer].name;
    gameData = data;
    document.getElementById("coinCounter").innerHTML = "Coins: " + data.players[playerId].resources.coins;
    document.getElementById("meatCounter").innerHTML = "Meat: " + data.players[playerId].resources.meat;
    document.getElementById("goldCounter").innerHTML = "Gold: " + data.players[playerId].resources.gold;
    document.getElementById("amethystCounter").innerHTML = "Amethyst: " + data.players[playerId].resources.amethyst;
    document.getElementById("milkCounter").innerHTML = "Milk: " + data.players[playerId].resources.milk;
    document.getElementById("eggCounter").innerHTML = "Eggs: " + data.players[playerId].resources.eggs;
    document.getElementById("reputationCounter").innerHTML = "Reputation: " + data.players[playerId].resources.reputation;
    document.getElementById("playerIndicator").innerHTML = "Turn: " + data.players[turnPlayer].name;
    document.getElementById("playerIndicator2").innerHTML = "Acting: " + data.players[statePlayer].name;
    document.getElementById("selfPlayerIndicator").innerHTML = "You Are: " + data.players[playerId].name;

    let dragonNames = document.getElementsByName("dragonName");

    for (let i = 0; i < dragonNames.length; i++) {
        try {
            dragonNames[i].innerHTML = data.players[playerId].dragon_hand[i].name;
        } catch {
            dragonNames[i].innerHTML = "";
        }
    }

    let dragonDescs = document.getElementsByName("dragonDesc");

    for (let i = 0; i < dragonDescs.length; i++) {
        try {
            dragonDescs[i].innerHTML = data.players[playerId].dragon_hand[i].action.description;
        } catch {
            dragonDescs[i].innerHTML = "";
        }
    }

    let dragonCosts = document.getElementsByName("dragonCost");

    for (let i = 0; i < dragonCosts.length; i++) {
        try {
            dragonCosts[i].innerHTML = getCostEmoji(data.players[playerId].dragon_hand[i]);
        } catch {
            dragonCosts[i].innerHTML = "";
        }
    }

    let caveDescs = document.getElementsByName("caveDesc");

    for (let i = 0; i < caveDescs.length; i++) {
        try {
            caveDescs[i].innerHTML = data.players[playerId].cave_hand[i].action.description;
        } catch {
            caveDescs[i].innerHTML = "";
        }
    }

    let shopDragonNames = document.getElementsByName("shopDragonName");

    for (let i = 0; i < shopDragonNames.length; i++) {
        try {
            shopDragonNames[i].innerHTML = data.board.dragonShop[i].name;
        } catch {
            shopDragonNames[i].innerHTML = "";
        }
    }

    let shopDragonDescs = document.getElementsByName("shopDragonDesc");

    for (let i = 0; i < shopDragonDescs.length; i++) {
        try {
            shopDragonDescs[i].innerHTML = data.board.dragonShop[i].action.description;
        } catch {
            shopDragonDescs[i].innerHTML = "";
        }
    }

    let shopDragonCosts = document.getElementsByName("shopDragonCost");

    for (let i = 0; i < shopDragonCosts.length; i++) {
        try {
            shopDragonCosts[i].innerHTML = getCostEmoji(data.board.dragonShop[i]);
        } catch {
            shopDragonCosts[i].innerHTML = "";
        }
    }

    let shopCaveDescs = document.getElementsByName("shopCaveDesc");

    for (let i = 0; i < shopCaveDescs.length; i++) {
        try {
            shopCaveDescs[i].innerHTML = data.board.caveShop[i].action.description;
        } catch {
            shopCaveDescs[i].innerHTML = "";
        }
    }

    // display caves
    for (let cavern = 0; cavern < 3; cavern++) {
        for (let column = 0; column < 4; column++) {
            // console.log(data.players[0].mat.caverns[cavern].caves[column]);
            try {
                document.getElementById("cavern-slot-"+cavern+"-"+column).innerHTML = `
                    <img src = "/images/caveCorner.png" alt= "Cave"/>
                    <div class="cavern-slot-text">${data.players[playerId].mat.caverns[cavern].caves[column].action.description}</div>
                `;
            } catch {
                document.getElementById("cavern-slot-"+cavern+"-"+column).innerHTML = "";
            }
        }
    }

    // display dragons on top of caves, overriding caves that have dragons and leaving caves that don't
    for (let cavern = 0; cavern < 3; cavern++) {
        for (let column = 0; column < 4; column++) {
            try {
                document.getElementById("cavern-slot-"+cavern+"-"+column).innerHTML = `
                    <img src = "/images/marbelDragon.png" alt = "Marble Dragon" />
                    <div class="card-text"><p>${data.players[playerId].mat.caverns[cavern].dragons[column].name}<br>
                    ${data.players[playerId].mat.caverns[cavern].dragons[column].action.description}</p></div>
                `;
            } catch {
                //do nothing
            }
        }
    }

    postDisplayUpdates();
}