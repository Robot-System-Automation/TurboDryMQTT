"use strict";

const topicElementMap = {
    "rsa/738/TD/op_station_number": "stationNrLabel",
    "rsa/738/TD/st1_lev2_lf": "st1lvl2lf",
    "rsa/738/TD/st1_lev2_rg": "st1lvl2rg",
    "rsa/738/TD/st1_lev1_lf": "st1lvl1lf",
    "rsa/738/TD/st1_lev1_rg": "st1lvl1rg",
    "rsa/738/TD/st2_lev2_lf": "st2lvl2lf",
    "rsa/738/TD/st2_lev2_rg": "st2lvl2rg",
    "rsa/738/TD/st2_lev1_lf": "st2lvl1lf",
    "rsa/738/TD/st2_lev1_rg": "st2lvl1rg",
    "rsa/738/TD/st3_lev2_lf": "st3lvl2lf",
    "rsa/738/TD/st3_lev2_rg": "st3lvl2rg",
    "rsa/738/TD/st3_lev1_lf": "st3lvl1lf",
    "rsa/738/TD/st3_lev1_rg": "st3lvl1rg",
    "rsa/738/TD/st4_lev2_lf": "st4lvl2lf",
    "rsa/738/TD/st4_lev2_rg": "st4lvl2rg",
    "rsa/738/TD/st4_lev1_lf": "st4lvl1lf",
    "rsa/738/TD/st4_lev1_rg": "st4lvl1rg",
    "rsa/738/TD/st5_lev2_lf": "st5lvl2lf",
    "rsa/738/TD/st5_lev2_rg": "st5lvl2rg",
    "rsa/738/TD/st5_lev1_lf": "st5lvl1lf",
    "rsa/738/TD/st5_lev1_rg": "st5lvl1rg",
    "rsa/738/TD/st6_lev2_lf": "st6lvl2lf",
    "rsa/738/TD/st6_lev2_rg": "st6lvl2rg",
    "rsa/738/TD/st6_lev1_lf": "st6lvl1lf",
    "rsa/738/TD/st6_lev1_rg": "st6lvl1rg",
    "rsa/738/TD/sole_presence_lf": "solePresenceLf",
    "rsa/738/TD/sole_presence_rg": "solePresenceRg",
    "rsa/738/TD/list_pos1": "listPos1",
    "rsa/738/TD/list_pos2": "listPos2",
    "rsa/738/TD/list_pos3": "listPos3",
    "rsa/738/TD/list_pos4": "listPos4",
    "rsa/738/TD/list_pos5": "listPos5",
    "rsa/738/TD/list_pos6": "listPos6",
    "rsa/738/TD/list_pos7": "listPos7",
    "rsa/738/TD/list_pos8": "listPos8",
    "rsa/738/TD/list_pos9": "listPos9",
    "rsa/738/TD/list_pos10": "listPos10",
    "rsa/738/TD/list_pos11": "listPos11",
    "rsa/738/TD/list_pos12": "listPos12",
};

// Connect to the SignalR hub
var connection = new signalR.HubConnectionBuilder()
    .withUrl(`${window.location.origin}/mqttHub`)
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Listen for messages from the backend
connection.on("ReceiveMessage", function (topic, message) {

    if (document.getElementById("messagesList")) {
        var li = document.createElement("li");
        li.textContent = `Topic: ${topic}, Message: ${message}`;
        document.getElementById("messagesList").appendChild(li);
    }

    if (topic === "rsa/738/TD/op_station_number") {
        document.getElementById("stationNrLabel").textContent = message;

        const cards = document.querySelectorAll('.card');

        cards.forEach(card => {
            const stationId = card.getAttribute('data-station-id');
            const statusMessage = card.querySelector('.status-message'); // Select the message element inside the card
            
            console.log(stationId);
            if (message === stationId) {
                card.classList.add('active');
                statusMessage.style.display = 'block'; // Make it visible
            } else {
                card.classList.remove('active'); 
                statusMessage.style.display = 'none'; // Hide it

            }
        });
    }

    if (topicElementMap[topic]) {
        // Update the corresponding element's text content
        document.getElementById(topicElementMap[topic]).textContent = message;
    }
    else {
        console.warn(`Element not found for topic: ${topic}`);
    }
});

// Start the SignalR connection
connection.start().then(function () {
    console.log("Connected to SignalR hub!");
    console.log("just test");
}).catch(function (err) {
    console.error("Error connecting to SignalR:", err.toString());
});
