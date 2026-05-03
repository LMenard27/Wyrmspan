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

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/displayHub")
    .build();

connection.on("updateDisplay", function (data) {
    console.log("Recieved websocket push");
    updateDisplay(data);
});

connection.on("assignPlayer", function (id) {
    playerId = id;
    console.log("You are Player:", playerId);
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

function handleDiscardCave() {
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
    <b>Select a cave to discard</b>`;

    var container = document.getElementById("oneModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");

    gameData.players[statePlayer].cave_hand.forEach(cave => {
        var option = document.createElement("option");

        option.value = cave.id;      // useful for later lookup
        option.textContent = cave.action.description;

        dropdown.appendChild(option);
    });

    container.appendChild(dropdown);
}

function handleGainCave() {
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
    <b>Select a cave to gain!</b>`;

    var container = document.getElementById("oneModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");

    gameData.board.caveShop.forEach(cave => {
        var option = document.createElement("option");

        option.value = cave.id;      // useful for later lookup
        option.textContent = cave.action.description;

        dropdown.appendChild(option);
    });

    container.appendChild(dropdown);
}

function handleDiscardDragon() {
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
    <b>Select a dragon to discard</b>`;

    var container = document.getElementById("oneModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");

    gameData.players[statePlayer].dragon_hand.forEach(dragon => {
        var option = document.createElement("option");

        option.value = dragon.id;      // useful for later lookup
        option.textContent = dragon.name;

        dropdown.appendChild(option);
    });

    container.appendChild(dropdown);
}

function handleGainDragon() {
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
    <b>Select a dragon to gain!</b>`;

    var container = document.getElementById("oneModalDynamicContent");

    // Clear previous content
    container.innerHTML = "";

    // Create dropdown
    var dropdown = document.createElement("select");

    gameData.board.dragonShop.forEach(dragon => {
        var option = document.createElement("option");

        option.value = dragon.id;      // useful for later lookup
        option.textContent = dragon.name;

        dropdown.appendChild(option);
    });

    container.appendChild(dropdown);
}

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